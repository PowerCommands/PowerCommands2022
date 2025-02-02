namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandTest(tests: " ")]
    [PowerCommandDesign(description: "Change or view the current working directory",
                            options: "bookmark|roaming|startup|recent|documents|programs|windows|profile|templates|videos|pictures|music",
                 disableProxyOutput: true,
                            example: "//View current working directory|cd|//Traverse down one directory|//Change working directory|cd ..|cd \"C:\\ProgramData\"|//Set bookmark as the working directory using name|cd --bookmark program|//Set bookmark as the working directory using index|cd --bookmark 0|//Set first existing bookmark (if any) as working directory|cd --bookmark|//Set Windows directory as working directory|cd --windows")]
    public class CdCommand(string identifier, CommandsConfiguration configuration) : CommandWithToolbarBase<CommandsConfiguration>(identifier, configuration), IWorkingDirectoryChangesListener
    {
        public static Action<string[], string[]>? WorkingDirectoryChanged;

        public override RunResult Run()
        {
            var path = Environment.CurrentDirectory;
            if (Input.SingleArgument == "\\")
            {
                var directory = new DirectoryInfo(Environment.CurrentDirectory);
                Environment.CurrentDirectory = directory.Root.FullName;
                ShowDirectories();
                return Ok();
            }

            var inputPath = string.IsNullOrEmpty(Input.Path) ? !string.IsNullOrEmpty(Input.SingleQuote) ? Path.Combine(path, Input.SingleQuote) : Path.Combine(path, string.Join(' ', Input.Arguments)) : Input.Path;

            var bookMarkIndex = Input.OptionToInt("bookmark", -1);
            var bookMark = GetOptionValue("bookmark");
            if (bookMarkIndex > -1)
            {
                if (bookMarkIndex > Configuration.Bookmark.Bookmarks.Count - 1)
                {
                    WriteError($"\nThere is no bookmark with index {bookMarkIndex} defined in {ConfigurationGlobals.MainConfigurationFile}\n");
                    WriteHeadLine("Defined bookmarks");
                    foreach (var b in Configuration.Bookmark.Bookmarks) WriteCodeExample($"{b.Index}", b.Name);
                    return Ok();
                }
                path = Configuration.Bookmark.Bookmarks[bookMarkIndex].Path;
            }
            else if (!string.IsNullOrEmpty(bookMark) && Configuration.Bookmark.Bookmarks.Any(b => b.Name.ToLower() == bookMark.ToLower()))
            {
                path = Configuration.Bookmark.Bookmarks.First(b => b.Name.ToLower() == bookMark.ToLower()).Path;
            }
            else if (HasOption("bookmark") && Configuration.Bookmark.Bookmarks.Count > 0)
            {
                path = Configuration.Bookmark.Bookmarks.First().Path;
            }
            else if (HasOption("roaming"))
            {
                path = ConfigurationGlobals.ApplicationDataFolder;
            }
            else if (HasOption("startup"))
            {
                var exutableFileInfo = new FileInfo($"{Environment.ProcessPath}");
                path = exutableFileInfo.DirectoryName;
            }
            else if (HasOption("programs"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            }
            else if (HasOption("documents"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else if (HasOption("recent"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            }
            else if (HasOption("windows"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            }
            else if (HasOption("music"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }
            else if (HasOption("pictures"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            else if (HasOption("videos"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            }
            else if (HasOption("templates"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
            }
            else if (HasOption("profile"))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else if (!string.IsNullOrEmpty(inputPath))
            {
                path = inputPath;
            }
            else if (Input.SingleArgument == "..")
            {
                var skipLast = path.EndsWith("\\") ? 2 : 1;
                var paths = Environment.CurrentDirectory.Split(Path.DirectorySeparatorChar).SkipLast(skipLast);
                path = string.Join(Path.DirectorySeparatorChar, paths);
            }
            else
            {
                var dir = string.IsNullOrEmpty(Input.SingleArgument) ? Input.SingleQuote : Input.SingleArgument;
                var paths = Environment.CurrentDirectory.Split(Path.DirectorySeparatorChar).ToList();
                if (!string.IsNullOrEmpty(dir)) paths.Add(dir);
                path = string.Join(Path.DirectorySeparatorChar, paths);
            }
            path = $"{path}".Replace(ConfigurationGlobals.UserNamePlaceholder, Environment.UserName, StringComparison.CurrentCultureIgnoreCase);
            if (path.Contains(ConfigurationGlobals.RoamingDirectoryPlaceholder)) path = path.Replace(ConfigurationGlobals.RoamingDirectoryPlaceholder, ConfigurationGlobals.ApplicationDataFolder);
            if (Directory.Exists(path)) Environment.CurrentDirectory = $"{Path.GetFullPath(path)}";
            else WriteFailureLine($"[{path}] does not exist");
            ShowDirectories();
            return Ok();
        }
        protected void ShowDirectories(string filter = "", bool output = false)
        {
            var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
            WriteHeadLine(dirInfo.FullName);
            var fileSuggestions = new List<string>();
            var dirSuggestions = new List<string>();
            var fileCounter = dirInfo.GetFiles().Length;
            var dirCounter = dirInfo.GetDirectories().Length;
            var userFilter = string.IsNullOrEmpty(filter) ? $"{GetOptionValue("filter")}".ToLower() : filter.ToLower();
            WriteCodeExample("Files", $"{fileCounter}");
            WriteCodeExample("Directories", $"{dirCounter}");
            if (!string.IsNullOrEmpty(userFilter)) WriteCodeExample("Filter", userFilter);
            foreach (var directoryInfo in dirInfo.GetDirectories().Where(d => d.Name.ToLower().Contains(userFilter) || string.IsNullOrEmpty(userFilter)))
            {
                if (output) Console.WriteLine($"{directoryInfo.CreationTime}\t<DIR>\t{directoryInfo.Name}");
                dirSuggestions.Add(directoryInfo.Name);
            }
            foreach (var fileInfo in dirInfo.GetFiles().Where(f => f.Name.ToLower().Contains(userFilter) || string.IsNullOrEmpty(userFilter)))
            {
                if (output) Console.WriteLine($"{fileInfo.CreationTime}\t     \t{fileInfo.Name}");
                fileSuggestions.Add(fileInfo.Name);
            }
            WorkingDirectoryChanged?.Invoke(fileSuggestions.ToArray(), dirSuggestions.ToArray());
        }
        public virtual void OnWorkingDirectoryChanged(string[] files, string[] directories)
        {
            var suggestions = new List<string>();
            suggestions.AddRange(directories);
            if (Identifier != "cd") suggestions.AddRange(files);
            SuggestionProviderManager.AppendContextBoundSuggestions(Identifier, suggestions.ToArray());
        }
        public virtual void InitializeWorkingDirectory()
        {
            var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
            var fileSuggestions = dirInfo.GetFiles().Select(d => d.Name).ToArray();
            var dirSuggestions = dirInfo.GetDirectories().Select(d => d.Name).ToArray();
            WorkingDirectoryChanged?.Invoke(fileSuggestions, dirSuggestions);
        }
    }
}