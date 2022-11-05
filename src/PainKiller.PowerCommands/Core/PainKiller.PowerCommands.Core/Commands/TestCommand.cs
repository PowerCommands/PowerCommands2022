using PainKiller.PowerCommands.Shared.Utils.DisplayTable;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandDesign( description: "Run test on specific command or all commands, the must have a PowerCommandTest attribute declared on class level.",
                           flags: "!command|all|trace",
                         example: "//Test a specific command|test --command commandName|//Test all commands (default) flag could be omitted|test --all")]
public class TestCommand : CommandBase<CommandsConfiguration>
{
    private int _rowCount;
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

                var tableContent = ConsoleTable
                    .From<CommandTestItem>(items)
                    .Configure(o => o.NumberAlignment = Alignment.Right)
                    .Read(WriteFormat.Alternative).Split("\r\n");

                foreach(var row in tableContent) WriteRow(row);

                //var maxLength = items.Max(i => i.Test.Length) + command.Identifier.Length + 1;
                //WriteHeadLine($"\nResults for {command.Identifier}");
                //DisplayHeader(maxLength);
                //foreach (var item in items)
                //{
                //    if (item.Disabled) WriteLine("Disabled".PadLeft(40));
                //    else DisplayItem(item, maxLength);
                //}
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
                var testItem = new CommandTestItem { ExpectedResult = !test.StartsWith("!"), Status = result.Status, Identifier = command.Identifier, Test = test };
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
    private void WriteRow(string row)
    {
        if (_rowCount < 3) WriteHeadLine(row);
        else if (row.Contains("*YES*"))
        {
            var sections = row.Split("*YES*");
            Write(sections[0]);
            WriteSuccess(" YES ");
            WriteLine(sections[1]);
        }
        else if (row.Contains("*NO*"))
        {
            var sections = row.Split("*NO*");
            Write(sections[0]);
            WriteFailure(" NO ");
            WriteLine(sections[1]);
        }
        else WriteLine(row);
        _rowCount++;
    }
}