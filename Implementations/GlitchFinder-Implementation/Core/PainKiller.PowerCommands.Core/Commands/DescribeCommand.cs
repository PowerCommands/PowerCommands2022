using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Documentation;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(       description: "With help command you will be shown the provided description or online documentation of the command or a PowerCommand feature.",
                       arguments: "<command name or feature you are interested of knowing more>",
                      suggestion: "cls",
                           flags: "docs",
                         example: "describe exit|describe cls|describe log")]
public class DescribeCommand : CommandBase<CommandsConfiguration>
{
    public DescribeCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if(Input.HasFlag("docs")) ShowDoc();
        else ShowCommand();
        return CreateRunResult();
    }

    public void ShowDoc()
    {
        var docSearch = Input.HasFlag("docs") ? Input.GetFlagValue("docs").ToLower() : Input.SingleArgument.ToLower();
        var docs = StorageService<DocsDB>.Service.GetObject().Docs;
        var matchDocs = docs.Where(d => d.DocID.ToString().PadLeft(4, '0') == docSearch || d.Name.ToLower().Contains(docSearch) || d.Tags.ToLower().Contains(docSearch)).ToArray();
        if (matchDocs.Length == 1)
        {
            ShellService.Service.OpenWithDefaultProgram(matchDocs.First().Uri);
            return;
        }
        if (matchDocs.Length > 1)
        {
            WriteHeadLine($"Found {matchDocs.Length} number of documents, you can use the docID the for digit number to the right to choose document to show.");
            foreach (var matchDoc in matchDocs)
            {
                WriteLine($"{matchDoc.DocID.ToString().PadLeft(4, '0')} {matchDoc.Name} {matchDoc.Uri.Split('/').Last()} {matchDoc.Tags}");
            }
            return;
        }
        WriteHeadLine("Could not find any command or documentation to describe");
        WriteHeadLine("Documentation");
        foreach (var doc in docs) WriteLine($"{doc.Name} {doc.Uri.Split('/').Last()} {doc.Tags}");
    }

    private void ShowCommand()
    {
        var identifier = string.IsNullOrEmpty(Input.SingleArgument) ? "describe" : Input.SingleArgument;
        var command = IPowerCommandsRuntime.DefaultInstance?.Commands.FirstOrDefault(c => c.Identifier == identifier);
        if (command == null)
        {
            if (Input.Identifier != nameof(DescribeCommand).ToLower().Replace("command", "")) WriteLine($"Command with identifier:{Input.Identifier} not found");
            ShowDoc();
            return;
        }
        HelpService.Service.ShowHelp(command);
        Console.WriteLine();
    }
}