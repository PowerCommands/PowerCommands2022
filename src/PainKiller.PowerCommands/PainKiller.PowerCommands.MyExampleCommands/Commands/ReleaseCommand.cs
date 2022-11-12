using PainKiller.PowerCommands.Shared.Utils.DisplayTable;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Command that prepares the implementation VS solutions with the new Core projects and then builds them to verify that the changes did not break anything",
                          flags: "all|no-build|build-only|publish|windows|knowledge|glitch|nist|web",
                        example: "//Release and build all|release --all|//Release and build Glitchfinder and NistNvd|release --glitch --nist|//Just replace Core without build|release --all --no-build|//Just build the implementations|release --all --build-only")]
public class ReleaseCommand : CommandBase<PowerCommandsConfiguration>
{
    private readonly List<BuildSummary> _summary = new();
    public ReleaseCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (!Input.MustOnOfTheseFlagsCheck(Input.Flags)) return BadParameterError("You must provide a least one flag option of these <all|windows|knowledge|glitch|nist>");
        _summary.Clear();

        var all = Input.HasFlag("all");
        var releases = new Release[]
        {
            new(){Flag ="windows",Name = "Windows"},
            new() { Flag = "knowledge", Name = "KnowledgeDB" },
            new() { Flag = "glitch", Name = "GlitchFinder" },
            new() { Flag = "nist", Name = "NistNvd" },
            new() { Flag = "web", Name = "WebClient" }
        };
        foreach (var release in releases)
        {
            if (!Input.HasFlag($"{release.Flag}") && !all) continue;
            var solutionPath = SolutionFileManager.GetLocalSolutionRoot().Replace("src\\PainKiller.PowerCommands\\", $"Implementations\\{release.Name}-implementation");
            WriteHeadLine($"{release.Name}-implementation...");
            if (Input.NoFlag("build-only")) UpdateCoreDirectory(solutionPath);
            if (Input.NoFlag("no-build")) Build(solutionPath);
        }
        var projectPath = SolutionFileManager.GetLocalSolutionRoot().Replace("src\\PainKiller.PowerCommands\\", $"Implementations\\KnowledgeDB-Implementation\\PainKiller.PowerCommands.PowerCommandsConsole");
        if (Input.HasFlag("publish")) Publish(projectPath);

        ConsoleTableService.RenderConsoleCommandTable(_summary, this);
        return Ok();
    }
    private void UpdateCoreDirectory(string solutionPath)
    {
        var coreImplementationDirectory = Path.Combine(solutionPath, "Core");
        WriteSuccessLine($"Delete {coreImplementationDirectory}");
        if (Directory.Exists(coreImplementationDirectory)) Directory.Delete(coreImplementationDirectory, recursive: true);

        var coreDirectory = Path.Combine(SolutionFileManager.GetLocalSolutionRoot(), "Core");
        WriteSuccessLine($"Copy {coreDirectory} to {coreImplementationDirectory}");
        IOService.CopyFolder(coreDirectory, coreImplementationDirectory);
        _summary.Add(new BuildSummary { Path = solutionPath, CoreProjectsUpdated = true });
    }
    private void Publish(string solutionPath)
    {
        ShellService.Service.Execute("dotnet", "publish -p:PublishProfile=FolderProfile", solutionPath, WriteLine, "", waitForExit: true);
        var success = !FindInOutput("Build FAILED");
        AddOrUpdate(success ? new BuildSummary { Path = SolutionFileManager.GetLocalSolutionRoot().Replace("src\\PainKiller.PowerCommands\\", $"Implementations\\KnowledgeDB-implementation"), BuildStatus = "*OK*", Publish = "*OK*"} : new BuildSummary { Path = solutionPath, BuildStatus = "*FAILED*", Publish = "*FAILED*" });
    }
    private void Build(string solutionPath)
    {
        ClearOutput();
        ShellService.Service.Execute("dotnet", "build", solutionPath, WriteLine, "", waitForExit: true);
        var success = !FindInOutput("Build FAILED");
        AddOrUpdate(success ? new BuildSummary { Path = solutionPath, BuildStatus = "*OK*" } : new BuildSummary { Path = solutionPath, BuildStatus = "*FAILED*" });
    }
    private void AddOrUpdate(BuildSummary summary)
    {
        var existing = _summary.FirstOrDefault(s => s.Path == summary.Path);
        if (existing != null) existing.BuildStatus = summary.BuildStatus;
        else _summary.Add(summary);
    }
    class Release { public string? Flag { get; set; } public string? Name { get; set; } }
    class BuildSummary : IConsoleCommandTable
    {
        [ColumnRenderOptions(caption: "Solution path", order: 1)]
        public string Path { get; set; } = "";
        [ColumnRenderOptions(caption: "Core updated", order: 2)]
        public bool CoreProjectsUpdated { get; set; }
        [ColumnRenderOptions(caption: "Status", order: 3, renderFormat: ColumnRenderFormat.SucessOrFailure, trigger1: "*OK*", trigger2: "*FAILED*", mark: "*")]
        public string BuildStatus { get; set; } = "";
        [ColumnRenderOptions(caption: "Published", order: 4, renderFormat: ColumnRenderFormat.SucessOrFailure, trigger1: "*OK*", trigger2: "*FAILED*", mark: "*")]
        public string Publish { get; set; } = "Disabled";
    }
}