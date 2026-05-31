namespace login_system.DTOs.Requests
{
    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<int>? RoleIds { get; set; }
    }
}