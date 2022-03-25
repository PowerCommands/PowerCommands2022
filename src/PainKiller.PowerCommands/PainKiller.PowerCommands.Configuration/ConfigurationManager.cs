using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Configuration.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PainKiller.PowerCommands.Configuration;

public static class ConfigurationManager
{
    public static YamlContainer<T> GetConfiguration<T>(string inputFileName = "") where T : new()
    {
        var fileName = string.IsNullOrEmpty(inputFileName) ? $"{typeof(T).Name}.yaml".GetSafePathRegardlessHowApplicationStarted() : inputFileName;
        var yamlContent = File.ReadAllText(fileName);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<YamlContainer<T>>(yamlContent) ?? new YamlContainer<T>();
    }
}