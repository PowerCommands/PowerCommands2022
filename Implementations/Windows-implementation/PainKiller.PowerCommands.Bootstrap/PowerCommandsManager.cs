using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Core.Commands;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;
using PainKiller.PowerCommands.WindowsCommands.Configuration;

namespace PainKiller.PowerCommands.Bootstrap;

public class PowerCommandsManager : IPowerCommandsManager
{
    public readonly IExtendedPowerCommandServices<PowerCommandsConfiguration> Services;
    public PowerCommandsManager(IExtendedPowerCommandServices<PowerCommandsConfiguration> services) { Services = services; }
    public void Run(string[] args)
    {
        var runAutomatedAtStartup = args.Length > 0;
        var runResultStatus = RunResultStatus.Initializing;
        var input = "";
        while (runResultStatus is not RunResultStatus.Quit)
        {
            try
            {
                var promptText = runResultStatus == RunResultStatus.Async ? "" : $"\n{ConfigurationGlobals.Prompt}";
                input = runAutomatedAtStartup ? string.Join(' ', args) : ReadLine.ReadLineService.Service.Read(prompt: promptText);
                if (string.IsNullOrEmpty(input.Trim())) continue;
                var interpretedInput = input.Interpret();
                if (runAutomatedAtStartup)
                {
                    Services.Diagnostic.Message($"Started up with args: {interpretedInput.Raw}");
                    ConsoleService.Write($"{nameof(PowerCommandsManager)}", ConfigurationGlobals.Prompt, null);
                    ConsoleService.Write($"{nameof(PowerCommandsManager)} automated startup", $"{interpretedInput.Identifier}", ConsoleColor.Blue);
                    ConsoleService.WriteLine($"{nameof(PowerCommandsManager)} automated startup", interpretedInput.Raw.Replace($"{interpretedInput.Identifier}", ""), null);
                }
                runAutomatedAtStartup = false;
                Services.Logger.LogInformation($"Console input Identifier:{interpretedInput.Identifier} raw:{interpretedInput.Raw}");
                Services.Diagnostic.Start();
                var runResult = Services.Runtime.ExecuteCommand($"{input}");
                runResultStatus = runResult.Status;
                RunResultHandler(runResult);
                Services.Diagnostic.Stop();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                var commandsCommand = new CommandsCommand("commands", (Services.Configuration as CommandsConfiguration)!);
                var interpretedInput = input.Interpret();
                ConsoleService.WriteError(GetType().Name, $"Could not found any commands with a matching Id: {interpretedInput.Raw} and there is no defaultCommand defined in configuration or the defined defaultCommand does not exist.");
                commandsCommand.InitializeAndValidateInput(interpretedInput);
                commandsCommand.Run();
                Services.Logger.LogError(ex, "Could not found any commands with a matching Id");
            }
            catch (Exception e)
            {
                Services.Logger.LogError(e, "Unknown error");
                ConsoleService.WriteError(GetType().Name, "Unknown error occurred, please try again");
            }
        }
    }
    private void RunResultHandler(RunResult runResult)
    {
        Services.Logger.LogInformation($"Command {runResult.ExecutingCommand.Identifier} run with input: [{runResult.Input.Raw}] output: [{runResult.Output.Trim()}] status: [{runResult.Status}]");
        Services.Diagnostic.Message($"Input: {runResult.Input.Raw} Output: {runResult.Output} Status: {runResult.Status}");
        switch (runResult.Status)
        {
            case RunResultStatus.Quit:
                Services.Logger.LogInformation($"Command return status Quit, program execution ends...");
                break;
            case RunResultStatus.ArgumentError:
            case RunResultStatus.ExceptionThrown:
            case RunResultStatus.InputValidationError:
            case RunResultStatus.SyntaxError:
                var message = $"Error occurred of type {runResult.Status}";
                Services.Logger.LogError(message);
                ConsoleService.WriteError(GetType().Name, $"{message} {runResult.Output}");
                HelpService.Service.ShowHelp(runResult.ExecutingCommand, clearConsole: false);
                break;
            case RunResultStatus.RunExternalPowerCommand:
            case RunResultStatus.Initializing:
            case RunResultStatus.Ok:
            case RunResultStatus.Help:
            default:
                break;
        }
    }
}