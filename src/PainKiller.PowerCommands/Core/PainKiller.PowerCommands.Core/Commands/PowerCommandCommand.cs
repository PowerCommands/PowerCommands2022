namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandDesign(      description: "Create or update the Visual Studio Solution with all depended projects",
                    arguments:"!<action> (new or update)",
                    flags:"command|solution|output|template|backup",
                    suggestion:"new",
                    quotes: "<path>",
                    example: "//create new VS solution|powercommand new --solution testproject --output \"C:\\Temp\\\"|//Create new PowerCommand named Demo|powercommand new --command Demo|//Update powercommands core, this will first delete current Core projects and than apply the new Core projects|powercommand update|//Only update template(s)|powercommand update --templates|//Update with backup|powercommand update --backup|//Create a new command|powercommand new --command MyNewCommand")]
public class PowerCommandCommand : CommandBase<CommandsConfiguration>
{
    private readonly ArtifactPathsConfiguration _artifact = ConfigurationService.Service.Get<ArtifactPathsConfiguration>().Configuration;
    public PowerCommandCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        var name = Input.HasFlag("solution") ? Input.GetFlagValue("solution") : Input.GetFlagValue("template");
        _artifact.Name = name;

        if (Input.SingleArgument == "new" && Input.HasFlag("solution") && !string.IsNullOrEmpty(Input.GetFlagValue("solution")) && Input.HasFlag("output") && !string.IsNullOrEmpty(Input.GetFlagValue("output")))
        {
            var cmdNew = new CommandNewSolution(Identifier, Configuration, _artifact, Input);
            return cmdNew.Run();
        }
        if (Input.SingleArgument == "update")
        {
            var cmdUpdate = new CommandUpdate(Identifier, Configuration, _artifact, Input);
            return cmdUpdate.Run();
        }
        if (Input.HasFlag("template") && !string.IsNullOrEmpty(Input.GetFlagValue("template")))
        {
            BadParameterError("Not implemented yet...");
        }
        if (Input.SingleArgument == "new" && Input.HasFlag("command")) return CreateCommand(Input.GetFlagValue("command"));
        return BadParameterError("Missing arguments");
    }

    private RunResult CreateCommand(string name)
    {
        var templateManager = new TemplateManager(name, WriteLine);
        templateManager.CreateCommand(templateName: "Default", name);
        return Ok();
    }

    protected void UpdateTemplates(ICliManager cliManager, bool cloneRepo = false, string newProjectName = "")
    {
        if (cloneRepo)
        {
            cliManager.DeleteDownloadsDirectory();
            cliManager.CreateDownloadsDirectory();
            cliManager.CloneRepo(Configuration.Repository);
        }

        var name = string.IsNullOrEmpty(newProjectName) ? CliManager.GetName() : newProjectName;
        var templateManager = new TemplateManager(name, WriteLine);
        templateManager.InitializeTemplatesDirectory();
        templateManager.CopyTemplates();
        
        cliManager.MergeDocsDB();
        cliManager.DeleteFile(_artifact.DocsDbFile, repoFile:true);
    }
}