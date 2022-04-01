using PainKiller.PowerCommands.Bootstrap;

try
{
    var app = Startup.Initialize();
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine($"Critical error, program could not start, exception message:{e.Message})");
    Console.ReadLine();
}