namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandTest(tests: "list|view|--process git")]
    [PowerCommandDesign(description: "View and manage the log",
                             options: "!process",
                         suggestions: "view|archive",
                   disableProxyOutput: true,
                              example: "//View a list with all the logfiles|log|//Archive the logs into a zip file.|log archive|//View content of the current log|log view|//Filter the log show only posts matching the provided process tag, this requires that you are using process tags when logging in your command(s).|log --process created")]
    public class LogCommand(string identifier, CommandsConfiguration configuration) : CommandBase<CommandsConfiguration>(identifier, configuration)
    {
        public override RunResult Run()
        {
            if (Input.SingleArgument == "archive") return Archive();
            if (Input.SingleArgument == "files") return List();
            if (Input.HasOption("process")) return ProcessLog($"{Input.GetOptionValue("process")}");

            return View();
        }
        private RunResult List()
        {
            DisableLog();
            var dir = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, Configuration.Log.FilePath));
            foreach (var file in dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime)) WriteLine($"{file.Name} {file.LastWriteTime}");
            Console.WriteLine();
            WriteHeadLine("To view current logfile type log view");
            WriteCodeExample("log", "view");

            Console.WriteLine();
            WriteHeadLine("To archive the logs into a zip file type log archive");
            WriteCodeExample("log", "archive");

            Console.WriteLine();
            WriteHeadLine("To view a certain process log use:");
            WriteCodeExample("log", "--process myProcess");
            EnableLog();
            return Ok();
        }
        private RunResult Archive()
        {
            WriteLine(Configuration.Log.ArchiveLogFiles());
            return Ok();
        }

        private RunResult View()
        {
            foreach (var line in Configuration.Log.ToLines()) Console.WriteLine(line);
            return Ok();
        }
        private RunResult ProcessLog(string processTag)
        {
            foreach (var line in Configuration.Log.GetProcessLog(processTag)) Console.WriteLine(line);
            return Ok();
        }
    }
}