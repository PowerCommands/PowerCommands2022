using System.ComponentModel;

namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PowerCommandDesignAttribute : Attribute
{
    public string Description { get; }
    [Description("Separate items with | character, if required begin with a ! character")]
    public string Arguments { get; }
    [Description("Separate items with | character, if required begin with a ! character")]
    public string Quotes { get; }
    [Description("Separate items with | character, if required begin with a ! character")] 
    public string Flags { get; }
    [Description("Separate items with | character, if required begin with a ! character")]
    public string Secrets { get; }
    [Description("The command will exexute the RunAsync instead of Run method")]
    public bool UseAsync { get; }
    [Description("This mean that the command itself will handle the --help in any way")]
    public bool OverrideHelpFlag { get; }
    [Description("Separate items with |, if you begin with // the value will be displayed as an comment row in help view.")]
    public string Examples { get; }
    public string Suggestion { get; }
    public PowerCommandDesignAttribute(string description, bool overrideHelpFlag = false, string arguments = "", string quotes = "", string example = "", string flags = "", string secrets = "", string suggestion = "", bool useAsync = false)
    {
        Description = description;
        OverrideHelpFlag = overrideHelpFlag;
        Arguments = arguments;
        Quotes = quotes;
        Flags = flags;
        Examples = example;
        Suggestion = suggestion;
        UseAsync = useAsync;
        Secrets = secrets;
    }
}