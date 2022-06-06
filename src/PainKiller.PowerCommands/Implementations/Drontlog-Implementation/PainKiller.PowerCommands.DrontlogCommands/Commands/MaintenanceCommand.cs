namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

public class MaintenanceCommand : CommandBase<PowerCommandsConfiguration>
{
    public MaintenanceCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var writerInputCommand = new WriterInputCommand(Identifier, Configuration);
        var cleanUpCommand = new CleanupCommand(Identifier, Configuration);
        var runJob = true;
        var iterationCount = 0;
        while (runJob)
        {
            WriteLine($"{DateTime.Now} Running maintenance job");
            writerInputCommand.Run(new CommandLineInput{ Arguments = new[] { "" } });
            if (iterationCount % 10 == 0)
            {
                writerInputCommand.Run(new CommandLineInput { Arguments = new[] { "delete" } });
                cleanUpCommand.Run(new CommandLineInput());
            }
            Console.WriteLine();
            Console.WriteLine("Waiting 60 seconds...");
            Console.WriteLine();
            Thread.Sleep(60000);
            iterationCount++;
        }
        return CreateRunResult(input);
    }
}