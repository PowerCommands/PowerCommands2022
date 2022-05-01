using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("project|help")]
[PowerCommand( description: "Project management, no parameter lists all projects, with log could se the log for a named project",
                 arguments: "log|delete",
                     qutes: "<project name>",
                   example: $"project|project log \"my project\"|project delete \"my project\"")]
public class ProjectCommand : CommandBase<PowerCommandsConfiguration>
{
    public ProjectCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "log" && !string.IsNullOrEmpty(input.SingleQuote))
        {
            ViewLog(input.SingleQuote);
            return CreateRunResult(input);
        }
        if (input.SingleArgument == "delete" && !string.IsNullOrEmpty(input.SingleQuote))
        {
            Delete(input.SingleQuote);
            return CreateRunResult(input);
        }

        WriteHeadLine("Projects");
        foreach (var c in Configuration.ComparisonProjects) this.WriteObjectDescription("     Comparison", $"{c.Name}");
        foreach (var r in Configuration.RegressionProjects) this.WriteObjectDescription("Regression test", $"{r.Name}");
        return CreateRunResult(input);
    }
    private void ViewLog(string projectName)
    {
        foreach (var line in Configuration.Log.GetProcessLog(projectName)) Console.WriteLine(line);
    }

    private void Delete(string projectName)
    {
        var projectC = Configuration.ComparisonProjects.FirstOrDefault(p => p.Name == projectName);
        var projectR = Configuration.RegressionProjects.FirstOrDefault(p => p.Name == projectName);
        if (projectC == null && projectR == null)
        {
            WriteLine($"No project with the name \"{projectName}\" found.");
            return;
        }
        if (projectC != null) Configuration.ComparisonProjects.Remove(projectC);
        if (projectR != null) Configuration.RegressionProjects.Remove(projectR);
        ConfigurationService.Service.SaveChanges(Configuration);
        WriteLine($"Project(s) with name {projectName} deleted.");
        WriteProcessLog(projectName,"Project deleted");

        Console.Write("Do you also want to delete the projet folder with content? y/n?: ");
        var response = Console.ReadLine();
        var dirName = Path.Combine(AppContext.BaseDirectory, "Projects", projectName);
        if (response == "y" && Directory.Exists(dirName))
        {
            Directory.Delete(dirName, recursive: true);
            WriteLine($"Directory {dirName} with all files and subdirectories deleted");
            WriteProcessLog(projectName, "Project directory deleted");
        }
    }
}