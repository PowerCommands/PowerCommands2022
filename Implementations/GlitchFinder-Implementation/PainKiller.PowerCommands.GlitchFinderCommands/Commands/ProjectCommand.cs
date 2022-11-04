using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[PowerCommand( description: "Project management, no parameter lists all projects, with log could se the log for a named project",
                 arguments: "log|delete",
                     qutes: "<project name>",
                   example: $"project|project log \"my project\"|project delete \"my project\"")]
public class ProjectCommand : CommandBase<PowerCommandsConfiguration>
{
    public ProjectCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (Input.SingleArgument == "log" && !string.IsNullOrEmpty(Input.SingleQuote))
        {
            ViewLog(Input.SingleQuote);
            return Ok();
        }
        if (Input.SingleArgument == "delete" && !string.IsNullOrEmpty(Input.SingleQuote))
        {
            Delete(Input.SingleQuote);
            return Ok();
        }

        WriteHeadLine("Projects");
        foreach (var c in Configuration.ComparisonProjects) ConsoleService.WriteObjectDescription(GetType().Name, "     Comparison", $"{c.Name}");
        foreach (var r in Configuration.RegressionProjects) ConsoleService.WriteObjectDescription(GetType().Name, "Regression test", $"{r.Name}");
        return Ok();
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