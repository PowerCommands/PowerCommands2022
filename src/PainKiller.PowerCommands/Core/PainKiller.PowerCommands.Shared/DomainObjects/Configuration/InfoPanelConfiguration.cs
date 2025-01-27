namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

public class InfoPanelConfiguration
{
    public bool Use { get; set; }
    public bool AutoAdjustOnResize { get; set; } = true;
    public string Color { get; set; } = ConsoleColor.DarkMagenta.ToString();
    public int Height { get; set; } = 2;
    public int UpdateIntervalSeconds { get; set; } = 60;
    public ConsoleColor GetConsoleColor() => (ConsoleColor) Enum.Parse(typeof(ConsoleColor), Color);
}