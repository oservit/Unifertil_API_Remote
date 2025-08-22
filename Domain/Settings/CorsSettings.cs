namespace Domain.Settings
{
    [Serializable]
    public class CorsSettings
    {
        public List<string> AllowedOrigins { get; set; } = new List<string>();
    }
}
