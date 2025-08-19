using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Settings
{
    [Serializable]
    public class AppSettings
    {
        [JsonPropertyName("Central")]
        public CentralSettings Central { get; set; } = new CentralSettings();

        [JsonPropertyName("Jwt")]
        public JwtSettings Token { get; set; } = new JwtSettings();

        [JsonPropertyName("Cors")]
        public CorsSettings Cors { get; set; } = new CorsSettings();

        [JsonPropertyName("Kestrel")]
        public KestrelSettings Kestrel { get; set; } = new KestrelSettings();

        [JsonPropertyName("Connection")]
        public required List<ConnectionSettings> Connections { get; set; }

        public HttpUser HttpUser { get; set; } = new HttpUser();
    }
}
