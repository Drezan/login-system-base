namespace login_system.DTOs.Responses
{
    public class AuthReponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
