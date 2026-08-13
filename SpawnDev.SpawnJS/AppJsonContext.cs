using SpawnDev.SpawnJS.Marshallers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS
{
    [JsonSerializable(typeof(HeapViewDescriptor))]
    internal partial class AppJsonContext : JsonSerializerContext
    {
        private static readonly JsonSerializerOptions _options;
        static AppJsonContext()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = Default
            };
            _options.TypeInfoResolver = Default;
        }
        public static JsonSerializerOptions JsonSerializerOptions => _options;
        public static string Serialize<T>(T data) => JsonSerializer.Serialize(data, _options);
        public static T Deserialize<T>(string data) => data == null ? default! : JsonSerializer.Deserialize<T>(data, _options)!;
        public static void Init() { }
    }
}
