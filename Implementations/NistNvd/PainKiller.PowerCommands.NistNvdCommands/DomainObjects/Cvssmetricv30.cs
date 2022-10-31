namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Cvssmetricv30
{
    public string source { get; set; }
    public string type { get; set; }
    public Cvssdata1 cvssData { get; set; }
    public float exploitabilityScore { get; set; }
    public float impactScore { get; set; }
}