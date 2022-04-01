using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

public class ConfigurationCommand : CommandBase<BasicCommandsConfiguration>
{
    public ConfigurationCommand(string identifier, BasicCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input?.Arguments.First() == "create")
        {
            var fileName = ConfigurationManager.Update(new BasicCommandsConfiguration());
            return new(this, input, $"A new default file named {fileName} has been created in the root directory", RunResultStatus.Ok);
        }
        return new(this, input!, $"", RunResultStatus.Ok);
    }
}