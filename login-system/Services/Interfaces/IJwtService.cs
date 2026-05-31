using login_system.Entities;

namespace login_system.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
