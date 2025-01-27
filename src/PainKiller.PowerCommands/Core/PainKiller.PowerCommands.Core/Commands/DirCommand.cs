using PainKiller.PowerCommands.Shared.Extensions;

namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandDesign(description: "List the content of the working directory or this applications app directory, with the option to open the directory with the File explorer ",
                 disableProxyOutput: true,
                            options: "!filter|browse|drive-info",
                            example: "//List the content and open the current working directory|dir --open|//Open the AppData roaming directory|dir --app --open")]
    public class DirCommand(string identifier, CommandsConfiguration configuration) : CdCommand(identifier, configuration)
    {
        public override RunResult Run()
        {
            if (HasOption("drive-info")) return ShowDriveInfo();

            var inputs = Input.Raw.Split(' ');
            var path = Environment.CurrentDirectory;
            if (inputs.Length > 1) path = inputs[1].Contains("\"") ? Input.Quotes.First().Replace("\"", "") : inputs[1];
            if (path.StartsWith("--")) path = Environment.CurrentDirectory;


            if (!path.Contains('\\')) path = Path.Combine(Environment.CurrentDirectory, path);

            if (!Directory.Exists(path)) return BadParameterError($"Could not find directory \"{path}\"");
            if (HasOption("browse")) ShellService.Service.OpenDirectory(path);

            Environment.CurrentDirectory = path;
            var filter = GetOptionValue("filter");
            ShowDirectories(filter, output: true);
            return Ok();
        }
        public RunResult ShowDriveInfo()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (!drive.IsReady) continue;
                WriteHeadLine($"Drive: {drive.Name}");
                Console.WriteLine($"  Drive type: {drive.DriveType}");
                Console.WriteLine($"  Volume label: {drive.VolumeLabel}");
                Console.WriteLine($"  File system: {drive.DriveFormat}");
                Console.WriteLine($"  Total size: {drive.TotalSize.GetDisplayFormattedFileSize()}");
                Console.WriteLine($"  Free space: {drive.TotalFreeSpace.GetDisplayFormattedFileSize()}");
            }
            return Ok();
        }
    }
}