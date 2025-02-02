namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandTest(tests: "exit|start --docs")]
    [PowerCommandDesign(description: "With help command you will be shown the provided description or online documentation of the command or a PowerCommand feature.",
                           arguments: "<command name or feature you are interested of knowing more>",
                             options: "new|all",
                  disableProxyOutput: true,
                             example: "describe exit|describe cls|describe log|//Open documentation about options (if any)|describe options --doc")]
    public class DescribeCommand(string identifier, CommandsConfiguration configuration) : CommandBase<CommandsConfiguration>(identifier, configuration)
    {
        protected static List<Doc> Items = new();
        protected static Doc SelectedItem = new();
        public override RunResult Run()
        {
            if (HasOption("new"))
            {
                SelectedItem = new Doc();
                AddOrEdit();
                return Ok();
            }
            if (HasOption("all"))
            {
                Items = StorageService<DocsDB>.Service.GetObject().Docs;
                ShowResult("All created descriptions...");
                if(Items.Count == 0) WriteLine("No description documents added...");
                return Ok();
            }
            ShowDoc();
            return Ok();
        }
        public void ShowDoc()
        {
            var docSearch = Input.SingleArgument.ToLower();
            var docs = StorageService<DocsDB>.Service.GetObject().Docs;
            var matchDocs = docs.Where(d => d.DocID.ToString().PadLeft(4, '0') == docSearch || d.Name.ToLower().Contains(docSearch) || d.Tags.ToLower().Contains(docSearch)).ToArray();
            if (Input.Arguments.Length > 1)
            {
                var arguments = Input.Arguments.ToList();
                //Add filters as many as the user have given, but limit the filter count to 10, higher value must some kind of abuse.
                var iterations = arguments.Count < 100 ? arguments.Count : 10;
                for (var i = 1; i < iterations; i++) matchDocs = matchDocs.Where(d => d.Name.ToLower().Contains(arguments[i].ToLower()) || d.Tags.ToLower().Contains(arguments[i].ToLower())).ToArray();;
            }
            if (matchDocs.Length > 0)
            {
                Items = matchDocs.ToList();
                if (ShowResult(docSearch)) ShellService.Service.OpenWithDefaultProgram(matchDocs.First().Uri);
                return;
            }
            if(ShowCommand()) return;
            WriteHeadLine("Could not find any command or documentation to describe, configured AI services will try to get you an answer.");
            ShellService.Service.OpenWithDefaultProgram(Configuration.DefaultAIBotUri.Replace(ConfigurationGlobals.QueryPlaceholder, string.Join(" ", Input.Arguments)));
        }
        private bool ShowCommand()
        {
            var commandIdentifier = string.IsNullOrEmpty(Input.SingleArgument) ? "describe" : Input.SingleArgument;
            var command = IPowerCommandsRuntime.DefaultInstance?.Commands.FirstOrDefault(c => c.Identifier == commandIdentifier);
            if (command == null)
            {
                if (Input.Identifier != nameof(DescribeCommand).ToLower().Replace("command", "")) WriteLine($"Command with identifier:{Input.Identifier} not found");
                return false;
            }
            HelpService.Service.ShowHelp(command, clearConsole: true);
            Console.WriteLine();
            return true;
        }
        protected bool ShowResult(string headLine)
        {
            var selected = ListService.ListDialog($"{headLine}\nSearch phrase(s): {Input.Raw.Replace("find ","")}", Items.Select(i => $"{i.Name} {i.Uri} {i.Tags}").ToList());
            if (selected.Count == 0) return false;
        
            SelectedItem = Items[selected.Keys.First()];
            var navigateOption = ToolbarService.NavigateToolbar<DescribeDialogAlternatives>();
            if(navigateOption == DescribeDialogAlternatives.Continue) return false;
            if(navigateOption == DescribeDialogAlternatives.Open) return OpenDocument();
            if (navigateOption == DescribeDialogAlternatives.Edit) return AddOrEdit();
            if (navigateOption == DescribeDialogAlternatives.Delete) return Delete();
            if (navigateOption == DescribeDialogAlternatives.CreateMarkdownFile) return CreateMarkdownFile(SelectedItem.Name);
            if (navigateOption == DescribeDialogAlternatives.UseYourAIService) return OpenAIService();
            return false;
        }
        public bool OpenAIService()
        {
            var arguments = new List<string> { $"{SelectedItem.Name}" };
            arguments.AddRange(SelectedItem.Tags.Split(","));
            ShellService.Service.OpenWithDefaultProgram(Configuration.DefaultAIBotUri.Replace(ConfigurationGlobals.QueryPlaceholder, string.Join(" ", arguments)));
            return true;
        }
        public bool OpenDocument()
        {
            ShellService.Service.OpenWithDefaultProgram(SelectedItem.Uri);
            WriteHeadLine(SelectedItem.Name);
            WriteCodeExample("Tags:", SelectedItem.Tags);
            WriteCodeExample("Uri:", SelectedItem.Uri);
            WriteCodeExample("Last updated:", SelectedItem.Updated.ToShortDateString());
            return true;
        }
        public bool AddOrEdit()
        {
            if(string.IsNullOrEmpty(SelectedItem.Name)) SelectedItem.Name = string.IsNullOrEmpty(Input.SingleArgument) ? DialogService.QuestionAnswerDialog("Name your decription:") : Input.SingleArgument;
            SelectedItem.Tags = DialogService.QuestionAnswerDialog("Add tags separated with ,");
            var attachFile = DialogService.YesNoDialog("Attach a file? (or just add a uri in next step)");
            if (attachFile)
            {
                var selectedFile = DialogService.SelectFileDialog(Environment.CurrentDirectory, onlyTextFiles: true);
                if (!string.IsNullOrEmpty(selectedFile))
                {
                    var targetDirPath = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, ConfigurationGlobals.DocsDirectoryName);
                    var selectedFileInfo = new FileInfo(selectedFile);
                    if(!Directory.Exists(targetDirPath))  Directory.CreateDirectory(targetDirPath);
                    if (File.Exists(Path.Combine(Environment.CurrentDirectory, selectedFile)) && !File.Exists(Path.Combine(targetDirPath, selectedFileInfo.Name)))
                    {
                        var sourceFile = Path.Combine(Environment.CurrentDirectory, selectedFile);
                        var destinationFile = Path.Combine(targetDirPath, selectedFileInfo.Name);
                        File.Copy(sourceFile, destinationFile);
                        WriteSuccessLine($"File [{sourceFile}] copied to [{destinationFile}].");
                    }
                    SelectedItem.Uri = $"{ConfigurationGlobals.RoamingDirectoryPlaceholder}\\{ConfigurationGlobals.DocsDirectoryName}\\{selectedFileInfo.Name}";
                }
            }
            else SelectedItem.Uri = DialogService.QuestionAnswerDialog("Add uri (could be left blank)");
            var db = StorageService<DocsDB>.Service.GetObject();
            if (SelectedItem.DocID > -1)
            {
                var existingDoc = db.Docs.FirstOrDefault(d => d.DocID == SelectedItem.DocID);
                if (existingDoc != null) db.Docs.Remove(existingDoc);
            }
            else
            {
                if (db.Docs.Count == 0) SelectedItem.DocID = 1;
                else SelectedItem.DocID = db.Docs.Max(d => d.DocID) + 1;
                if (string.IsNullOrEmpty(SelectedItem.Uri)) if ( DialogService.YesNoDialog("Do you want to add a markdown file to this description?")) CreateMarkdownFile(SelectedItem.Name);
            }
            db.Docs.Add(SelectedItem);
            StorageService<DocsDB>.Service.StoreObject(db);
            WriteSuccessLine($"\nDocument [{SelectedItem.Name}] saved.");
            return true;
        }
        public bool Delete()
        {
            var db = StorageService<DocsDB>.Service.GetObject();
            var existingDoc = db.Docs.FirstOrDefault(d => d.DocID == SelectedItem.DocID);
            if (existingDoc == null) return false;
            var confirmDeletion = DialogService.YesNoDialog($"Delete [{SelectedItem.Name}] are you sure?");
            if (!confirmDeletion) return false;
            db.Docs.Remove(existingDoc);
            StorageService<DocsDB>.Service.StoreObject(db);
            WriteSuccessLine($"\nDocument [{SelectedItem.Name}] deleted.");
            if (SelectedItem.Uri.StartsWith(ConfigurationGlobals.RoamingDirectoryPlaceholder))
            {
                var localFileExist = db.Docs.Any(d => d.Uri == SelectedItem.Uri);
                if (!localFileExist)
                {
                    var localFilePath = SelectedItem.Uri.Replace(ConfigurationGlobals.RoamingDirectoryPlaceholder, ConfigurationGlobals.ApplicationDataFolder);
                    if(File.Exists(localFilePath)) File.Delete(localFilePath);
                    WriteSuccessLine($"\nFile [{localFilePath}] deleted.");
                }
            } 
            return true;
        }
        public bool CreateMarkdownFile(string name)
        {
            var targetDirPath = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, ConfigurationGlobals.DocsDirectoryName);
            var fileName = Path.Combine(targetDirPath, $"{name}.md");
            var markdownEditor = new MarkdownEditorManager(fileName, ConsoleService.Service, Configuration.Prompt);
            markdownEditor.Run();
            SelectedItem.Uri = $"{ConfigurationGlobals.RoamingDirectoryPlaceholder}\\{ConfigurationGlobals.DocsDirectoryName}\\{name}.md";
            
            var db = StorageService<DocsDB>.Service.GetObject();
            var existingDoc = db.Docs.FirstOrDefault(d => d.DocID == SelectedItem.DocID);
            ShellService.Service.OpenWithDefaultProgram(fileName);
            if (existingDoc == null) return false;
            db.Docs.Remove(existingDoc);
            db.Docs.Add(SelectedItem);
            StorageService<DocsDB>.Service.StoreObject(db);
            WriteSuccessLine($"\nDocument [{fileName}] created.");
            return true;
        }
    }
}