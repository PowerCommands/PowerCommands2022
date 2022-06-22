namespace PainKiller.PowerCommands.WindowsCommands.Configuration;

public class FavoriteConfiguration
{
    public string Name { get; set; } = "Music";
    public string NameOfExecutable { get; set; } = "spotify";
    public string FileExtension { get; set; } = "exe";
    public string WorkingDirectory { get; set; } = "";
    public bool WaitForExit { get; set; }
}