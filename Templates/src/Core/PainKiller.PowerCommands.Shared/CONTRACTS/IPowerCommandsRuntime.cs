using $safeprojectname$.DomainObjects.Core;

namespace $safeprojectname$.Contracts
{
    public interface IPowerCommandsRuntime
    {
        string[] CommandIDs { get; }
        RunResult ExecuteCommand(string rawInput);
        List<IConsoleCommand> Commands { get; }
        public static IPowerCommandsRuntime? DefaultInstance { get; protected set; }
        RunResult? Latest { get; }
    }
}