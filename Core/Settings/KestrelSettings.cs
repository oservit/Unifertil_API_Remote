using System.Text.Json.Serialization;

namespace Domain.Settings
{
    [Serializable]
    public class KestrelSettings
    {
        [JsonPropertyName("Endpoints")]
        public EndpointSettings Endpoints { get; set; }
    }

    [Serializable]
    public class EndpointSettings
    {
        [JsonPropertyName("Https")]
        public HttpsSettings Https { get; set; }
    }

    [Serializable]
    public class HttpsSettings
    {
        [JsonPropertyName("Url")]
        public string Url { get; set; }

        [JsonPropertyName("Certificate")]
        public CertificateSettings Certificate { get; set; }
    }

    [Serializable]
    public class CertificateSettings
    {
        [JsonPropertyName("Path")]
        public string Path { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
