namespace PowerCommands.WebShared.Contracts;
public interface ILocalStorageService
{
    Task<T?> GetItemAsync<T>(string key) where T : new();
    Task SetItem<T>(string key, T value);
    Task RemoveItem(string key);
}