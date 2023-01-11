namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandTest(        tests: " ")]
[PowerCommandDesign(description: "Change or view the current working directory",
                        options: "bookmark",
             disableProxyOutput: true,
                        example: "//View current working directory|cd|//Traverse down one directory|//Change working directory|cd ..|cd \"C:\\ProgramData\"|//Set bookmark as the working directory using name|cd --bookmark program|//Set bookmark as the working directory using index|cd --bookmark 0")]
public class CdCommand : CommandBase<CommandsConfiguration>
{
    public static string WorkingDirectory = AppContext.BaseDirectory;
    public static Action<string>? WorkingDirectoryChanged;
    public CdCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        var path = WorkingDirectory;
        if (Input.SingleArgument == "\\")
        {
            var directory = new DirectoryInfo(WorkingDirectory);
            WorkingDirectory = directory.Root.FullName;
            ShowDirectories(appendFiles: false);
            return Ok();
        }

        var inputPath = string.IsNullOrEmpty(Input.Path) ? !string.IsNullOrEmpty(Input.SingleQuote) ? Path.Combine(path, Input.SingleQuote) : Path.Combine(path, string.Join(' ', Input.Arguments)) : Input.Path;

        var bookMarkIndex = Input.OptionToInt("bookmark", -1);
        var bookMark = GetOptionValue("bookmark");
        if (bookMarkIndex >-1)
        {
            if (bookMarkIndex > Configuration.Bookmark.Bookmarks.Count-1)
            {
                WriteError($"\nThere is no bookmark with index {bookMarkIndex} defined in {ConfigurationGlobals.MainConfigurationFile}\n");
                WriteHeadLine("Defined bookmarks");
                foreach(var b in Configuration.Bookmark.Bookmarks) WriteCodeExample($"{b.Index}", b.Name);
                return Ok();
            }
            path = Configuration.Bookmark.Bookmarks[bookMarkIndex].Path;
        }
        else if (!string.IsNullOrEmpty(bookMark) && Configuration.Bookmark.Bookmarks.Any(b => b.Name.ToLower() == bookMark.ToLower()))
        {
            path = Configuration.Bookmark.Bookmarks.First(b => b.Name.ToLower() == bookMark.ToLower()).Path;
        }
        else if (!string.IsNullOrEmpty(inputPath))
        {
            path = inputPath;
        }
        else if (Input.SingleArgument == "..")
        {
            var skipLast = path.EndsWith("\\") ? 2 : 1;
            var paths = WorkingDirectory.Split(Path.DirectorySeparatorChar).SkipLast(skipLast);
            path = string.Join(Path.DirectorySeparatorChar, paths);
        }
        else 
        {
            var dir = string.IsNullOrEmpty(Input.SingleArgument) ? Input.SingleQuote : Input.SingleArgument;
            var paths = WorkingDirectory.Split(Path.DirectorySeparatorChar).ToList();
            if(!string.IsNullOrEmpty(dir)) paths.Add(dir);
            path = string.Join(Path.DirectorySeparatorChar, paths);
        }

        if (Directory.Exists(path)) WorkingDirectory = path;
        else WriteFailureLine($"[{path}] does not exist");
        ShowDirectories(appendFiles: false);
        return Ok();
    }
    public void ShowDirectories(string directory = "", bool appendDirectories = true, bool appendFiles = true, bool appendSuggestions = true, bool showOutput = true)
    {
        var dirInfo = new DirectoryInfo(string.IsNullOrEmpty(directory) ? WorkingDirectory : directory);
        if(showOutput) Console.WriteLine(dirInfo.FullName);
        var suggestions = new List<string>();
        var dirSuggestions = new List<string>();
        foreach (var directoryInfo in dirInfo.GetDirectories())
        {
            if(showOutput) Console.WriteLine($"{directoryInfo.CreationTime}\t<DIR>\t{directoryInfo.Name}");
            if (appendSuggestions && appendDirectories)
            {
                suggestions.Add(directoryInfo.Name);
                dirSuggestions.Add(directoryInfo.Name);
            }
        }
        foreach (var fileInfo in dirInfo.GetFiles())
        {
            if(showOutput) Console.WriteLine($"{fileInfo.CreationTime}\t     \t{fileInfo.Name}");
            if(appendSuggestions && appendFiles) suggestions.Add(fileInfo.Name);
        }
        if (appendSuggestions)
        {
            SuggestionProviderManager.AppendContextBoundSuggestions(Identifier, suggestions.ToArray());
            if(Identifier != "cd") SuggestionProviderManager.AppendContextBoundSuggestions("cd", dirSuggestions.ToArray());
        }
    }
    public virtual void OnWorkingDirectoryChanged(string workingDirectory) => ShowDirectories(showOutput: false);
    public virtual void InitializeWorkingDirectory() => ShowDirectories(showOutput: false);
}