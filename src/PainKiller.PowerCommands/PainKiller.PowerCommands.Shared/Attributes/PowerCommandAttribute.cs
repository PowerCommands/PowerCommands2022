namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PowerCommandAttribute : Attribute
{
    public string Description { get; }
    public string Arguments { get; }
    public string Qutes { get; }

    public PowerCommandAttribute(string description, string arguments = "", string qutes = "")
    {
        Description = description;
        Arguments = arguments;
        Qutes = qutes;
    }
}