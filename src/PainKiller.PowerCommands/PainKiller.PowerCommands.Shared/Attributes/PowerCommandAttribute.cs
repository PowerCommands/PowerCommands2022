namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PowerCommandAttribute : Attribute
{
    public string Description { get; }
    public string Arguments { get; }
    public string Qutes { get; }
    public bool UseAsync { get; }
    public string Examples { get; }
    public string DefaultParameter { get; }
    public PowerCommandAttribute(string description, string arguments = "", string qutes = "", string example = "", string defaultParameter = "", bool useAsync = false)
    {
        Description = description;
        Arguments = arguments;
        Qutes = qutes;
        UseAsync = useAsync;
        Examples = example;
        DefaultParameter = defaultParameter;
    }
}