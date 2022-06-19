using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.WindowsCommands.Configuration;
using PainKiller.PowerCommands.WindowsCommands.Enums;

namespace PainKiller.PowerCommands.WindowsCommands.Commands;

[Tags("example|shell|execute|program")]
[PowerCommand(description: "Shows how to execute a external program in combination with some custom configuration.\nFavorite must be defined in the favorites section in the PowerCommandsConfiguration.yaml file",
    arguments: "<favorite name>",
    argumentMandatory: true,
    example: "start music|start games")]
public class GameCommand : FavoriteCommand
{
    public GameCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var steam = FindFavorite($"{GameFavorites.Steam}");
        var ac = FindFavorite($"{GameFavorites.AC}");
        var cheat = FindFavorite($"{GameFavorites.Cheat}");

        if (input.SingleArgument.ToLower() == $"{GameFavorites.Steam}".ToLower() && steam != null) Start(steam);
        if (ac != null) Start(ac);
        if (cheat != null) Start(cheat);

        if (!string.IsNullOrEmpty(input.SingleQuote))
        {
            var game = FindFavorite(input.SingleQuote);
            if (game != null) Start(game);
        }
        return CreateRunResult(input);
    }
}