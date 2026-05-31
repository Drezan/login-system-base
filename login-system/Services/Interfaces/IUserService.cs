using login_system.DTOs.Requests;
using login_system.DTOs.Responses;

namespace login_system.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(RegisterUserRequest request);
        Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request);
        Task DeleteUserAsync(Guid userId);
        Task<List<UserResponse>> GetAllUsersAsync();
        Task<UserResponse> GetUserByIdAsync(Guid userId);
        Task<AuthReponse> LoginAsync(LoginRequest request);
        Task UpdatePasswordAsync(Guid userId, UpdatePasswordRequest request);
    }
}