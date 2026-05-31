using System.Text.Json.Serialization;

namespace login_system.Common
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}