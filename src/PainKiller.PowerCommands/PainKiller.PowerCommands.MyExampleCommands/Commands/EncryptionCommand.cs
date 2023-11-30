namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(tests: "\"my decrypted secret\"|--decrypt EAAAAHYqSRC/lZWEFQRFGDA7wo3UjIKnGgSAMSpN69e/ShsXz0/r75bhQ2KSB5p9dmBOow==")]
[PowerCommandDesign(  description: "Encrypt or decrypt your input", 
                      options: "decrypt",
                    example: "//Encrypt something|encryption \"my decrypted secret\"|//Decrypt your encrypted string|encryption --decrypt EAAAAHYqSRC/lZWEFQRFGDA7wo3UjIKnGgSAMSpN69e/ShsXz0/r75bhQ2KSB5p9dmBOow==")]
public class EncryptionCommand : CommandBase<CommandsConfiguration>
{
    public EncryptionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine(Input.HasOption("decrypt") ? EncryptionService.Service.DecryptString(Input.GetOptionValue("decrypt")) : EncryptionService.Service.EncryptString(Input.SingleQuote));
        return Ok();
    }
}