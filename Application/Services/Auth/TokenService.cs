using Application.Services.Core;
using Infrastructure.Http;
using Libs;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IApiClient _apiClient;
        private readonly IConfiguration _config;
        private string? _cachedToken;
        private DateTime _expiresAt;

        public TokenService(IApiClient apiClient, IConfiguration config)
        {
            _apiClient = apiClient;
            _config = config;
        }

        public async Task<string> GetTokenAsync(RemoteCredentials credentials)
        {
            try
            {
                if (!string.IsNullOrEmpty(_cachedToken) && _expiresAt > DateTime.UtcNow.AddMinutes(1))
                    return _cachedToken;

                var user = new
                {
                    username = credentials.User,
                    password = Crypto.Decrypt(credentials.Password)
                };

                var response = await _apiClient.PostAsync<ApiResponse<string>>(
                    $"{credentials.BaseUrl}/Auth/GetToken",
                    user
                );

                if (response == null || !response.Success)
                    throw new Exception($"Erro ao autenticar na API Destino: {response?.Message}");

                _cachedToken = response.Data;
                _expiresAt = DateTime.UtcNow.AddMinutes(30);

                return _cachedToken;
            }
            catch
            {
                throw;
            }
        }
    }
}
