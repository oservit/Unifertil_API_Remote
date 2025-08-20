using System.Text.Json.Serialization;

namespace Application.Services.Core
{
    public class ApiResponse<T>
    {

        [JsonPropertyName("pageIndex")]
        public long PageIndex { get; set; }

        [JsonPropertyName("pageSize")]
        public long PageSize { get; set; }

        [JsonPropertyName("totalCount")]
        public long TotalCount { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
}
