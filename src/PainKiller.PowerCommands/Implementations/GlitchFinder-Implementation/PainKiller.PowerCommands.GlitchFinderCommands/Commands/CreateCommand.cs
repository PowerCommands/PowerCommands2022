using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;
namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("configuration|help")]
[PowerCommand(description: "Creates a new project in config file from a template.\nA folder with the given project name in the projects directory will be created.",
                arguments: "comparsion|regression",
        argumentMandatory: true,
                    qutes: "project name:<name>",
           qutesMandatory: true,
                  example: "create comparsion \"My comparison project\"|create regression \"My regression test project\"")]
public class CreateCommand : CommandBase<PowerCommandsConfiguration>
{
    public CreateCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if(string.IsNullOrEmpty(input.SingleArgument)) return CreateBadParameterRunResult(input, "You must provide witch type of project to create, comparison or regression");
        if (string.IsNullOrEmpty(input.SingleQuote)) return CreateBadParameterRunResult(input, "The project needs a name");

        var projectName = input.SingleQuote;
        CreateProjectDirectory(projectName);

        if(input.SingleArgument == "comparison") NewComparison(projectName);
        if(input.SingleArgument == "regression") NewRegressionTest(projectName);
        
        return CreateRunResult(input);
    }
    public void NewComparison(string projectName)
    {
        Configuration.ComparisonProjects.Add(new(){Name = projectName});
        ConfigurationService.Service.SaveChanges(Configuration);
        WriteLine($"A new comparison project \"{projectName}\" has been created");
    }
    public void NewRegressionTest(string projectName)
    {
        Configuration.RegressionProjects.Add(new(){Name = projectName});
        ConfigurationService.Service.SaveChanges(Configuration);
        WriteLine($"A new configuration template \"{projectName}\" has been created");
    }
    private void CreateProjectDirectory(string projectName)
    {
        var projectsRootPath = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath);
        if (!Directory.Exists(projectsRootPath)) Directory.CreateDirectory(projectsRootPath);

        var projectsPath = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName);
        if (!Directory.Exists(projectsPath)) Directory.CreateDirectory(projectsPath);
    }
}