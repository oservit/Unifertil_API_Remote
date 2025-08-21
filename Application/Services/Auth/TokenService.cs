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
            // Retorna token cacheado se ainda válido
            if (!string.IsNullOrEmpty(_cachedToken) && _expiresAt > DateTime.UtcNow.AddMinutes(1))
                return _cachedToken;

            // Cria o objeto de usuário
            var user = new
            {
                username = credentials.User,
                password = Crypto.Decrypt(credentials.Password)
            };

            // POST para login
            var response = await _apiClient.PostAsync<ApiResponse<string>>(
                $"{credentials.Url}/Auth/GetToken",
                user
            );

            if (response?.Success != true || string.IsNullOrEmpty(response.Data))
                throw new Exception("Falha ao autenticar na API Central");

            // Cache do token com Bearer
            _cachedToken = response.Data;
            _expiresAt = DateTime.UtcNow.AddMinutes(30);

            return _cachedToken;
        }
    }
}
