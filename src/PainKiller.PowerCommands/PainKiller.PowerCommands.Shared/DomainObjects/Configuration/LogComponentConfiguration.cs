namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

public class LogComponentConfiguration : BaseComponentConfiguration
{
    public LogComponentConfiguration()
    {
        Name = "Serialog";
        Component = "PainKiller.SerilogExtensions.dll";
        Checksum = "173831af7e77b8bd33e32fce0b4e646d";
    }
    public string FileName { get; set; } = "powercommands-log.txt";
    public string RollingIntervall { get; set; } = "Day";
    public string RestrictedToMinimumLevel { get; set; } = "Information";
    public string OutputTemplate { get; set; } = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
}