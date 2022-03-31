using System.Reflection;
using PainKiller.PowerCommands.Bootstrap;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

try
{
    //ConfigurationManager.Update(new PowerCommandsConfiguration{Log = new LogComponentConfiguration(),Metadata = new Metadata{Name = "Default",Description = "Description"},ShowDiagnosticInformation = true,Components = new List<BaseComponentConfiguration>(){new BaseComponentConfiguration {Name = "Name",Checksum = "--",Component = "babar.dll"}}},"default.yaml");
    
    var configuration = Startup.Initialize();
    var runCommandStatus = new RunResult { Status = RunResultStatus.Ok };
    var command = ReflectionManager.GetCommands(Assembly.Load($"{configuration.Components.Last().Component}".Replace(".dll", "")), configuration).First();
    while (runCommandStatus.Status != RunResultStatus.Quit) runCommandStatus = command.Run($"{Console.ReadLine()}");
}
catch (Exception e)
{
    Console.WriteLine($"Critical error, program could not start, exception message:{e.Message})");
    Console.ReadLine();
}