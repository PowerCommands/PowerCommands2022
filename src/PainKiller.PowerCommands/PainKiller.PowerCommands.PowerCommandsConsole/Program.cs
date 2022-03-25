// See https://aka.ms/new-console-template for more information

using PainKiller.PowerCommands.Bootstrap;

Console.WriteLine("Try to read configuration");
var configuration = Startup.Initialize();
Console.WriteLine($"{configuration.Metadata.Name} {configuration.Metadata.Description}");
foreach (var command in configuration.Commands)
{
    Console.WriteLine(command);
}
Console.ReadLine();

