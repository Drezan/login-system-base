using login_system.DTOs.Requests;
using login_system.DTOs.Responses;

namespace login_system.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleReponse>> GetRolesAsync();
        Task<RoleReponse> CreateRoleAsync(CreateRoleRequest request);
        Task DeleteRoleAsync(int id);
    }
}