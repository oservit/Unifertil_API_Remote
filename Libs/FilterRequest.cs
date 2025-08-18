using System.Text.Json;

namespace Libs
{
    public class FilterRequest
    {
        public string Connector { get; set; }
        public List<FilterByFieldType> Filters { get; set; } = new List<FilterByFieldType>();

        public FilterRequest() { }

        public FilterRequest(string? filterJson)
        {
            if (string.IsNullOrEmpty(filterJson))
                return;

            try
            {
                var deserializedRequest = JsonSerializer.Deserialize<FilterRequest>(filterJson, JsonSerializerOptionsProvider.Default);
                if (deserializedRequest != null)
                {
                    Connector = deserializedRequest.Connector;
                    Filters = deserializedRequest.Filters;
                }
            }

            catch(JsonException ex)
            {
                throw new Exception($"Invalid filter format: {ex.Message}");
            }
        }
    }

    public class FilterByFieldType
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public OperationType Operation { get; set; }
    }



    public enum OperationType
    {
        Equals = 1,
        NotEquals = 2,
        GreaterThan = 3,
        GreaterThanOrEquals = 4,
        LessThan = 5,
        LessThanOrEquals = 6,
        Contains = 7
    }
}
