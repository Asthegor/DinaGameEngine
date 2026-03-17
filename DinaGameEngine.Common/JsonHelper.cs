using System.Text.Json;

namespace DinaGameEngine.Common
{
    public static class JsonHelper
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static T? Deserialize<T>(string json)
            => JsonSerializer.Deserialize<T>(json, Options);

        public static string Serialize<T>(T obj)
            => JsonSerializer.Serialize(obj, Options);
    }
}
