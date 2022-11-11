using System.Text.Json;
using Microsoft.JSInterop;

namespace PowerCommands.WebClient.Services;
public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;
    public LocalStorageService(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;
    public async Task<T?> GetItemAsync<T>(string key) where T : new()
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        if (string.IsNullOrEmpty(json)) return new T();
        return JsonSerializer.Deserialize<T>(json);
    }
    public async Task SetItem<T>(string key, T value) => await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
    public async Task RemoveItem(string key) => await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
}