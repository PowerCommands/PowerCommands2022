using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PowerCommands.WebClient;
using MudBlazor.Services;
using PowerCommands.WebClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<ICommandsService, CommandsService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
