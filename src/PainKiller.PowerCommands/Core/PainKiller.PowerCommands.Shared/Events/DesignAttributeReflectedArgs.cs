using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.Events
{
    public class DesignAttributeReflectedArgs : EventArgs
    {
        public DesignAttributeReflectedArgs(PowerCommandDesignAttribute designAttribute, IConsoleCommand command)
        {
            DesignAttribute = designAttribute;
            Command = command;
        }
        public PowerCommandDesignAttribute DesignAttribute { get; }
        public IConsoleCommand Command { get; }
    }
}