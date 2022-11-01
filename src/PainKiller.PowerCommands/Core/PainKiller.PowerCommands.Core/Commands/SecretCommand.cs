using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Security.Services;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Commands;
[PowerCommand(description: "Get, creates, removes or view secrets, first you need to configure your encryption key with initialize argument",
                    flags: "create|get|remove|salt",
                  example: "//View all declared secrets|secret|secret --get \"mycommand-pass\"|secret --create \"mycommand-pass\"|secret --remove \"mycommand-pass\"|Initialize your machine with a new encryption key (stops if this is already done)|secret --initialize")]
public class SecretCommand : CommandBase<CommandsConfiguration>
{
    public SecretCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (Input.HasFlag("salt")) return Salt();
        if ((Input.Arguments.Length + Input.Quotes.Length < 2) && Input.Arguments.Length > 0) throw new MissingFieldException("Two parameters must be provided");
        if (Input.Arguments.Length == 0 || Input.Arguments[0] == "view") return List();

        if (Input.HasFlag("get")) return Get();
        if (Input.HasFlag("create")) return Create();
        if (Input.HasFlag("remove")) return Remove();

        return CreateBadParameterRunResult("No matching parameter");
    }

    private RunResult Salt()
    {
        Console.WriteLine(IEncryptionService.GetRandomSalt());
        return CreateRunResult();
    }
    private RunResult List()
    {
        foreach (var secret in Configuration.Secret.Secrets) ConsoleService.WriteObjectDescription($"{GetType().Name}", secret.Name, $"{string.Join(',', secret.Options.Keys)}");
        return CreateRunResult();
    }
    private RunResult Get()
    {
        var name = Input.SingleQuote;
        var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
        if (secret == null) return CreateBadParameterRunResult($"No secret with name \"{name}\" found.");

        var val = SecretService.Service.GetSecret(name, secret.Options, EncryptionService.Service.DecryptString);
        ConsoleService.WriteObjectDescription($"{GetType().Name}", name, val);

        return CreateRunResult();
    }
    private RunResult Create()
    {
        var name = Input.SingleQuote;
        Console.Write("Enter secret: ");
        var password = PasswordPromptService.Service.ReadPassword();
        Console.WriteLine();
        Console.Write("Confirm secret: ");
        var passwordConfirm = PasswordPromptService.Service.ReadPassword();

        if (password != passwordConfirm)
        {
            Console.WriteLine();
            return CreateBadParameterRunResult("Passwords do not match");
        }

        var secret = new SecretItemConfiguration { Name = name };
        var val = SecretService.Service.SetSecret(name, password, secret.Options, EncryptionService.Service.EncryptString);

        Configuration.Secret.Secrets.Add(secret);
        ConfigurationService.Service.SaveChanges(Configuration);
        Console.WriteLine();
        WriteHeadLine("New secret created and stored in configuration file");
        ConsoleService.WriteObjectDescription($"{GetType().Name}", name, val);

        return CreateRunResult();
    }
    private RunResult Remove()
    {
        var name = Input.SingleQuote;

        var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
        if (secret == null) return CreateBadParameterRunResult($"No secret with name \"{name}\" found.");

        Configuration.Secret.Secrets.Remove(secret);
        ConfigurationService.Service.SaveChanges(Configuration);

        WriteHeadLine("Secret removed from configuration file\nManually remove the secret key from environment variables or vault depending on how they are stored.");
        return CreateRunResult();
    }
}