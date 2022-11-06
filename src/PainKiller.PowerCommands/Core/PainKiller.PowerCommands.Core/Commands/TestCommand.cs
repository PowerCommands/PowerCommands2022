namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandDesign( description: "Run test on specific command or all commands, the must have a PowerCommandTest attribute declared on class level.",
                           flags: "!command|all|trace",
                         example: "//Test a specific command|test --command commandName|//Test all commands (default) flag could be omitted|test --all")]
public class TestCommand : CommandBase<CommandsConfiguration>
{
    public TestCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        Console.Clear();
        var commands = IPowerCommandsRuntime.DefaultInstance?.Commands ?? new List<IConsoleCommand>();
        if (Input.HasFlag("command"))
        {
            var command = commands.FirstOrDefault(c => c.Identifier == Input.GetFlagValue("command"));
            if (command != null)
            {
                var items = RunTest(command);
                if (!Input.HasFlag("trace")) Console.Clear();

                ConsoleTableService.RenderConsoleCommandTable(items.ToArray(), this);
            }
            else WriteError($"Could not find a command with identity [{Input.GetFlagValue("command")}]");
        }
        return Ok();
    }

    private List<CommandTestItem> RunTest(IConsoleCommand command)
    {
        try
        {
            var attribute = command.GetAttribute<PowerCommandTestAttribute>();
            if (attribute.Disabled)
            {
                WriteLine("Test for this command is not declared or disabled");
                return new List<CommandTestItem>{ new CommandTestItem { Disabled = true } };
            }
            ConsoleService.DisableLog = true;
            var retVal = new List<CommandTestItem>();
            var runtime = IPowerCommandServices.DefaultInstance!.Runtime;
            foreach (var test in attribute.Tests.Split("|"))
            {
                var result = runtime.ExecuteCommand($"{command.Identifier} {test.Replace("!","")}");
                var testItem = new CommandTestItem { ExpectedResult = !test.StartsWith("!"), Status = result.Status, Command = command.Identifier, Test = test };
                retVal.Add(testItem);
            }
            ConsoleService.DisableLog = false;
            return retVal;
        }
        catch
        {
            ConsoleService.DisableLog = false;
            throw;
        }
    }
}