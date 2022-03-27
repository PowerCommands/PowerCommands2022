using PainKiller.PowerCommands.Bootstrap;

try
{
    Startup.Initialize();
}
catch { Console.WriteLine("Critical error, program could not start, check the log for more details"); }