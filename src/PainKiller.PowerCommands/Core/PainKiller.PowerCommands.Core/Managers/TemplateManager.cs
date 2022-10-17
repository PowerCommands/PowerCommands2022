using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Managers;

public class TemplateManager : ITemplateManager
{
    private readonly string _name;
    private readonly string _path = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, "templates"); 
    private readonly Action<string, bool> _logger;

    public bool DisplayAndWriteToLog = true;

    public TemplateManager(string name, Action<string, bool> logger)
    {
        _name = name;
        _logger = logger;
    }
    public void InitializeTemplatesDirectory()
    {
        if (Directory.Exists(_path)) Directory.Delete(_path, recursive: true);
        Directory.CreateDirectory(_path);
        _logger.Invoke($"Template directory {_path} initialized", DisplayAndWriteToLog);
    }
    public void CopyTemplates() => Directory.Move(Path.Combine(GetSrcCodeDownloadPath(), "PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.MyExampleCommands\\Commands\\Templates"), Path.Combine(_path,"Commands"));

    public void CreateCommand(string templateName, string commandName)
    {
        var filePath = Path.Combine(Path.Combine(_path, "Commands"), $"{templateName}Command.cs");
        if (!File.Exists(filePath))
        {
            _logger.Invoke($"Template not found, run following command to download current templates\npowercommand update --template", DisplayAndWriteToLog);
            return;
        }
        var content = File.ReadAllText(filePath).Replace("namespace PainKiller.PowerCommands.MyExampleCommands.Commands.Templates;", $"namespace PainKiller.PowerCommands.{FindProjectName()}Commands.Commands;").Replace("NameCommand", $"{commandName}Command");
        var commandsFolder = FindCommandsProjectDirectory();
        var copyFilePath = $"{commandsFolder}\\Commands\\{commandName}Command.cs";

        File.WriteAllText(copyFilePath, content);
        _logger.Invoke($"File [{copyFilePath}] created", DisplayAndWriteToLog);
    }
    private string GetSrcCodeDownloadPath() => _path.Replace("\\templates", "\\download");
    private string FindCommandsProjectDirectory() => Directory.GetDirectories(CliManager.GetLocalSolutionRoot()).First(c => c.EndsWith("Commands"));
    private string FindProjectName() => FindCommandsProjectDirectory().Split('\\').Last().Split('.').Last().Replace("Commands","");
}