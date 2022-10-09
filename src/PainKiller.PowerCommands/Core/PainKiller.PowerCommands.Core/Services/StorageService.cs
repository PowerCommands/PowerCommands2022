using System.Text;
using System.Text.Json;
using PainKiller.PowerCommands.Configuration.DomainObjects;

namespace PainKiller.PowerCommands.Core.Services;

public static class StorageService
{
    public static string StoreObject<T>(T storeObject) where T : new()
    {
        var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"{typeof(T).Name}.data");
        var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
        var jsonString = JsonSerializer.Serialize(storeObject, options);
        File.WriteAllText(fileName, jsonString, Encoding.Unicode);
        return fileName;
    }

    public static T GetObject<T>() where T : new()
    {
        var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"{typeof(T).Name}.data");
        var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
        var jsonString = File.ReadAllText(fileName);
        return File.Exists(fileName) ? JsonSerializer.Deserialize<T>(jsonString, options) ?? new T() : new T();
    }
}