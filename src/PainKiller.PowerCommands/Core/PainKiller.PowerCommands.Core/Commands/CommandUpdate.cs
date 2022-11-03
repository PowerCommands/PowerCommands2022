namespace PainKiller.PowerCommands.Core.Commands;

public class CommandUpdate : PowerCommandCommand
{
    private readonly ArtifactPathsConfiguration _artifact;
    private string _path = "";
    public CommandUpdate(string identifier, CommandsConfiguration configuration, ArtifactPathsConfiguration artifact, ICommandLineInput input) : base(identifier, configuration)
    {
        _artifact = artifact;
        Input = input;
    }
    public override RunResult Run()
    {
        var template = Input.HasFlag("template");
        if (template)
        {
            _path = Path.Combine(AppContext.BaseDirectory, "output");
            UpdateTemplates(new CliManager(CliManager.GetName(), _path, WriteLine), cloneRepo: true);
            return CreateRunResult();
        }
        _path = CliManager.GetLocalSolutionRoot();
        var solutionFile = Directory.GetFileSystemEntries(_path, "*.sln").FirstOrDefault();
        var existingName = CliManager.GetName();
        var backup = Input.HasFlag("backup");

        var cli = new CliManager(existingName, _path, WriteLine);

        if (solutionFile == null)
        {
            WriteLine("When running in application scope (outside VS Env) only the Documentation file will be updated...");
            cli.MergeDocsDB();
            return CreateRunResult();
        }

        Console.WriteLine("Update will delete and replace everything in the [Core] and [Third party components] folder");
        if (backup) Console.WriteLine($"A backup will be saved in folder [{Path.Combine(_path)}]", "Backup. /nEarlier backups needs to be removed");
        Console.WriteLine("");
        Console.WriteLine("Do you want to continue with the update? y/n");
        var response = Console.ReadLine();
        if ($"{response?.Trim()}" != "y") return CreateRunResult();

        cli.DeleteDownloadsDirectory();
        cli.CreateRootDirectory(onlyRepoSrcCodeRootPath: true);

        var backupDirectory = "";
        if (backup) backupDirectory = cli.BackupDirectory(_artifact.Target.Core);

        cli.CloneRepo(Configuration.Repository);
        WriteLine("Fetching repo from Github...");

        UpdateTemplates(cli);

        cli.DeleteDir(_artifact.VsCode);
        cli.DeleteDir(_artifact.CustomComponents);

        cli.DeleteDir(_artifact.Target.Core);
        cli.MoveDirectory(_artifact.Source.Core, _artifact.Target.Core);

        cli.DeleteDownloadsDirectory();

        WriteLine("Your PowerCommands Core component is now up to date with latest code from github!");
        WriteLine("if you started this from Visual Studio you probably need to restart Visual Studio to reload all dependencies");
        if (backup) WriteLine($"A backup of the Core projects has been stored here [{backupDirectory}]");

        ShellService.Service.OpenDirectory(backupDirectory);

        return CreateRunResult();
    }
}