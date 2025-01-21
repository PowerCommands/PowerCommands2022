namespace PainKiller.PowerCommands.Core.Services
{
    public static class RunCommandService
    {
        public static RunResult Run(string commandName, ICommandLineInput input)
        {
            var command = GetCommand(commandName, input);
            var runResult = command.Run();
            return runResult;
        }
        public static async Task<RunResult> RunAsync(string commandName, ICommandLineInput input)
        {
            var command = GetCommand(commandName, input);
            var runResult = await command.RunAsync();
            return runResult;
        }
        public static IConsoleCommand GetCommand(string commandName)
        {
            var command = IPowerCommandServices.DefaultInstance?.Runtime.Commands.FirstOrDefault(c => c.Identifier == commandName) ?? IPowerCommandServices.DefaultInstance?.Runtime.Commands.FirstOrDefault(c => c.Identifier == IPowerCommandServices.DefaultInstance?.Configuration.DefaultCommand);
            if (command == null) throw new IndexOutOfRangeException($"{commandName} could not be found, please provide a valid Command name, run commands to see all available commands.");
            return command;
        }
        private static IConsoleCommand GetCommand(string commandName, ICommandLineInput input)
        {
            var command = IPowerCommandServices.DefaultInstance?.Runtime.Commands.FirstOrDefault(c => c.Identifier == commandName);
            if (command == null) throw new IndexOutOfRangeException($"{commandName} could not be found, please provide a valid Command name, run commands to see all available commands.");
            var pcAttribute = command.GetPowerCommandAttribute();
            command.InitializeAndValidateInput(input, pcAttribute);
            return command;
        }
    }
}