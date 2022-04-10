using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.Extensions;
using PainKiller.PowerCommands.Core;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.ReadLine;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.SerilogExtensions.Managers;

namespace PainKiller.PowerCommands.MyExampleCommands;

public class PowerCommandServices : IPowerCommandsService<PowerCommandsConfiguration>
{
    private PowerCommandServices()
    {
        Configuration = ConfigurationManager.Get<PowerCommandsConfiguration>().Configuration; 
        Diagnostic = new DiagnosticManager(Configuration.ShowDiagnosticInformation);
        Runtime = new PowerCommandsRuntime<PowerCommandsConfiguration>(Configuration, Diagnostic); 
        Logger = GetLoggerManager.GetFileLogger(Configuration.Log.FileName.GetSafePathRegardlessHowApplicationStarted(Configuration.Log.FilePath),Configuration.Log.RollingIntervall,Configuration.Log.RestrictedToMinimumLevel);
        ReadLineService.InitializeAutoComplete(history: new string[]{},suggestions: Runtime.CommandIDs);
    }

    private static readonly Lazy<PowerCommandServices> Lazy = new(() => new PowerCommandServices());
    
    // TODO:Could I use interface here? Problem with extension in Bootstrap
    public static PowerCommandServices Service => Lazy.Value;
    
    public IPowerCommandsRuntime Runtime { get; }
    public PowerCommandsConfiguration Configuration { get; }
    public ILogger Logger { get; }
    public IDiagnosticManager Diagnostic { get; }
}