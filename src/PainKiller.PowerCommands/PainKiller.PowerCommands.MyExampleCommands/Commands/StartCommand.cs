using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("example|shell|execute|program")]
[PowerCommand(description: "Shows how to execute a external program in combination with some custom configuration.\nFavorite must be defined in the favorites section in the PowerCommandsConfiguration.yaml file",
                arguments: "<favorite name>",
        argumentMandatory: true,
                  example: "start music|start games")]
public class StartCommand : CommandBase<PowerCommandsConfiguration>
{
    public StartCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var favorite = Configuration.Favorites.FirstOrDefault(f => f.Name.ToLower() == Input.SingleArgument);
        if (favorite == null) return CreateBadParameterRunResult("No matching favorite found in configuration file");

        ShellService.Service.Execute(favorite.NameOfExecutable, arguments: "", workingDirectory: "",WriteLine, favorite.FileExtension);
        return CreateRunResult();
    }
}