namespace Infrastructure.Http
{
    public interface IApiClient
    {
        Task<T?> GetAsync<T>(string url, string? bearerToken = null);
        Task<T?> PostAsync<T>(string url, object data, string? bearerToken = null);
        Task<T?> PutAsync<T>(string url, object data, string? bearerToken = null);
        Task<bool> DeleteAsync(string url, string? bearerToken = null);
    }
}
