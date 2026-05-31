using Microsoft.EntityFrameworkCore;
using login_system.Common;
using login_system.Data;
using login_system.DTOs.Requests;
using login_system.DTOs.Responses;
using login_system.Entities;

namespace login_system.Services.Interfaces
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RoleReponse> CreateRoleAsync(CreateRoleRequest request)
        {
            var exists = await _context.Roles.AnyAsync(r => r.Name == request.Name);

            if (exists)
                throw new AppException("Role already exists.", 400);

            var role = new Role
            {
                Name = request.Name
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleReponse
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role is null)
                throw new AppException("Role not found.", 404);

            _context.Roles.Remove(role);

            await _context.SaveChangesAsync();
        }

        public async Task<List<RoleReponse>> GetRolesAsync()
        {
            return await _context.Roles.Select(r => new RoleReponse
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync();
        }
    }
}
