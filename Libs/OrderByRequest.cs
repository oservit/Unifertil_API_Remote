using System.Text.Json;

namespace Libs
{
    public class OrderByRequest
    {
        public string? SortProperty { get; set; }
        public string? SortDirection { get; set; }

        public OrderByRequest() { }

        public OrderByRequest(string? orderByJson)
        {
            if (string.IsNullOrEmpty(orderByJson))
                return;

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                };

                var deserializedRequest = JsonSerializer.Deserialize<OrderByRequest>(orderByJson, options);

                if (deserializedRequest != null)
                {
                    SortProperty = deserializedRequest.SortProperty;
                    SortDirection = deserializedRequest.SortDirection;
                }
                else
                {
                    throw new ArgumentException("Deserialized object is null.");
                }
            }
            catch (JsonException ex)
            {
                throw new ArgumentException($"Invalid order by format: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An unexpected error occurred while deserializing: {ex.Message}", ex);
            }
        }

        public bool ValidateRequest()
        {
            if (string.IsNullOrEmpty(SortProperty))
                return false;

            if (string.IsNullOrEmpty(SortDirection))
                return false;

            return true;
        }
    }
}