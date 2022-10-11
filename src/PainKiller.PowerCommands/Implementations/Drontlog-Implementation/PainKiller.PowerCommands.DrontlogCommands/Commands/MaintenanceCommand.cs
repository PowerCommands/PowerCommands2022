namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

[Tags("maintenance|util")]
[PowerCommand(description: $"Maintenance jobs, add news post added to the WriterTable, delete rows older then 31 days in table [{nameof(PostCache)}]")]
public class MaintenanceCommand : CommandBase<PowerCommandsConfiguration>
{
    public MaintenanceCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var writerInputCommand = new WriterInputCommand(Identifier, Configuration);
        var cleanUpCommand = new CleanupCommand(Identifier, Configuration);
        var runJob = true;
        var iterationCount = 0;
        while (runJob)
        {
            WriteLine($"{DateTime.Now} Running maintenance job");
            writerInputCommand.InitializeRun(new CommandLineInput { Arguments = new[] { "" } });
            writerInputCommand.Run();
            if (iterationCount % 10 == 0)
            {
                writerInputCommand.InitializeRun(new CommandLineInput { Arguments = new[] { "delete" } });
                writerInputCommand.Run();
                cleanUpCommand.InitializeRun(new CommandLineInput());
                cleanUpCommand.Run();
            }
            Console.WriteLine();
            Console.WriteLine("Waiting 60 seconds...");
            Console.WriteLine();
            Thread.Sleep(60000);
            iterationCount++;
        }
        return CreateRunResult();
    }
}