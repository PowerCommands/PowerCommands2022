namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Configuration
{
    public Node[] nodes { get; set; }
    public string _operator { get; set; }
    public bool negate { get; set; }
}