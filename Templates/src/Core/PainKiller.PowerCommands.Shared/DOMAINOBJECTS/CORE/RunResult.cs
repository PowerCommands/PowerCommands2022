using $safeprojectname$.Contracts;
using $safeprojectname$.Enums;

namespace $safeprojectname$.DomainObjects.Core
{
    public class RunResult
    {
        public RunResult(IConsoleCommand executingCommand, ICommandLineInput input, string output, RunResultStatus status, string continueWith = "")
        {
            ExecutingCommand = executingCommand;
            Input = input;
            Output = output;
            Status = status;
            ContinueWith = continueWith;
        }
        public IConsoleCommand ExecutingCommand { get; }
        public ICommandLineInput Input { get; }
        public string Output { get; }
        public string ContinueWith { get; set; }
        public RunResultStatus Status { get; }
    }
}