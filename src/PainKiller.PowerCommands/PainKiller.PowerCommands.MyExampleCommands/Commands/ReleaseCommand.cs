namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Command that prepares the implementation VS solutions with the new Core projects and then builds them to verify that the changes did not break anything",
                          flags: "all|windows|knowledge|glitch|nist",
                        example: "//Release and build all|release --all|//Release and build Glitchfinder and NistNvd|release --glitch --nist")]
public class ReleaseCommand : CommandBase<PowerCommandsConfiguration>
{
    public ReleaseCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (!Input.MustOnOfTheseFlagsCheck(Input.Flags)) return BadParameterError("You must provide a least one flag option of these <all|windows|knowledge|glitch|nist>");

        var all = Input.HasFlag("all");
        string solutionPath;
        
        if (Input.HasFlag("windows") || all)
        {
            solutionPath = SolutionFileManager.GetLocalSolutionRoot().Replace("src\\PainKiller.PowerCommands\\", "Implementations\\Windows-implementation");
            WriteHeadLine("Windows-implementation...");
            UpdateCoreDirectory(solutionPath);
            Build(solutionPath);
        }
        if (Input.HasFlag("knowledge") || all)
        {
            WriteHeadLine("KnowledgeDB-implementation...");
            solutionPath = SolutionFileManager.GetLocalSolutionRoot().Replace("src\\PainKiller.PowerCommands\\", "Implementations\\KnowledgeDB-Implementation");
            UpdateCoreDirectory(solutionPath);
            Build(solutionPath);
        }

        if (Input.HasFlag("glitch") || all)
        {
            WriteHeadLine("GlitchFinder-implementation...");
            solutionPath = SolutionFileManager.GetLocalSolutionRoot().Replace("src\\PainKiller.PowerCommands\\", "Implementations\\GlitchFinder-Implementation");
            UpdateCoreDirectory(solutionPath);
            Build(solutionPath);
        }

        if (Input.HasFlag("nist") || all)
        {
            WriteHeadLine("NistNvd-Implementation...");
            solutionPath = SolutionFileManager.GetLocalSolutionRoot().Replace("src\\PainKiller.PowerCommands\\", "Implementations\\NistNvd-Implementation");
            UpdateCoreDirectory(solutionPath);
            Build(solutionPath);
        }
        return Ok();
    }
    private void UpdateCoreDirectory(string solutionPath)
    {
        var coreImplementationDirectory = Path.Combine(solutionPath, "Core");
        WriteSuccessLine($"Delete {coreImplementationDirectory}");
        if(Directory.Exists(coreImplementationDirectory)) Directory.Delete(coreImplementationDirectory, recursive: true);

        var coreDirectory = Path.Combine(SolutionFileManager.GetLocalSolutionRoot(), "Core");
        WriteSuccessLine($"Copy {coreDirectory} to {coreImplementationDirectory}");
        IOService.CopyFolder(coreDirectory, coreImplementationDirectory);
        
    }
    private void Build(string solutionPath)
    {
        ShellService.Service.Execute("dotnet", "build", solutionPath, WriteLine, "", waitForExit: true);
        WriteSuccessLine($"Build {solutionPath} OK");
    }
}