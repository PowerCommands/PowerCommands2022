using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class PowerCommandDesignConfiguration : IPowerCommandDesign
    {
        public PowerCommandDesignConfiguration() { }

        public PowerCommandDesignConfiguration(PowerCommandDesignConfiguration cfg, PowerCommandDesignAttribute attr)
        {
            Description = string.IsNullOrEmpty(cfg.Description) ? attr.Description : cfg.Description;
            Arguments = string.IsNullOrEmpty(cfg.Arguments) ? attr.Arguments : cfg.Arguments;
            Quotes = string.IsNullOrEmpty(cfg.Quotes) ? attr.Quotes : cfg.Quotes;
            Options = string.IsNullOrEmpty(cfg.Options) ? attr.Options : cfg.Options;
            Examples = string.IsNullOrEmpty(cfg.Examples) ? attr.Examples : cfg.Examples;
            Suggestions = string.IsNullOrEmpty(cfg.Suggestions) ? attr.Suggestions : cfg.Suggestions;
            UseAsync = cfg.UseAsync ?? attr.UseAsync;
            ShowElapsedTime = cfg.ShowElapsedTime ?? attr.ShowElapsedTime;
        }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string Arguments { get; set; } = String.Empty;
        public string Quotes { get; set; } = String.Empty;
        public string Options { get; set; } = String.Empty;
        public string Examples { get; set; } = String.Empty;
        public string Suggestions { get; set; } = String.Empty;
        public bool? UseAsync { get; set; } = null;
        public bool? ShowElapsedTime { get; set; } = null;
    }
}