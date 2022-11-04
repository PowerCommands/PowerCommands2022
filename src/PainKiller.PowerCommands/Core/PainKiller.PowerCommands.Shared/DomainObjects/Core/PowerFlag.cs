namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class PowerFlag
{
    public PowerFlag(string attributeValue)
    {
        IsRequired = attributeValue.StartsWith("!");
        Name = attributeValue.Replace("!", "");
    }
    public string Name { get; set; }
    public string Value { get; set; } = "";
    public bool IsRequired { get; set; }
}