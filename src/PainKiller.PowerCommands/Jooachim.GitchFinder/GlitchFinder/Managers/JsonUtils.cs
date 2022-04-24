using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlitchFinder.Managers
{
    public class JsonUtils
    {
        internal static readonly JsonSerializerOptions JsonSerializerOptions = new() { Converters = { new JsonStringEnumConverter() }, WriteIndented = true };

        public static T Load<T>(string fileName) 
        {
            var jsonData = File.ReadAllText(fileName);
            
            T deserialized = JsonSerializer.Deserialize<T>(jsonData, JsonSerializerOptions);
            return deserialized;
        }

        public static void Save<T>(string fileName, T deserialized)
        {
            var jsData = JsonSerializer.Serialize(deserialized, JsonSerializerOptions);
            File.WriteAllText(fileName, jsData);
        }
    }
}
