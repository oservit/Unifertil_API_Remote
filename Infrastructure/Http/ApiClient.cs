using Infrastructure.Logging;
using Libs.Exceptions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Http
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IAppLogger _logger;

        public ApiClient(HttpClient httpClient, IAppLogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T?> GetAsync<T>(string url, string? bearerToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddBearerToken(request, bearerToken);

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<T>(response, "GET", url);
        }

        public async Task<T?> PostAsync<T>(string url, object data, string? bearerToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(data, _jsonOptions), Encoding.UTF8, "application/json")
            };
            AddBearerToken(request, bearerToken);

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<T>(response, "POST", url, data);
        }

        public async Task<T?> PutAsync<T>(string url, object data, string? bearerToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(data, _jsonOptions), Encoding.UTF8, "application/json")
            };
            AddBearerToken(request, bearerToken);

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<T>(response, "PUT", url, data);
        }

        public async Task<bool> DeleteAsync(string url, string? bearerToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            AddBearerToken(request, bearerToken);

            var response = await _httpClient.SendAsync(request);
            await HandleResponse<string>(response, "DELETE", url);
            return true;
        }

        private void AddBearerToken(HttpRequestMessage request, string? bearerToken)
        {
            if (!string.IsNullOrEmpty(bearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        private async Task<T?> HandleResponse<T>(
            HttpResponseMessage response,
            string method,
            string url,
            object? requestData = null)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInfo($"{method} {url} - Sucesso ({(int)response.StatusCode})");
                return typeof(T) == typeof(string)
                    ? (T)(object)content
                    : JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }
            else
            {
                string? apiMessage = null;

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(content, _jsonOptions);
                    if (errorResponse != null)
                    {
                        apiMessage = errorResponse.Message;
                        _logger.LogWarning($"{method} {url} - API retornou erro {(int)response.StatusCode}: {apiMessage}");
                    }
                }
                catch
                {
                    // tenta extrair manualmente o "message" do JSON cru
                    try
                    {
                        using var doc = JsonDocument.Parse(content);
                        if (doc.RootElement.TryGetProperty("message", out var msgProp))
                            apiMessage = msgProp.GetString();
                    }
                    catch
                    {
                        // ignora, apiMessage continua null
                    }
                }

                if (!string.IsNullOrEmpty(apiMessage))
                    throw new BusinessException(apiMessage) { Code = (int)response.StatusCode };

                // fallback genérico se não conseguimos pegar a mensagem
                _logger.LogError($"{method} {url} - Erro {(int)response.StatusCode}: {response.ReasonPhrase}\n{content}");
                throw new BusinessException("Erro interno ao chamar API remota") { Code = (int)response.StatusCode };
            }
        }
    }
}
