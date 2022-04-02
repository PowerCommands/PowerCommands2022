using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Bootstrap;

public class PowerCommandsApplication
{
    public PowerCommandsApplication(IPowerCommandsManager runtime, PowerCommandsConfiguration configuration, ILogger logger)
    {
        Runtime = runtime;
        Configuration = configuration;
        Logger = logger;
    }
    public IPowerCommandsManager Runtime { get;  }
    public PowerCommandsConfiguration Configuration { get;  }
    public ILogger Logger { get;  }

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
                Logger.LogInformation($"Console input Identifier:{interpretedInput.Identifier} raw {interpretedInput.Raw}");
                runResult = Runtime.ExecuteCommand($"{input}");
                Logger.LogInformation($"Command {runResult.ExecutingCommand?.Identifier} run with input: [{runResult.Input.Raw}] output: [{runResult.Output}] status: [{runResult.Status}]");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Could not found any commands with a matching Id, please try again...");
                Logger.LogError(ex, "Could not found any commands with a matching Id");
            }
            catch (Exception e)
            {
                Logger.LogError(e,"Unkown error");
                Console.WriteLine("Unknown error occured, please try again");
            }
        }
    }
}