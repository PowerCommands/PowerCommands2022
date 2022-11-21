using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.WindowsCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Run powershell commands, with possibility to filter the output. You don´t have to use the options, you could just write your powershell commands as is.",
                         options: "Get-Help|Get-Command|Get-Process|Get-Service|filter",
                         example: "//Get all available powershell commands on your machine.|ps --Get-Command")]
public class PsCommand : CommandBase<CommandsConfiguration>
{
    private readonly List<OutputItem> _output = new();
    public PsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        _output.Clear();
        ShellService.Service.Execute("powershell", Input.NoOption("filter") ? $"{Input.FirstOptionWithValue()} {string.Join(' ', Input.Arguments)}" : $"{Input.FirstOptionWithValue()}", workingDirectory: AppContext.BaseDirectory, waitForExit: true, writeFunction: CollectOutput);
        foreach (var outputItem in _output.Where(o => o.Result.ToLower().Contains(Input.GetOptionValue("filter").ToLower()) && o.Result.ToLower().StartsWith(Input.GetOptionValue("begins"))   )) Console.WriteLine(outputItem.Result);
        return Ok();
    }
    private void CollectOutput(string output)
    {
        var rows = output.Split("\r\n");
        foreach (var row in rows) _output.Add(new OutputItem { Result = row });
    }
    public class OutputItem{ public string Result { get; set; } = ""; } 
}