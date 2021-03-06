using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Extensions;
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
public class StartCommand : CommandBase<PowerCommandsConfiguration>
{
    public StartCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleArgument))
        {
            Show();
            return CreateRunResult(input);
        }

        var favorite = FindFavorite(input.SingleArgument);
        if (favorite != null) Start(favorite);
        return CreateRunResult(input);
    }
    protected FavoriteConfiguration? FindFavorite(string name)
    {
        var favorite = Configuration.Favorites.FirstOrDefault(f => f.Name.ToLower() == name.ToLower());
        if (favorite == null) WriteLine($"Favorite {name} has no match in the configuration file.");
        return favorite;
    }

    protected void Start(FavoriteConfiguration favorite) => ShellService.Service.Execute(favorite.NameOfExecutable, arguments: "", workingDirectory: $"{favorite.WorkingDirectory}", WriteLine, favorite.FileExtension);

    private void Show()
    {
        WriteHeadLine("Favorites\n");
        foreach (var favorite in Configuration.Favorites) this.WriteObjectDescription(favorite.Name, $"Shell executes: {favorite.NameOfExecutable}");
    }
}