namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Metrics
{
    public Cvssmetricv2[] cvssMetricV2 { get; set; }
    public Cvssmetricv30[] cvssMetricV30 { get; set; }
    public Cvssmetricv31[] cvssMetricV31 { get; set; }
}