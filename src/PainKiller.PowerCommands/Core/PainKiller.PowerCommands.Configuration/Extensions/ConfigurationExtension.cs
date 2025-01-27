using PainKiller.PowerCommands.Configuration.DomainObjects;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PainKiller.PowerCommands.Configuration.Extensions
{
    public static class ConfigurationExtension
    {
        public static string GetSafePathRegardlessHowApplicationStarted(this string fileName, string directory = "") => string.IsNullOrEmpty(directory) ? Path.Combine(AppContext.BaseDirectory, fileName) : Path.Combine(AppContext.BaseDirectory, directory, fileName);

        public static string GetPath(this ArtifactPathsConfiguration configuration, string config)
        {
            var paths = config.Split(ConfigurationGlobals.ArraySplitter);
            if (string.IsNullOrEmpty(config)) return "";
            if (paths.Length == 1) return ReplaceTags(config, configuration.Name);
            if (paths.Length == 2) return Path.Combine(ReplaceTags(paths[0], configuration.Name), ReplaceTags(paths[1], configuration.Name));
            return Path.Combine(ReplaceTags(paths[0], configuration.Name), ReplaceTags(paths[1], configuration.Name), ReplaceTags(paths[2], configuration.Name));
        }
        private static string ReplaceTags(string raw, string name) => raw.Replace("{appdata}", AppContext.BaseDirectory).Replace("{name}", name);
        public static string GetYaml<T>(this T configuration) where T : new()
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            return serializer.Serialize(configuration);
        }
        public static T GetObjectFromYaml<T>(this string yaml) where T : new()
        {
            var serializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            return serializer.Deserialize<T>(yaml);
        }
    }
}