using $safeprojectname$.Attributes;
using $safeprojectname$.Contracts;

namespace $safeprojectname$.Events;

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