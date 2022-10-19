using System.Text;
using System.Text.Json;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Services;

public class StorageService<T> : IStorageService<T> where T : new()
{
    private StorageService() { }

    private static readonly Lazy<IStorageService<T>> Lazy = new(() => new StorageService<T>());
    public static IStorageService<T> Service => Lazy.Value;

    public string StoreObject(T storeObject)
    {
        var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"{typeof(T).Name}.data");
        var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
        var jsonString = JsonSerializer.Serialize(storeObject, options);
        File.WriteAllText(fileName, jsonString, Encoding.Unicode);
        return fileName;
    }

    public string DeleteObject()
    {
        var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"{typeof(T).Name}.data");
        File.Delete(fileName);
        return fileName;
    }

    public T GetObject()
    {
        var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"{typeof(T).Name}.data");
        var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
        if (!File.Exists(fileName)) return new T();
        var jsonString = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<T>(jsonString, options) ?? new T();
    }

    public string Backup()
    {
        var d = DateTime.Now;
        var fileName = $"{typeof(T).Name}";
        var sourceFilePath = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"{fileName}.data");
        var backupFilePath = Path.Combine(IPowerCommandServices.DefaultInstance!.Configuration.BackupPath, $"{fileName}-{d.Year}{d.Month}{d.Day}{d.Hour}{d.Minute}{d.Second}.data");
        var content = File.ReadAllText(sourceFilePath);
        File.WriteAllText(backupFilePath, content);
        return backupFilePath;
    }
}