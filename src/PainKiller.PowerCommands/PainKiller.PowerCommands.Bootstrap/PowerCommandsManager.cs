using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Bootstrap;

public class PowerCommandsManager : IPowerCommandsManager
{
    public readonly IExtendedPowerCommandServices<PowerCommandsConfiguration> Services;
    public PowerCommandsManager(IExtendedPowerCommandServices<PowerCommandsConfiguration> services) { Services = services; }
    public void Run()
    {
        RunResult runResult = null!;
        while (runResult is not {Status: RunResultStatus.Quit})
        {
            try
            {
                var input = ReadLine.ReadLineService.Service.Read(prompt:"\npcm>");
                var interpretedInput = $"{input}".Interpret();
                Services.Logger.LogInformation($"Console input Identifier:{interpretedInput.Identifier} raw:{interpretedInput.Raw}");
                Services.Diagnostic.Start();
                runResult = Services.Runtime.ExecuteCommand($"{input}");
                RunResultHandler(runResult);
                Services.Diagnostic.Stop();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Could not found any commands with a matching Id, please try again...");
                Services.Logger.LogError(ex, "Could not found any commands with a matching Id");
            }
            catch (Exception e)
            {
                Services.Logger.LogError(e,"Unkown error");
                Console.WriteLine("Unknown error occured, please try again");
            }
        }
    }
    private void RunResultHandler(RunResult runResult)
    {
        Services.Logger.LogInformation($"Command {runResult.ExecutingCommand?.Identifier} run with input: [{runResult.Input.Raw}] output: [{runResult.Output.Trim()}] status: [{runResult.Status}]");
        switch (runResult.Status)
        {
            case RunResultStatus.Quit:
                Services.Logger.LogInformation($"Command return status Quit, program execution ends...");
                break;
            case RunResultStatus.ArgumentError:
            case RunResultStatus.ExceptionThrown:
            case RunResultStatus.SyntaxError:
                var message = $"Error occured of type {runResult.Status}";
                Services.Logger.LogError(message);
                Console.WriteLine(message);
                Console.WriteLine(runResult.Output);
                break;
            case RunResultStatus.RunExternalPowerCommand:
            case RunResultStatus.Initializing:
            case RunResultStatus.Ok:
            default:
                break;
        }
    }
}