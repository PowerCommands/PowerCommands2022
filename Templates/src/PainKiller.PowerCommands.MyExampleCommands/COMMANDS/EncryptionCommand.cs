namespace $safeprojectname$.Commands;

[PowerCommandTest(tests: "\"my decrypted secret\"|--decrypt EAAAAPv9AsKo6nfHJoFBNmQw9nKZv9PCdLyYhWoJgbovqGQpwY7PmSAkSPO9aagX0kSQyQ==")]
[PowerCommandDesign(  description: "Encrypt or decrypt your input", 
                      options: "decrypt",
                    example: "//Encrypt something|encryption \"my decrypted secret\"|//Decrypt your encrypted string|encryption --decrypt EAAAAPv9AsKo6nfHJoFBNmQw9nKZv9PCdLyYhWoJgbovqGQpwY7PmSAkSPO9aagX0kSQyQ==")]
public class EncryptionCommand : CommandBase<CommandsConfiguration>
{
    public EncryptionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine(Input.HasOption("decrypt") ? EncryptionService.Service.DecryptString(Input.GetOptionValue("decrypt")) : EncryptionService.Service.EncryptString(Input.SingleQuote));
        return Ok();
    }
}