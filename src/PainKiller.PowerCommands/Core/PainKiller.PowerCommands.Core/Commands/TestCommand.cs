namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandDesign( description: "Run test on specific command or all commands, the must have a PowerCommandTest attribute declared on class level.",
                           flags: "!command|all",
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
            if (command != null) RunTest(command);
            else WriteError($"Could not find a command with identity [{Input.GetFlagValue("command")}]");
        }
        return Ok();
    }

    private void RunTest(IConsoleCommand command)
    {
        try
        {
            var attribute = command.GetAttribute<PowerCommandTestAttribute>();
            if (attribute.Disabled)
            {
                WriteLine("Test for this command is not declared or disabled");
                return;
            }
            ConsoleService.DisableLog = true;
            var runtime = IPowerCommandServices.DefaultInstance!.Runtime;
            foreach (var test in attribute.Tests.Split("|"))
            {
                WriteHeadLine($"Test: {test}");
                var result = runtime.ExecuteCommand($"{command.Identifier} {test}");
                Console.WriteLine("");
                if (result.Status != RunResultStatus.Ok && result.Status != RunResultStatus.Quit)
                {
                    WriteError($"Result: {result.Status}");
                }
                else WriteSuccess($"Result: {result.Status}");
            }
            ConsoleService.DisableLog = false;
        }
        catch(Exception e)
        {
            ConsoleService.DisableLog = false;
            throw;
        }
        
    }
}