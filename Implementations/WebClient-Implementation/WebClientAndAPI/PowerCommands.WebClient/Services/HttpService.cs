using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using PowerCommands.WebShared.DomainObjects;

namespace PowerCommands.WebClient.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        protected readonly NavigationManager NavigationManager;
        protected readonly ClientConfiguration Configuration;
        public HttpService(HttpClient httpClient, NavigationManager navigationManager, IConfiguration configuration)
        {
            _httpClient = httpClient;
            NavigationManager = navigationManager;
            Configuration = configuration.Get<ClientConfiguration>();
        }
        public async Task<T> Delete<T>(string uri) where T : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            return await SendRequest<T>(request);
        }
        public async Task<string> GetAsString(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await SendStringRequest(request);
        }
        public async Task<string> GetAsStringWithCustomHeaders(string uri, IDictionary<string,string> headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            foreach (var header in headers) request.Headers.Add(header.Key, header.Value);

            return await SendStringRequest(request, addTicket: false);
        }
        public async Task<T> Get<T>(string uri) where T : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await SendRequest<T>(request);
        }
        public async Task<T> Post<T>(string uri, object value) where T : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri) {Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")};
            return await SendRequest<T>(request);
        }
        public async Task<T> Login<T>(string uri, object value) where T : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri) {Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")};
            return await SendRequest<T>(request, addTicket: false);
        }
        public async Task<T> Put<T>(string uri, object value) where T : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri) {Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")};
            return await SendRequest<T>(request);
        }
        // helper methods
        private async Task<T> SendRequest<T>(HttpRequestMessage request, bool addTicket = true) where T : new()
        {
            try
            {
                using var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return new T();
                }
                return await response.Content.ReadFromJsonAsync<T>() ?? new T();
            }
            catch (Exception)
            {
                return new T();
            }
        }
        private async Task<string> SendStringRequest(HttpRequestMessage request, bool addTicket = true)
        {
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}