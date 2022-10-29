using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.WindowsCommands.Configuration;
using PainKiller.PowerCommands.WindowsCommands.Enums;

namespace PainKiller.PowerCommands.WindowsCommands.Commands;

[PowerCommand(description: "Starts favorites AC, Cheat and Steam (Steam only if you want to).\nYou could provide a name for your game to start as a Quote parameter\nFavorite must be defined in the favorites section in the PowerCommandsConfiguration.yaml file",
    arguments: "steam",
    qutes:"<Favorite name>",
    example: "game|game steam|game \"Forza\"|game steam \"Forza\"")]
public class GameCommand : StartCommand
{
    public GameCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var steam = FindFavorite($"{GameFavorites.Steam}");
        var ac = FindFavorite($"{GameFavorites.Ac}");
        var cheat = FindFavorite($"{GameFavorites.Cheat}");
        var wheel = FindFavorite($"{GameFavorites.Wheel}");

        if (Input.Arguments.Any(a => a.ToLower().Contains($"{GameFavorites.Wheel}".ToLower())) && wheel != null) Start(wheel);
        if (Input.Arguments.Any(a => a.ToLower().Contains($"{GameFavorites.Steam}".ToLower())) && steam != null) Start(steam);

        if (ac != null) Start(ac);
        if (cheat != null) Start(cheat);

        if (string.IsNullOrEmpty(Input.SingleQuote)) return CreateRunResult();
        
        var game = FindFavorite(Input.SingleQuote);
        if (game != null) Start(game);
        return CreateRunResult();
    }
}