using PainKiller.PowerCommands.Bootstrap;
using PainKiller.PowerCommands.MyExampleCommands.Commands;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

try
{
    Startup.Initialize();
    var command = new EncryptionCommand("babar");
    
    var runCommand = new RunResult{Status = RunResultStatus.Ok};
    while (runCommand.Status != RunResultStatus.Quit) runCommand = command.Run(Console.ReadLine());
    
}
catch { Console.WriteLine("Critical error, program could not start, check the log for more details"); }