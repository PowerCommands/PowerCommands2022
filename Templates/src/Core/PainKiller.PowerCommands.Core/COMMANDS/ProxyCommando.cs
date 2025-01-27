﻿namespace $safeprojectname$.Commands
{
    [PowerCommandDesign(description: "Proxy command, this command is executing a command outside this application, the functionality is therefore unknown, you can however use options if you want, to control how the proxy should behave.",
                                  options: "retry-interval-seconds|no-quit",
                                  example: "//Use the retry-interval-seconds option to decide how long pause it should be between retries|//\nUse the --no-quit option to tell the proxy application to not quit after the command is run.",
                       disableProxyOutput: true)]
    public class ProxyCommando(string identifier, CommandsConfiguration configuration, string name, string workingDirectory, string aliasName)
        : CommandBase<CommandsConfiguration>(string.IsNullOrEmpty(aliasName) ? identifier : aliasName, configuration)
    {
        public override RunResult Run()
        {
            WriteProcessLog("Proxy", $"{Input.Raw}");
            var input = (Identifier == identifier) ? Input.Raw.Interpret(Configuration.DefaultCommand) : Input.Raw.Replace(aliasName, identifier).Interpret(Configuration.DefaultCommand);
            var start = DateTime.Now;
            var quitOption = Input.HasOption("no-quit") ? "" : " --justRunOnceThenQuitPowerCommand";
            ShellService.Service.Execute(name, $"{input.Raw}{quitOption}", workingDirectory, WriteLine, useShellExecute: true);

            var retries = 0;
            var maxRetries = 10;
            var foundOutput = false;
            var retryInterval = (int.TryParse(Input.GetOptionValue("retry-interval-seconds"), out var index) ? index * 1000 : 500);
            while (!foundOutput && retries < maxRetries)
            {
                Thread.Sleep(retryInterval);
                var fileName = GetOutputFilename();
                if (File.Exists(fileName))
                {
                    var result = StorageService<ProxyResult>.Service.GetObject(fileName);
                    if (result.Created > start)
                    {
                        WriteLine(result.Output);
                        break;
                    }
                }
                WriteWarning($"Retrying... ({retries + 1} of {maxRetries})");
                retries++;
            }
            return Ok();
        }
        private string GetOutputFilename() => Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"proxy_{identifier}.data");
    }
}