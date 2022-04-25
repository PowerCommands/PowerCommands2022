﻿using PainKiller.PowerCommands.Configuration.Contracts;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Configuration.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PainKiller.PowerCommands.Configuration;

public class ConfigurationService : IConfigurationService
{
    private ConfigurationService(){}

    private static readonly Lazy<IConfigurationService> Lazy = new(() => new ConfigurationService());
    public static IConfigurationService Service => Lazy.Value;
    public YamlContainer<T> Get<T>(string inputFileName = "") where T : new()
    {
        var fileName = string.IsNullOrEmpty(inputFileName) ? $"{typeof(T).Name}.yaml".GetSafePathRegardlessHowApplicationStarted() : inputFileName;
        var yamlContent = File.ReadAllText(fileName);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<YamlContainer<T>>(yamlContent);
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
    public YamlContainer<T> GetAppDataConfiguration<T>(T defaultIfMissing, string inputFileName = "") where T : new()
    {
        var directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{nameof(PowerCommands)}";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        var fileName = Path.Combine(directory, inputFileName);
        if (!File.Exists(fileName))
        {
            var yaml = CreateContent(defaultIfMissing);
            File.WriteAllText(fileName, yaml);
        }
        
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