namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Cvssmetricv2
{
    public string source { get; set; }
    public string type { get; set; }
    public Cvssdata cvssData { get; set; }
    public float exploitabilityScore { get; set; }
    public float impactScore { get; set; }
    public bool acInsufInfo { get; set; }
    public bool obtainAllPrivilege { get; set; }
    public bool obtainUserPrivilege { get; set; }
    public bool obtainOtherPrivilege { get; set; }
    public bool userInteractionRequired { get; set; }
}