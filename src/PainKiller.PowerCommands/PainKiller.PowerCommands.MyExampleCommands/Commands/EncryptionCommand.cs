namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(  description: "Encrypt or decrypt your input", 
                      flags: "decrypt",
                    example: "//Encrypt something|encryption \"my decrypted secret\"|//Decrypt your encrypted string|encryption --decrypt EAAAAPv9AsKo6nfHJoFBNmQw9nKZv9PCdLyYhWoJgbovqGQpwY7PmSAkSPO9aagX0kSQyQ==")]
public class EncryptionCommand : CommandBase<CommandsConfiguration>
{
    public EncryptionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine(Input.HasFlag("decrypt") ? EncryptionService.Service.DecryptString(Input.GetFlagValue("decrypt")) : EncryptionService.Service.EncryptString(Input.SingleQuote));
        return Ok();
    }
}