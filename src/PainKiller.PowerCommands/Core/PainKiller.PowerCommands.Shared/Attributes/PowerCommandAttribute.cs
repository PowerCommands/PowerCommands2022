using System.ComponentModel;

namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PowerCommandAttribute : Attribute
{
    public string Description { get; }
    public string Arguments { get; }
    public bool ArgumentMandatory { get; }
    public string Qutes { get; }
    public string Flags { get; }
    public bool QutesMandatory { get; }
    public bool UseAsync { get; }
    [Description("This mean that the command itself will handle the --help in any way")]
    public bool OverrideHelpFlag { get; }
    public string Examples { get; }
    public string Suggestion { get; }
    public PowerCommandAttribute(string description, bool overrideHelpFlag = false, string arguments = "", string qutes = "", string example = "", string flags = "", string suggestion = "", bool useAsync = false, bool argumentMandatory = false, bool qutesMandatory = false)
    {
        Description = description;
        OverrideHelpFlag = overrideHelpFlag;
        Arguments = arguments;
        Qutes = qutes;
        Flags = flags;
        Examples = example;
        Suggestion = suggestion;
        UseAsync = useAsync;
        ArgumentMandatory = argumentMandatory;
        QutesMandatory = qutesMandatory;
    }
}