using System.Text;
using PainKiller.PowerCommands.Configuration.Contracts;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Configuration.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PainKiller.PowerCommands.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private ConfigurationService() { }

        private static readonly Lazy<IConfigurationService> Lazy = new(() => new ConfigurationService());
        public static IConfigurationService Service => Lazy.Value;
        public YamlContainer<T> Get<T>(string inputFileName = "") where T : new()
        {
            var fileName = string.IsNullOrEmpty(inputFileName) ? $"{typeof(T).Name}.yaml".GetSafePathRegardlessHowApplicationStarted() : inputFileName.GetSafePathRegardlessHowApplicationStarted();
            var yamlContent = File.ReadAllText(fileName);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            try
            {
                return deserializer.Deserialize<YamlContainer<T>>(yamlContent);
            }
            catch (Exception)
            {
                Console.WriteLine($"Could not deserialize the configuration file, default configuration will be loaded instead\nA template configuration file named default_{typeof(T).Name}.yaml will be created in application root.");
                var defaultConfig = new T();
                SaveChanges(defaultConfig, $"default_{typeof(T).Name}.yaml");
                return new YamlContainer<T>();
            }
        }
        public YamlContainer<T> GetByNodeName<T>(string filePath, string nodeName) where T : new()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("version: 1.0\r\nconfiguration:");
            var nodeFound = false;
            var nodeIndentation = 0;

            using (var reader = new StreamReader(filePath))
            {
                while (reader.ReadLine() is { } line)
                {
                    if (line.Trim().StartsWith(nodeName + ":"))
                    {
                        nodeFound = true;
                        nodeIndentation = line.TakeWhile(Char.IsWhiteSpace).Count();
                        stringBuilder.AppendLine(line);
                        continue;
                    }
                    if (!nodeFound) continue;
                    var currentIndentation = line.TakeWhile(Char.IsWhiteSpace).Count();
                    if (string.IsNullOrWhiteSpace(line) || currentIndentation <= nodeIndentation) break;
                    stringBuilder.AppendLine(line);
                }
            }

            var yamlContent = stringBuilder.ToString();
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            try
            {
                return deserializer.Deserialize<YamlContainer<T>>(yamlContent);
            }
            catch (Exception)
            {
                Console.WriteLine($"Could not deserialize the configuration file, default configuration will be loaded instead\nA template configuration file named default_{typeof(T).Name}.yaml will be created in application root.");
                return new YamlContainer<T>();
            }

        }
        public string SaveChanges<T>(T configuration, string inputFileName = "") where T : new()
        {
            if (configuration is null) return "";
            var fileName = string.IsNullOrEmpty(inputFileName) ? $"{configuration.GetType().Name}.yaml".GetSafePathRegardlessHowApplicationStarted() : inputFileName.GetSafePathRegardlessHowApplicationStarted();

            var yamlContainer = new YamlContainer<T> { Configuration = configuration, Version = "1.0" };
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yamlData = serializer.Serialize(yamlContainer);
            File.WriteAllText(fileName, yamlData);
            return fileName;
        }
        public void Create<T>(T configuration, string fullFileName) where T : new()
        {
            if (configuration is null) return;
            var yamlContainer = new YamlContainer<T> { Configuration = configuration, Version = "1.0" };
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yamlData = serializer.Serialize(yamlContainer);
            File.WriteAllText(fullFileName, yamlData);
        }

        /// <summary>
        /// Return a configuration file stored in the AppData/Roaming/PowerCommands directory, if the file does not exist it will be created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultIfMissing"></param>
        /// <param name="inputFileName"></param>
        /// <returns></returns>
        public YamlContainer<T> GetAppDataConfiguration<T>(string inputFileName = "") where T : new()
        {
            var directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{nameof(PowerCommands)}";
            var fileName = Path.Combine(directory, inputFileName);
            if (!File.Exists(fileName)) return new YamlContainer<T>();
            var yamlContent = File.ReadAllText(fileName);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            return deserializer.Deserialize<YamlContainer<T>>(yamlContent);
        }
        private string CreateContent<T>(T item) where T : new()
        {
            if (item is not null)
            {
                var yamlContainer = new YamlContainer<T> { Configuration = item, Version = "1.0" };
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                var yamlData = serializer.Serialize(yamlContainer);
                return yamlData;
            }
            return "--- item is null and can not be serialized ---";
        }
    }
}