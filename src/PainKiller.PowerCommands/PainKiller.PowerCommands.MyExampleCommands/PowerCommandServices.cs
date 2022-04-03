using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.Extensions;
using PainKiller.PowerCommands.Core;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.SerilogExtensions.Managers;

namespace PainKiller.PowerCommands.MyExampleCommands;

public class PowerCommandServices : IPowerCommandsService<PowerCommandsConfiguration>
{
    private PowerCommandServices()
    {
        var configuration = ConfigurationManager.Get<PowerCommandsConfiguration>().Configuration;
        var logger = GetLoggerManager.GetFileLogger(configuration.Log.FileName.GetSafePathRegardlessHowApplicationStarted("logs"));
        var pcr = new PowerCommandsRuntime<PowerCommandsConfiguration>(configuration);

        Runtime = pcr;
        Configuration = configuration;
        Logger = logger;
    }
    
    
    private static readonly Lazy<PowerCommandServices> _service = new(() => new PowerCommandServices());
    public static PowerCommandServices Service => _service.Value;
    public IPowerCommandsRuntime Runtime { get; }
    public PowerCommandsConfiguration Configuration { get; }
    public ILogger Logger { get; }
}