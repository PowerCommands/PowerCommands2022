using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.MyExampleCommands;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Bootstrap;

public class PowerCommandsManager : IPowerCommandsManager
{
    public readonly IPowerCommandsService<PowerCommandsConfiguration> _services;
    public PowerCommandsManager(PowerCommandServices service) { _services = service; }
    public void Run()
    {
        RunResult runResult = null!;
        while (runResult is not {Status: RunResultStatus.Quit})
        {
            try
            {
                Console.Write("\npcm>");
                var input = Console.ReadLine();
                var interpretedInput = $"{input}".Interpret();
                _services.Logger.LogInformation($"Console input Identifier:{interpretedInput.Identifier} raw {interpretedInput.Raw}");
                _services.Diagnostic.Start();
                runResult = _services.Runtime.ExecuteCommand($"{input}");
                _services.Diagnostic.Stop();
                _services.Logger.LogInformation($"Command {runResult.ExecutingCommand?.Identifier} run with input: [{runResult.Input.Raw}] output: [{runResult.Output}] status: [{runResult.Status}]");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Could not found any commands with a matching Id, please try again...");
                _services.Logger.LogError(ex, "Could not found any commands with a matching Id");
            }
            catch (Exception e)
            {
                _services.Logger.LogError(e,"Unkown error");
                Console.WriteLine("Unknown error occured, please try again");
            }
        }
    }
}