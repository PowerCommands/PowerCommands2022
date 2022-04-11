using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.Extensions;
using PainKiller.PowerCommands.Core;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.ReadLine;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.SerilogExtensions.Managers;

namespace PainKiller.PowerCommands.MyExampleCommands;

public class PowerCommandServices : IExtendedPowerCommandServices<PowerCommandsConfiguration>
{
    private PowerCommandServices()
    {
        ExtendedConfiguration = ConfigurationManager.Get<PowerCommandsConfiguration>().Configuration;
        Diagnostic = new DiagnosticManager(ExtendedConfiguration.ShowDiagnosticInformation);
        Runtime = new PowerCommandsRuntime<PowerCommandsConfiguration>(ExtendedConfiguration, Diagnostic); 
        Logger = GetLoggerManager.GetFileLogger(ExtendedConfiguration.Log.FileName.GetSafePathRegardlessHowApplicationStarted(ExtendedConfiguration.Log.FilePath),ExtendedConfiguration.Log.RollingIntervall,ExtendedConfiguration.Log.RestrictedToMinimumLevel);
        
        var suggestions = new List<string>(Runtime.CommandIDs);
        suggestions.AddRange(Runtime.CommandIDs.Select(s => $"describe {s}").ToList());
        suggestions.AddRange(Runtime.Commands.Where(c => !string.IsNullOrEmpty(c.GetDefaultParameter())).Select(c => $"{c.Identifier} {c.GetDefaultParameter()}").ToList());

        ReadLineService.InitializeAutoComplete(history: new string[]{},suggestions: suggestions.ToArray());
    }

    private static readonly Lazy<IExtendedPowerCommandServices<PowerCommandsConfiguration>> Lazy = new(() => new PowerCommandServices());
    
    // TODO:Could I use interface here? Problem with extension in Bootstrap
    public static IExtendedPowerCommandServices<PowerCommandsConfiguration> Service => Lazy.Value;
    
    public IPowerCommandsRuntime Runtime { get; }
    public ICommandsConfiguration Configuration => ExtendedConfiguration as ICommandsConfiguration;
    public PowerCommandsConfiguration ExtendedConfiguration { get; }
    public ILogger Logger { get; }
    public IDiagnosticManager Diagnostic { get; }
}