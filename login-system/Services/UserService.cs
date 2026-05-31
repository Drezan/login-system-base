using Microsoft.EntityFrameworkCore;
using login_system.Common;
using login_system.Data;
using login_system.DTOs.Requests;
using login_system.DTOs.Responses;
using login_system.Entities;
using login_system.Services.Interfaces;

namespace login_system.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        public UserService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<UserResponse> CreateUserAsync(RegisterUserRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
                throw new AppException("User with this email already exists.", 409);

            List<Role> roles;

            if (request.RoleIds is not null && request.RoleIds.Any())
            {
                roles = await _context.Roles
                    .Where(x => request.RoleIds.Contains(x.Id))
                    .ToListAsync();

                if (roles.Count != request.RoleIds.Count)
                {
                    throw new AppException(
                        "One or more roles are invalid.",
                        400);
                }
            }
            else
            {
                var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                if (defaultRole == null)
                    throw new AppException("Default role not found.", 500);
                roles = new List<Role> { defaultRole };
            }

            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var role in roles)
            {
                user.UserRoles.Add(new UserRole
                {
                    RoleId = role.Id
                });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToUserResponse(user);
        }

        public async Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new AppException("User not found.", 404);

            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email && u.Id != userId);

            if (emailExists)
                throw new AppException("Another user with this email already exists.", 409);

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (request.RoleIds is not null)
            {
                var roles = await _context.Roles.Where(x => request.RoleIds.Contains(x.Id)).ToListAsync();

                if (roles.Count != request.RoleIds.Count)
                    throw new AppException("One or more roles are invalid.", 400);

                user.UserRoles.Clear();

                foreach (var role in roles)
                {
                    user.UserRoles.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                }
            }

            await _context.SaveChangesAsync();

            return MapToUserResponse(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
                throw new AppException("User not found.", 404);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null)
                throw new AppException("Users not found.");
            
            return users.Select(MapToUserResponse).ToList();
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new AppException("User not found.");
            else
                return MapToUserResponse(user);
        }

        public async Task<AuthReponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
                throw new AppException("Invalid credentials.", 401);

            var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!validPassword)
                throw new AppException("Invalid credentials.", 401);

            var token = _jwtService.GenerateToken(user);

            return new AuthReponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(60)
            };
        }

        public async Task UpdatePasswordAsync(Guid id, UpdatePasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                throw new AppException("User not found.", 404);

            var validPassword = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password);

            if (!validPassword)
                throw new AppException("Current password is incorrect.", 401);

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            await _context.SaveChangesAsync();
        }

        #region Mapper
        private static UserResponse MapToUserResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            };
        }
        #endregion
    }
}