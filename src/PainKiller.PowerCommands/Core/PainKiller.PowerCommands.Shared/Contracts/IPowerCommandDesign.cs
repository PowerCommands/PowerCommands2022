namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface IPowerCommandDesign
    {
        string Description { get; }
        string Arguments { get; }
        string Quotes { get; }
        string Options { get; }
        bool? UseAsync { get; }
        string Examples { get; }
        string Suggestions { get; }
        bool? ShowElapsedTime { get; }
    }
}