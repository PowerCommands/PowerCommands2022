using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
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
        while (runResult == null || runResult.Status != RunResultStatus.Quit)
        {
            try
            {
                Console.Write("pcm>");
                var input = Console.ReadLine();
                runResult = Runtime.ExecuteCommand($"{input}");
                Logger.LogInformation($"Command {runResult.ExecutingCommand?.Identifier} run with input: [{runResult.Input.Raw}] returning status: [{runResult.Output}]");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Could not found any commands with a matching Id, please try again...");
            }
            catch (Exception e)
            {
                Logger.LogError(e,"Unkown error");
                Console.WriteLine("Unknown error occured, please try again");
            }
        }
    }
}