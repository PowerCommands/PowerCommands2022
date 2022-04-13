using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description:       "Secret command can encrypt or decrypt a string", 
                arguments:       "Method: should be encrypt or decrypt|Value: value to enrypt or decrypt, if value cointans whitespace use quote parameter instead",
                qutes:           "Only use qute with encryption if your value to encrypt contains whitespaces",
                example:         "secrets enrypt televinken44|secrets decrypt \"EAAAADehRmgu1XZNN+AA1/vSqITeUUFV07Sxd3vhbtkJQWwD\"",
                defaultParameter:"encrypt televinken44")]
[Tags("encryption|secret|example")]
public class SecretsCommand : CommandBase<CommandsConfiguration>
{
    public SecretsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        if (input.Arguments.Length + input.Quotes.Length < 2) throw new MissingFieldException("Two parameters must be provided");
        var method = input.Arguments[0];
        var parameter = string.IsNullOrEmpty(input.Arguments[1]) ? input.SingleQuote : input.Arguments[1];

        if (method == "encrypt")
        {
            var encrypt = EncryptionService.Service.EncryptString(parameter);
            WriteLine($"Input encrypted: {encrypt}", false);
        }
        if (method == "decrypt")
        {
            var decrypt = EncryptionService.Service.DecryptString(parameter);
            WriteLine($"Input decryptet: {decrypt}", false);
        }
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}