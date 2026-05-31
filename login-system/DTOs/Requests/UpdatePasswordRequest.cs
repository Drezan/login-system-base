namespace login_system.DTOs.Requests
{
    public class UpdatePasswordRequest
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}