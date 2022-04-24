using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

public class PowerCommandsConfiguration : CommandsConfiguration
{
    //Here is the placeholder for your custom configuration, you need to add the change to the PowerCommandsConfiguration.yaml file as well
    public string DefaultGitRepositoryPath { get; set; } = "C:\\repos";
    public string ProjectsRelativePath { get; set; } = "Projects";
    public List<ComparisonProject> ComparisonProjects { get; set; } = new();
    public List<RegressionProject> RegressionProjects  { get; set; } = new();
}