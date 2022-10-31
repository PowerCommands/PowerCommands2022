namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Weakness
{
    public string source { get; set; }
    public string type { get; set; }
    public Description1[] description { get; set; }
}