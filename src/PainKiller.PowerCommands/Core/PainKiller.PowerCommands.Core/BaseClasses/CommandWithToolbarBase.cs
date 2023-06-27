using PainKiller.PowerCommands.ReadLine;
using PainKiller.PowerCommands.ReadLine.Events;
using System.Timers;

namespace PainKiller.PowerCommands.Core.BaseClasses
{
    public abstract class CommandWithToolbarBase<TConfig> : CommandBase<TConfig> where TConfig : new()
    {
        protected string LatestHighlightedCommand = string.Empty;
        private readonly System.Timers.Timer _overflowTimer;
        private readonly bool _autoShowToolbar;
        private readonly ConsoleColor[]? _colors;
        protected List<string> Labels = new();
        protected PowerCommandsToolbarAttribute? ToolbarAttribute;
        protected CommandWithToolbarBase(string identifier, TConfig configuration, bool autoShowToolbar = true, ConsoleColor[]? colors = null) : base(identifier, configuration)
        {
            _autoShowToolbar = autoShowToolbar;
            _colors = colors;
            ReadLineService.CommandHighlighted += ReadLineService_CommandHighlighted;
            ToolbarAttribute = this.GetToolbarAttribute();
            var overflowMilliseconds = ToolbarAttribute?.Timer ?? 500;
            _overflowTimer = new System.Timers.Timer(Math.Clamp(overflowMilliseconds, 500, 5000));
            ReadLineService.CmdLineTextChanged += ReadLineService_CmdLineTextChanged;
        }
        protected virtual void ReadLineService_CmdLineTextChanged(object? sender, CmdLineTextChangedArgs e)
        {
            if (!e.CmdText.StartsWith(Identifier) && LatestHighlightedCommand == Identifier) ClearToolbar();
        }
        public override RunResult Run()
        {
            ClearToolbar();
            return Ok();
        }
        protected void ReadLineService_CommandHighlighted(object? sender, CommandHighlightedArgs e)
        {
            LatestHighlightedCommand = e.CommandName;
            if (e.CommandName != Identifier) return;
            if (_autoShowToolbar)
            {
                _overflowTimer.Elapsed += InitializeToolbar;
                _overflowTimer.AutoReset = true;
                _overflowTimer.Enabled = true;
            }
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