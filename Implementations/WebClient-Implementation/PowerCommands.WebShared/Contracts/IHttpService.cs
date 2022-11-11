namespace PowerCommands.WebShared.Contracts;
public interface IHttpService
{
    Task<T> Delete<T>(string uri) where T : new();
    Task<T> Get<T>(string uri) where T : new();
    Task<string> GetAsString(string uri);
    Task<string> GetAsStringWithCustomHeaders(string uri, IDictionary<string, string> headers);
    Task<T> Post<T>(string uri, object value) where T : new();
    Task<T> Put<T>(string uri, object value) where T : new();
    Task<T> Login<T>(string uri, object value) where T : new();
}