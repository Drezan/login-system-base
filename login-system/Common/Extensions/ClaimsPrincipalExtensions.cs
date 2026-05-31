using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace login_system.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId (this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Invalid token.");

            return Guid.Parse(userId);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }
    }
}