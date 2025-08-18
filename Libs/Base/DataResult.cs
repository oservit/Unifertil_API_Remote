namespace Libs.Base
{

    public class DataResult
    {
        public object? Data { get; set; }
        public bool Success { get; set; } = false;

        public string? Message { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;
    }
}