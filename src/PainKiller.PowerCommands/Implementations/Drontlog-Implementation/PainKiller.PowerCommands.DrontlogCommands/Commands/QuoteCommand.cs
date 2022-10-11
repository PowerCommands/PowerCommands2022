using System.Net;
using System.Text.Json;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

public class QuoteCommand : CommandBase<PowerCommandsConfiguration>
{
    public QuoteCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        using var webClient = new WebClient { BaseAddress = "https://favqs.com/" };
        var response = webClient.DownloadStringTaskAsync("api/qotd").Result;
        var quote = JsonSerializer.Deserialize<QuoteOfTheDay>(response);
        if (quote == null) return CreateBadParameterRunResult("No qute downloded");
        
        Console.WriteLine(quote.quote.body);
        return CreateRunResult();
    }
}