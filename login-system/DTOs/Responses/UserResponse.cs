namespace login_system.DTOs.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}