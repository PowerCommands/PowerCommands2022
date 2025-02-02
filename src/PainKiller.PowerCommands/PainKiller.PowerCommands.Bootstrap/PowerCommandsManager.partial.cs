using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.ReadLine.Managers;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Bootstrap;
public partial class PowerCommandsManager
{
    private static bool _hasRunOnce = false;
    private void RunCustomCode(RunFlowManager runFlow)
    {
        //Add your custom code here, som example code below shows an example on how could find a command an run a method, could be a startup toolbar for example.
        //This example is using the configuration startupToolbar in the PowerCommandsConfiguration.yaml file.
        if (!_hasRunOnce)
        {
            var commands = IPowerCommandServices.DefaultInstance?.Runtime.Commands.Select(c => c.Identifier).ToArray() ?? [];
            SuggestionProviderManager.AppendContextBoundSuggestions("describe", commands);
            _hasRunOnce = true;
        }
    }
}