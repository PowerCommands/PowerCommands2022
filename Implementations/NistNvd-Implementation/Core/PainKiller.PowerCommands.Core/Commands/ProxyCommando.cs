namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandDesign(      description: "Proxy command, this command is executing a command outside this application, the functionality is therefore unknown",
                              options: "retry-interval-seconds",
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
        ShellService.Service.Execute(_name, $"{Input.Raw} --justRunOnceThenQuitPowerCommand", _workingDirctory, WriteLine, useShellExecute: true);
        
        var retries = 0;
        var maxRetries = 9;
        var foundOutput = false;
        var retryInterval = (int.TryParse(Input.GetOptionValue("retry-interval-seconds"), out var index) ? index * 1000 : 500);
        while (!foundOutput || retries > maxRetries)
        {
            Thread.Sleep(retryInterval);
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
            retries++;
        }
        return Ok();
    }
}