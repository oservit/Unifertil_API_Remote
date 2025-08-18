namespace Infrastructure.Logging
{
    public interface IAppLogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
    }

    public class ConsoleAppLogger : IAppLogger
    {
        public void LogInfo(string message) => Console.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
        public void LogWarning(string message) => Console.WriteLine($"[WARN] {DateTime.Now:HH:mm:ss} - {message}");
        public void LogError(string message) => Console.WriteLine($"[ERRO] {DateTime.Now:HH:mm:ss} - {message}");
    }
}
