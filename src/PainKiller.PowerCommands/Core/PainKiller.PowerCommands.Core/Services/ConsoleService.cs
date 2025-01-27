using Microsoft.Extensions.Logging;

namespace PainKiller.PowerCommands.Core.Services
{
    public class ConsoleService : IConsoleService
    {
        private bool _disableLog;
        private static readonly Lazy<IConsoleService> Lazy = new(() => new ConsoleService());
        public static IConsoleService Service => Lazy.Value;
        public event OnWrite? WriteToOutput;
        /// <summary>
        /// Disable log of severity levels Trace,Debug and Information.
        /// </summary>
        public void DisableLog()
        {
            WriteToLog(nameof(ConsoleService), "Log from ConsoleService is disabled", LogLevel.Warning);
            _disableLog = true;
        }
        public void EnableLog()
        {
            _disableLog = false;
            WriteToLog(nameof(ConsoleService), "Log from ConsoleService is enabled");
        }
        public void WriteObjectDescription(string scope, string name, string description, bool writeLog = true)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{name}: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{description}");
            Console.ForegroundColor = currentColor;
            if (writeLog) WriteToLog(scope, $"{name} {description}");
            OnWriteToOutput($"{name}: {description}\n");
        }
        public void Write(string scope, string text, ConsoleColor? color = null, bool writeLog = true)
        {
            var currentColor = Console.ForegroundColor;
            if (color != null) Console.ForegroundColor = color.Value;
            Console.Write(text);
            Console.ForegroundColor = currentColor;
            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"{text}");
        }

        public void WriteLine(string scope, string text, ConsoleColor? color = null, bool writeLog = true)
        {
            var currentColor = Console.ForegroundColor;
            if (color != null) Console.ForegroundColor = color.Value;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"{text}\n");
        }

        public void WriteCodeExample(string scope, string commandName, string text, ConsoleColor? color = null, bool writeLog = true)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($" {commandName} ");
            Console.ForegroundColor = currentColor;
            Console.WriteLine(text);

            if (writeLog) WriteToLog(scope, $" {commandName} {text}");
            OnWriteToOutput($" {commandName} {text}\n");
        }
        public void WriteHeaderLine(string scope, string text, ConsoleColor color = ConsoleColor.DarkCyan, bool writeLog = true)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"{text}\n");
        }
        public void WriteWarning(string scope, string text)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
            WriteToLog(scope, $"{text}", LogLevel.Warning);
            OnWriteToOutput($"{text}");
        }
        public void WriteError(string scope, string text)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
            WriteToLog(scope, $"{text}", LogLevel.Error);
            OnWriteToOutput($"{text}");
        }
        public void WriteCritical(string scope, string text)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
            WriteToLog(scope, $"{text}", LogLevel.Critical);
            OnWriteToOutput($"{text}");
        }
        public void WriteSuccessLine(string scope, string text, bool writeLog = true) => WriteLine(scope, text, ConsoleColor.Green, writeLog);
        public void WriteSuccess(string scope, string text, bool writeLog = true) => Write(scope, text, ConsoleColor.Green, writeLog);
        public void WriteUrl(string scope, string text, bool writeLog = true)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;

            if (writeLog) WriteToLog(scope, $"{text}");
            OnWriteToOutput($"{text}\n");
        }
        public void ClearRow(int top)
        {
            var originalLeft = Console.CursorLeft;
            var originalTop = Console.CursorTop;

            Console.SetCursorPosition(0, top);
            var blankRow = new string(' ', Console.WindowWidth);

            Console.Write(blankRow);
            Console.SetCursorPosition(originalLeft, originalTop);
        }
        public void Clear()
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");   //This magic ANSI sequence tells the console to clear the whole buffer and scrollbars, it needs a Console.Clear before and after, just in case, should work on most operating systems.
            Console.Clear();
            Console.CursorTop = IPowerCommandServices.DefaultInstance != null ? IPowerCommandServices.DefaultInstance.Configuration.InfoPanel.Height : 2;
        }
        public void WritePrompt() => Console.Write(IPowerCommandServices.DefaultInstance?.Configuration.Prompt);
        public void WriteRowWithColor(int top, ConsoleColor foregroundColor, ConsoleColor backgroundColor, string rowContent)
        {
            int originalLeft = Console.CursorLeft;
            int originalTop = Console.CursorTop;

            ConsoleColor originalForeColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;

            Console.SetCursorPosition(0, top);

            int consoleWidth = Console.WindowWidth;

            // Clear the row
            Console.SetCursorPosition(0, top);
            Console.Write(new string(' ', consoleWidth));

            // Set the cursor position back to the start of the row
            Console.SetCursorPosition(0, top);

            // Set the new text color for the row
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            // Write the original row content again with the new text color
            Console.Write(rowContent);

            // Restore the original text and background colors
            Console.ForegroundColor = originalForeColor;
            Console.BackgroundColor = originalBackColor;

            // Restore the original cursor position
            Console.SetCursorPosition(originalLeft, originalTop);
        }
        private void WriteToLog(string scope, string message, LogLevel level = LogLevel.Information)
        {
            if (_disableLog && level is LogLevel.Information or LogLevel.Debug or LogLevel.Trace) return;
            var text = $"{scope} {message}";
            switch (level)
            {
                case LogLevel.Trace:
                    IPowerCommandServices.DefaultInstance?.Logger.LogTrace(text);
                    break;
                case LogLevel.Information:
                    IPowerCommandServices.DefaultInstance?.Logger.LogInformation(text);
                    break;
                case LogLevel.Warning:
                    IPowerCommandServices.DefaultInstance?.Logger.LogWarning(text);
                    break;
                case LogLevel.Error:
                    IPowerCommandServices.DefaultInstance?.Logger.LogError(text);
                    break;
                case LogLevel.Critical:
                    IPowerCommandServices.DefaultInstance?.Logger.LogCritical(text);
                    break;
                case LogLevel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
        protected virtual void OnWriteToOutput(string output) => WriteToOutput?.Invoke(output);
    }
}