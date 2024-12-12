using PainKiller.PowerCommands.Core.Commands;
using PainKiller.PowerCommands.ReadLine.Events;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(tests: " ")]
[PowerCommandDesign(description: "Generate API with OpenApi Code generator using a docker image, you create two files.\n First one with the API specification, the second one for the config, the filename should contain config so the command knows what is what...\n Navigate to the directory with the files using the cd command.",
                         example: "openapi --generate")]
public class OpenApiCommand : CommandWithToolbarBase<PowerCommandsConfiguration>
{
    public OpenApiCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration, autoShowToolbar: false) { }
    protected override void ReadLineService_CommandHighlighted(object? sender, CommandHighlightedArgs e)
    {
        if (e.CommandName == Identifier)
        {
            Labels.Clear();
            if (ValidFilesExists())
            {
                Labels.Add("Valid files found! ->");
                Labels.Add("--generate");
                DrawToolbar();
                return;
            }
            Labels.Add("Navigate to a directory with the yaml files, using cd command");
            DrawToolbar();
        }
    }
    public override RunResult Run()
    {
        if (HasOption("generate")) GenerateCode();
        return Ok();
    }
    public void GenerateCode()
    {
        var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
        var files = directoryInfo.GetFiles("*.yaml");
        if (files.Length != 2) throw new IndexOutOfRangeException("The directory must contain 2 and only 2 yaml files, one of them should contain \"config\" so that the generation could perform properly.");
        var config = files.FirstOrDefault(f => f.Name.ToLower().Contains("config"));
        if (config == null) throw new IndexOutOfRangeException("No yaml file found that contains \"config\" in the filename");
        var specification = files.First(f => f.Name != config.Name);
        var outputDir = Path.Combine(string.Join(Path.DirectorySeparatorChar, Environment.CurrentDirectory.Split(Path.DirectorySeparatorChar).SkipLast(1)), "generated-code");
        if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
        var dockerArgument = $"run --rm -v {Environment.CurrentDirectory}:/localin -v {outputDir}:/localout openapitools/openapi-generator-cli generate -i /localin/{specification.Name} -g aspnetcore -o /localout/ -c /localin/{config.Name}";
        WriteCodeExample("docker", dockerArgument);
        ShellService.Service.Execute("docker", $"run --rm -v {Environment.CurrentDirectory}:/localin -v {outputDir}:/localout openapitools/openapi-generator-cli generate -i /localin/{specification.Name} -g aspnetcore -o /localout/ -c /localin/{config.Name}", Environment.CurrentDirectory, ReadLine, "", waitForExit: true);

        WriteLine($"{LastReadLine}");
    }
    public bool ValidFilesExists()
    {
        var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
        var files = directoryInfo.GetFiles("*.yaml");
        if (files.Length != 2) return false;
        var config = files.FirstOrDefault(f => f.Name.ToLower().Contains("config"));
        if (config == null) return false;
        return true;
    }
}