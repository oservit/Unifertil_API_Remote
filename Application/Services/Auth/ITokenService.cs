namespace Application.Services.Auth
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(RemoteCredentials credentials);
    }
}
