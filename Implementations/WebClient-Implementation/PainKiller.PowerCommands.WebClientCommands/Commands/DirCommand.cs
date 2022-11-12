namespace PainKiller.PowerCommands.WebClientCommands.Commands;

[PowerCommandDesign(description: "Open a given directory or current working folder if argument is omitted, use flagg --app to open the AppData roaming directory",
                      arguments: "<directory name>",
                          flags: "app",
                        example: "//Open the bin directory where this program resides|dir|//Open a path, you can use code completion with tab, just begin with a valid path first like C:|dir C:\\repos|//Open the AppData roaming directory|dir --app")]
public class DirCommand : WebCommandBase<CommandsConfiguration>
{
    public DirCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var directory = string.IsNullOrEmpty(Input.Path) ? (Input.HasFlag("app") ? ConfigurationGlobals.ApplicationDataFolder : AppContext.BaseDirectory) : Input.Path;
        if (!Directory.Exists(directory)) return BadParameterError($"Could not find directory \"{directory}\"");
        ShellService.Service.OpenDirectory(directory);
        WriteLine($"Open directory {directory}");
        return Ok();
    }
}