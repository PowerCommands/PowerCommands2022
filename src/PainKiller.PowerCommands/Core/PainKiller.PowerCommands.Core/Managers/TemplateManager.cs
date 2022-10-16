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
        if (Directory.Exists(_path)) Directory.Delete(_path);
        Directory.CreateDirectory(_path);
        _logger.Invoke($"Template directory {_path} initialized", DisplayAndWriteToLog);
    }
    public void CopyTemplates() => Directory.Move(Path.Combine(GetSrcCodeDownloadPath(), "PowerCommands2022\\\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.MyExampleCommands\\Commands\\Templates"), _path);

    private string GetSrcCodeDownloadPath() => _path.Replace("\\download", "\\templates");
}