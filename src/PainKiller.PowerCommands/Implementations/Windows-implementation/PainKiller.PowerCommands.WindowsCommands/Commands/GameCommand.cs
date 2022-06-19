using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.WindowsCommands.Configuration;

namespace PainKiller.PowerCommands.WindowsCommands.Commands;

[Tags("example|shell|execute|program")]
[PowerCommand(description: "Shows how to execute a external program in combination with some custom configuration.\nFavorite must be defined in the favorites section in the PowerCommandsConfiguration.yaml file",
    arguments: "<favorite name>",
    argumentMandatory: true,
    example: "start music|start games")]
public class GameCommand : CommandBase<PowerCommandsConfiguration>
{
    public GameCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var favorite = Configuration.Favorites.FirstOrDefault(f => f.Name.ToLower() == input.SingleArgument);
        if (favorite == null) return CreateBadParameterRunResult(input, "No matching favorite found in configuration file");

        ShellService.Service.Execute(favorite.NameOfExecutable, arguments: "", workingDirectory: "", WriteLine, favorite.FileExtension);
        return CreateRunResult(input);
    }
}