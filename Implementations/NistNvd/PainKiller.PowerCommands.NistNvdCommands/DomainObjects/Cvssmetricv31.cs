namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Cvssmetricv31
{
    public string source { get; set; }
    public string type { get; set; }
    public Cvssdata2 cvssData { get; set; }
    public float exploitabilityScore { get; set; }
    public float impactScore { get; set; }
}