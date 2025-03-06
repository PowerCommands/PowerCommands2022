using PainKiller.PowerCommands.Shared.Extensions;

namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandTest(tests: "whats_new.md --read")]
    [PowerCommandDesign(description: "Handle basic file actions such as read, write, delete, copy, move\n You can use tab to traverse through current directory, use cd and dir command to change current directory or see it´s content.\nRemember that filename containing white spaces must be surrounded with quotation marks.",
                            options: "read|write|delete|open|properties|!copy|move|!command|!text|confirm|overwrite",
                          arguments: "fileName",
                 disableProxyOutput: true,
                            example: "//Read a file in current working directory (use dir command to see current directory, cd command to change directory)|file filename.txt --read|//Delete a file (without the --confirm flag, no questions will be asked.|file filename.txt --delete --confirm|//Write a file with output from an existing command, example using the VersionCommand|file \"output.txt\" --write --command version|//Copy a existing file, use --overwrite flag to overwrite existing file if target file exists.|file filename.txt --copy --overwrite \"copy file.txt\"|//Move an existing file, use --overwrite flag to overwrite existing file if target file exists.|file filename.txt --move \"moved file.txt\" --overwrite")]
    public class FileCommand(string identifier, CommandsConfiguration configuration) : CdCommand(identifier, configuration)
    {
        public override RunResult Run()
        {
            var inputs = Input.Raw.Split(' ');

            if (inputs.Length == 1 || HasOption("target")) return WritePreviousCommandResult();   //Name of file must be provided by previous executed command (using |)
            if (inputs.Length < 3) return BadParameterError("You must provide a file name and at least one option flag.");
            var path = inputs[1].Contains("\"") ? Input.Quotes.First().Replace("\"", "") : inputs[1];
            if (!path.Contains('\\')) path = Path.Combine(Environment.CurrentDirectory, path);

            if (string.IsNullOrEmpty(path)) return BadParameterError("A valid path to a file must be provided!");
            if (HasOption("write")) return WriteFile(path);
            if (HasOption("open")) return OpenFile(path);
            if (HasOption("properties")) return ShowFileInfo(path);
            if (HasOption("delete")) return DeleteFile(path);
            if (HasOption("copy")) return CopyFile(path);
            if (HasOption("move")) return MoveFile(path);
            return ReadFile(path);
        }

        private RunResult ShowFileInfo(string path)
        {
            if (!File.Exists(path)) return BadParameterError($"File [{path}] does not exist!");
            var fileInfo = new FileInfo(path);
            WriteHeadLine(path);
            WriteCodeExample("Name", fileInfo.FullName);
            WriteCodeExample("Description", fileInfo.GetFileTypeDescription());
            WriteCodeExample("Size", fileInfo.Length.GetDisplayFormattedFileSize());
            WriteCodeExample("Created", fileInfo.CreationTime.GetDisplayTimeSinceLastUpdate());
            WriteCodeExample("Modified", fileInfo.LastWriteTime.GetDisplayTimeSinceLastUpdate());
            WriteCodeExample("Accessed", fileInfo.LastAccessTime.GetDisplayTimeSinceLastUpdate());
            WriteCodeExample("Extension", fileInfo.Extension);
            WriteCodeExample("Is read only", $"{fileInfo.IsReadOnly}");
            WriteCodeExample("Attributes", $"{fileInfo.Attributes}");
            return Ok();
        }

        private RunResult ReadFile(string path)
        {
            WriteProcessLog(nameof(FileCommand), $"Read file [{path}]");
            var content = File.ReadAllText(path);
            Console.WriteLine(content);
            return Ok();
        }

        private RunResult WritePreviousCommandResult()
        {
            var latestCommandResult = IPowerCommandsRuntime.DefaultInstance?.Latest;
            if (latestCommandResult == null) return BadParameterError("Could not fetch the latest command.");
            var inputs = latestCommandResult.Input.Raw.Split(' ');
            var path = inputs[1].Contains("\"") ? Input.Quotes.First().Replace("\"", "") : inputs[1];
            if (!string.IsNullOrEmpty(GetOptionValue("target"))) path = GetOptionValue("target");   //If a path is provided with the option flag target, this will be used instead.
            if (!path.IsValidFileName()) return BadParameterError("You must provide a valid file path as the first parameter (must be surrounded with quotation marks if filename contains whitespaces.");
            if (!path.Contains('\\')) path = Path.Combine(Environment.CurrentDirectory, path);

            DisableLog();
            var content = latestCommandResult.Output;
            File.WriteAllText(path, content);
            InitializeWorkingDirectory();
            EnableLog();
            WriteSuccessLine($"File [{path}] successfully written.");
            return Ok();
        }
        private RunResult WriteFile(string path)
        {
            var commandName = $"{GetOptionValue("command")}";
            var content = $"{GetOptionValue("text")}"; ;

            if (!string.IsNullOrEmpty(commandName))
            {
                var result = RunCommandService.Run(commandName, Input);
                content = result.Output;
            }
            File.WriteAllText(path, content);
            InitializeWorkingDirectory();
            WriteSuccessLine($"File [{path}] successfully written.");
            return Ok();
        }
        private RunResult DeleteFile(string path)
        {
            if (!File.Exists(path)) return BadParameterError($"{path} does not exist!");
            var confirm = HasOption("confirm");
            if (confirm)
            {
                var dialogResponse = DialogService.YesNoDialog($"Do you want to delete the file {path}?");
                if (!dialogResponse) return Ok();
            }
            File.Delete(path);
            InitializeWorkingDirectory();
            WriteSuccessLine($"The file [{path}] has successfully been deleted.");
            return Ok();
        }
        private RunResult CopyFile(string path)
        {
            var copyPath = GetOptionValue("copy");
            var overwrite = HasOption("overwrite");
            if (!File.Exists(path)) return BadParameterError($"{path} does not exist!");
            File.Copy(path, copyPath, overwrite);
            WriteSuccessLine($"The file [{path}] has successfully been copied to file [{copyPath}].");
            InitializeWorkingDirectory();
            return Ok();
        }
        private RunResult MoveFile(string path)
        {
            var movePath = GetOptionValue("move");
            var overwrite = HasOption("overwrite");
            if (!File.Exists(path)) return BadParameterError($"{path} does not exist!");
            File.Move(path, movePath, overwrite);
            WriteSuccessLine($"The file [{path}] has successfully been moved to file [{movePath}].");
            InitializeWorkingDirectory();
            return Ok();
        }
        private RunResult OpenFile(string path)
        {
            if (!File.Exists(path)) return BadParameterError($"{path} does not exist!");
            ShellService.Service.OpenWithDefaultProgram(path);
            return Ok();
        }
    }
}