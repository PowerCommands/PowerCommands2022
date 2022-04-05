using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Core.BaseClasses;

public abstract class EncryptionCommandBase : CommandBase<CommandsConfiguration>
{
    protected EncryptionCommandBase(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    protected string EncryptString(string plainText) => EncryptionService.Service.EncryptString(plainText);
    protected string DecryptString(string plainText) => EncryptionService.Service.DecryptString(plainText);
}