using PainKiller.PowerCommands.Security.Services;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Encryption example, prompts you for a password an masks the input, displays it encrypted and gives you the option to show it in clear text",
                  example: "password|encrypt")]
public class PasswordCommand : CommandBase<CommandsConfiguration>
{
    public PasswordCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        Console.Write("Password: ");
        var password = PasswordPromptService.Service.ReadPassword();
        var encrypted = EncryptionService.Service.EncryptString(password);
        Console.WriteLine();
        WriteHeadLine("Encrypted password", false);
        WriteLine(encrypted);

        Console.Write("Do you want to see the password in clear text? y/? :");
        var response = Console.ReadLine();
        if (response != null && response.StartsWith('y')) Console.WriteLine(password);
        return Ok();
    }
}