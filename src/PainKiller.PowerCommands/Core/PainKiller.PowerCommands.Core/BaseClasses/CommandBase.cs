using System.Text;
using Microsoft.Extensions.Logging;

namespace PainKiller.PowerCommands.Core.BaseClasses
{
    public abstract class CommandBase<TConfig>(string identifier, TConfig configuration, IConsoleService? console = null) : IConsoleCommand, IConsoleWriter
        where TConfig : new()
    {
        private IConsoleService _console = console ?? ConsoleService.Service;
        protected ICommandLineInput Input = new CommandLineInput();
        protected List<PowerOption> Options = new();
        private readonly StringBuilder _output = new();
        protected string LastReadLine = "";

        protected virtual void ConsoleWriteToOutput(string output)
        {
            if (AppendToOutput) _output.Append(output);
        }
        public string Identifier { get; } = identifier;
        protected bool AppendToOutput { get; set; } = true;
        protected PowerCommandDesignAttribute? DesignAttribute { get; private set; }
        public virtual bool InitializeAndValidateInput(ICommandLineInput input, PowerCommandDesignAttribute? designAttribute = null)
        {
            Options.Clear();
            Log(LogLevel.Trace, $"{nameof(InitializeAndValidateInput)} Options.Clear()");
            designAttribute ??= new PowerCommandDesignAttribute("This command has no design attribute");
            Log(LogLevel.Trace, $"{nameof(InitializeAndValidateInput)} designAttribute: {designAttribute.Description}");
            if (IPowerCommandServices.DefaultInstance!.DefaultConsoleService.GetType().Name != _console.GetType().Name) _console = IPowerCommandServices.DefaultInstance.DefaultConsoleService;
            Input = input;
            DesignAttribute = designAttribute;
            IPowerCommandServices.DefaultInstance.Diagnostic.ShowElapsedTime = DesignAttribute.ShowElapsedTime.GetValueOrDefault();
            var validationManager = new InputValidationManager(this, input);
            var result = validationManager.ValidateAndInitialize();
            if (result.Options.Count > 0) Options.AddRange(result.Options);
            Log(LogLevel.Trace, $"{nameof(InitializeAndValidateInput)} validationManager result.Options: {string.Join(',', result.Options)}");
            _console.WriteToOutput += ConsoleWriteToOutput;
            Log(LogLevel.Trace, $"{nameof(InitializeAndValidateInput)} result.HasValidationError: {result.HasValidationError}");
            return result.HasValidationError;
        }
        public virtual void RunCompleted()
        {
            _console.WriteToOutput -= ConsoleWriteToOutput;
            _output.Clear();
            if (IPowerCommandServices.DefaultInstance!.DefaultConsoleService.GetType().Name != ConsoleService.Service.GetType().Name) Console.WriteLine(_output.ToString());
            if (!Input.Options.Contains("--pc_force_quit"))
            {
                if (!Input.Raw.Contains('|')) return;
                var nextCommandInput = Input.Raw.Split('|')[1].Trim().Interpret();
                RunCommandService.Run(nextCommandInput.Identifier, nextCommandInput);
                return;
            }
            WriteLine("--pc_force_quit was provided from command line, program will now quit");
            Environment.Exit(Environment.ExitCode);
        }
        public virtual RunResult Run() => throw new NotImplementedException();
        public virtual async Task<RunResult> RunAsync() => await Task.FromResult(new RunResult(this, Input, "", RunResultStatus.Initializing));
        protected TConfig Configuration { get; set; } = configuration;
        protected void Log(LogLevel level, string message) => IPowerCommandServices.DefaultInstance?.Logger.Log(level, message);

        /// <summary>
        /// Disable log of severity levels Trace,Debug and Information.
        /// </summary>
        protected void DisableLog() => ConsoleService.Service.DisableLog();
        protected void EnableLog() => ConsoleService.Service.EnableLog();

        #region Options
        protected string GetOptionValue(string optionName) => Input.GetOptionValue(optionName);
        protected void DoBadOptionCheck() => Input.DoBadOptionCheck(this);
        protected bool HasOption(string optionName) => Input.HasOption(optionName);
        protected string FirstOptionWithValue() => Input.FirstOptionWithValue();
        protected bool MustHaveOneOfTheseOptionCheck(string[] optionNames) => Input.MustHaveOneOfTheseOptionCheck(optionNames);
        protected bool NoOption(string optionName) => Input.NoOption(optionName);
        #endregion

        #region output
        protected void DisableOutput() => AppendToOutput = false;
        protected void EnableOutput() => AppendToOutput = true;
        protected void DisplayOutput() => Console.WriteLine(_output.ToString());
        protected void ClearOutput() => _output.Clear();
        protected bool FindInOutput(string findPhrase) => _output.ToString().Contains(findPhrase);
        #endregion

        #region RunResult
        protected RunResult Ok(string message) => new(this, Input, $"{message} {_output}", RunResultStatus.Ok);
        protected RunResult Ok() => new(this, Input, $"{_output}", RunResultStatus.Ok);
        protected RunResult Quit() => new(this, Input, $"{_output}", RunResultStatus.Quit);
        protected RunResult Quit(string message) => new(this, Input, $"{message} {_output}", RunResultStatus.Quit);
        protected RunResult BadParameterError(string output) => new(this, Input, output, RunResultStatus.ArgumentError);
        protected RunResult ExceptionError(string output) => new(this, Input, output, RunResultStatus.ExceptionThrown);
        protected RunResult ContinueWith(string rawInput) => new(this, Input, _output.ToString(), RunResultStatus.ExceptionThrown, rawInput);
        #endregion

        #region Write helpers
        public void Write(string output, ConsoleColor? color = null) => _console.Write(GetType().Name, output, color);
        public void WriteLine(string output) => _console.WriteLine(GetType().Name, output);
        /// <summary>
        /// Could be use when passing the method to shell execute and you need to get back what was written and you do not want that in a logfile (a secret for example)
        /// </summary>
        /// <param name="output"></param>
        public void ReadLine(string output) => LastReadLine = output;
        public void WriteCodeExample(string commandName, string text) => _console.WriteCodeExample(GetType().Name, commandName, text);
        public void WriteHeadLine(string output) => _console.WriteHeaderLine(GetType().Name, output);
        public void WriteSuccess(string output) => _console.WriteSuccess(GetType().Name, output);
        public void WriteSuccessLine(string output) => _console.WriteSuccessLine(GetType().Name, output);
        public void WriteUrl(string output) => _console.WriteUrl(GetType().Name, output);
        public void WriteFailure(string output) => _console.Write(GetType().Name, output, ConsoleColor.DarkRed);
        public void WriteFailureLine(string output) => _console.WriteLine(GetType().Name, output, ConsoleColor.DarkRed);
        public void WriteWarning(string output) => _console.WriteWarning(GetType().Name, output);
        public void WriteError(string output) => _console.WriteError(GetType().Name, output);
        public void WriteCritical(string output) => _console.WriteCritical(GetType().Name, output);
        public void WriteSeparatorLine()
        {
            Console.WriteLine("");
            WriteLine("".PadLeft(Console.WindowWidth - 2, '-'));
        }
        protected void OverwritePreviousLine(string output)
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            var padRight = Console.BufferWidth - output.Length;
            AppendToOutput = false;
            WriteLine(output.PadRight(padRight > 0 ? padRight : 0));
            AppendToOutput = true;
        }
        #endregion
        protected void WriteProcessLog(string processTag, string processDescription) => IPowerCommandServices.DefaultInstance?.Logger.LogInformation($"#{processTag}# {processDescription}");
    }
}