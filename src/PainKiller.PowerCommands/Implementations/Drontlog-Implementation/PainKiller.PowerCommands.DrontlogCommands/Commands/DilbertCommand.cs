using System.Net;
using PainKiller.PowerCommands.DrontlogCommands.Extensions;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

public class DilbertCommand : CommandBase<PowerCommandsConfiguration>
{
    public DilbertCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var url = $"https://dilbert.com/strip/{DateTime.Now.ToDateString()}";
        using var webClient = new WebClient { BaseAddress =  url};
        var find = "  <img class=\"img-responsive img-comic\"";
        
        var content = webClient.DownloadString(url);
        var rows = content.Split('\n');
        var row = rows.FirstOrDefault(r => r.Contains(find));

        if (row == null) return CreateBadParameterRunResult("No data found");
        
        // <img class="img-responsive img-comic" width="900" height="280" alt="Keep The Plastic Bag - Dilbert by Scott Adams" src="https://assets.amuniversal.com/ed7c0900f179013aba2a005056a9545d" />
        var imageLink = row.Split("src=")[1];
        imageLink = imageLink.Replace("\"", "").Replace(" />","");
        var htmlContent = $"<img src=\"{imageLink}\" alt=\"Dilbert daily\"/>";

        WriteLine(htmlContent);

        return CreateRunResult();
    }
}