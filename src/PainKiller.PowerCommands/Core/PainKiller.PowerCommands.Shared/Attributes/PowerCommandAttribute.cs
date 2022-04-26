namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PowerCommandAttribute : Attribute
{
    public string Description { get; }
    public string Arguments { get; }
    public bool ArgumentMandatory { get; }
    public string Qutes { get; }
    public bool QutesMandatory { get; }
    public bool UseAsync { get; }
    public string Examples { get; }
    public string Suggestion { get; }
    public PowerCommandAttribute(string description, string arguments = "", string qutes = "", string example = "", string suggestion = "", bool useAsync = false, bool argumentMandatory = false, bool qutesMandatory = false)
    {
        Description = description;
        Arguments = arguments;
        Qutes = qutes;
        Examples = example;
        Suggestion = suggestion;
        UseAsync = useAsync;
        ArgumentMandatory = argumentMandatory;
        QutesMandatory = qutesMandatory;
    }
}