using PainKiller.PowerCommands.Bootstrap;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

try
{
    var configuration = Startup.Initialize();
    var runCommandStatus = new RunResult {Status = RunResultStatus.Ok};
    var command = ReflectionManager.GetCommands(configuration.MyExampleCommand, configuration).First();
    while (runCommandStatus.Status != RunResultStatus.Quit) runCommandStatus = command.Run($"{Console.ReadLine()}");

}
catch (Exception e)
{
    Console.WriteLine($"Critical error, program could not start, exception message:{e.Message})");
}