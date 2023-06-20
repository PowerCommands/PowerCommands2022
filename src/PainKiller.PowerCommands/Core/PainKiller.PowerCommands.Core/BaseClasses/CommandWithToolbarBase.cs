using System.Timers;
using PainKiller.PowerCommands.ReadLine;
using PainKiller.PowerCommands.ReadLine.Events;

namespace PainKiller.PowerCommands.Core.BaseClasses
{
    public abstract class CommandWithToolbarBase<TConfig> : CommandBase<TConfig> where TConfig : new()
    {
        private static string _latestHighightedCommand = string.Empty;
        private readonly System.Timers.Timer _overflowTimer;
        private readonly ConsoleColor[]? _colors;
        protected List<string> Labels = new();
        protected PowerCommandsToolbarAttribute? ToolbarAttribute;
        protected CommandWithToolbarBase(string identifier, TConfig configuration, bool autoShowToolbar = true, ConsoleColor[]? colors = null) : base(identifier, configuration)
        {
            _colors = colors;
            if (autoShowToolbar) ReadLineService.CommandHighlighted += ReadLineService_CommandHighlighted;
            ToolbarAttribute = this.GetToolbarAttribute();
            var overflowMilliseconds = ToolbarAttribute?.Timer ?? 500;
            _overflowTimer = new System.Timers.Timer(Math.Clamp(overflowMilliseconds, 500, 5000));
            ReadLineService.CmdLineTextChanged += ReadLineService_CmdLineTextChanged;
        }
        protected virtual void ReadLineService_CmdLineTextChanged(object? sender, CmdLineTextChangedArgs e)
        {
            if (!e.CmdText.StartsWith(Identifier) && _latestHighightedCommand == Identifier) ClearToolbar();
        }
        public override RunResult Run()
        {
            ClearToolbar();
            return Ok();
        }
        protected void ReadLineService_CommandHighlighted(object? sender, CommandHighlightedArgs e)
        {
            _latestHighightedCommand = e.CommandName;
            if (e.CommandName != Identifier) return;
            _overflowTimer.Elapsed += InitializeToolbar;
            _overflowTimer.AutoReset = true;
            _overflowTimer.Enabled = true;
        }
        private void InitializeToolbar(object? sender, ElapsedEventArgs e)
        {
            var attribute = this.GetPowerCommandAttribute();
            var suggestions = ToolbarAttribute == null ? Labels.ToList() : ToolbarAttribute.Labels.Split(ConfigurationGlobals.ArraySplitter).ToList();
            if (suggestions.Count == 0) suggestions.AddRange(attribute.Suggestions.Split(ConfigurationGlobals.ArraySplitter));
            if (suggestions.Count == 0) return;
            Labels.Clear();
            Labels.AddRange(suggestions);
            DrawToolbar();
        }
        protected void DrawToolbar()
        {
            DialogService.DrawToolbar(Labels.ToArray(), _colors);

            _overflowTimer.Elapsed -= InitializeToolbar;
            _overflowTimer.Stop();
        }
        protected void ClearToolbar() => DialogService.ClearToolbar(Labels.ToArray());
    }
}