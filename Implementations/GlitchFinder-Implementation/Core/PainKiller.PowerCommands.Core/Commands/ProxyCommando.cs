namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandDesign(      description: "Proxy command, this command is executing a command outside this application, the functionality is therefore unknown, you can however use options if you want, to control how the proxy should behave.",
                              options: "retry-interval-seconds|no-quit",
                              example: "//Use the retry-interval-seconds option to decide how long pause it should be between retries|//\nUse the --no-quit option to tell the proxy application to not quit after the command is run.",
                   disableProxyOutput: true)]
public class ProxyCommando : CommandBase<CommandsConfiguration>
{
    private readonly string _name;
    private readonly string _workingDirctory;
    public ProxyCommando(string identifier, CommandsConfiguration configuration, string name, string workingDirectory) : base(identifier, configuration)
    {
        _name = name;
        _workingDirctory = workingDirectory;
    }

    public override RunResult Run()
    {
        WriteProcessLog("Proxy", $"{Input.Raw}");
        var input = Input.Raw.Interpret();
        var start = DateTime.Now;
        var quitOption = Input.HasOption("no-quit") ? "" : " --justRunOnceThenQuitPowerCommand";
        ShellService.Service.Execute(_name, $"{Input.Raw}{quitOption}", _workingDirctory, WriteLine, useShellExecute: true);
        
        var retries = 0;
        var maxRetries = 9;
        var foundOutput = false;
        var retryInterval = (int.TryParse(Input.GetOptionValue("retry-interval-seconds"), out var index) ? index * 1000 : 500);
        while (!foundOutput || retries > maxRetries)
        {
            Thread.Sleep(500);
            var fileName = input.GetOutputFilename();
            if (File.Exists(fileName))
            {
                var result = StorageService<ProxyResult>.Service.GetObject(fileName);
                if (result.Created > start)
                {
                    WriteLine(result.Output);
                    break;
                }
            }
            WriteWarning($"Retrying... ({retries} of {maxRetries})");
            Thread.Sleep(retryInterval);
            retries++;
        }
        return Ok();
    }
}