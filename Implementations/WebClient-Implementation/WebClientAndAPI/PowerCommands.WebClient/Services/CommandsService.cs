using Microsoft.AspNetCore.Components;
using PowerCommands.WebShared.Models;

namespace PowerCommands.WebClient.Services;

public class CommandsService : HttpService, ICommandsService
{
    public CommandsService(HttpClient httpClient, NavigationManager navigationManager, IConfiguration configuration) : base(httpClient, navigationManager, configuration) { }
    public async Task<RunResultModel> GetRunResultModel(InputModel input) => await Post<RunResultModel>($"{Configuration.ApiBaseAddress}/powercommand/", input);
}