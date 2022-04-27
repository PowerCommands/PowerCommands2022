using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Security.Services;

namespace PainKiller.PowerCommands.Core.Commands;
[Tags("core|encryption|secret|security")]
[PowerCommand(description: "Get, creates, removes or view secrets",
                arguments: "create|get|remove|view (default)",
                    qutes: "name:<name>",
                  example: "secret|secret get \"mycommand-pass\"|secret create \"mycommand-pass\"")]
public class SecretCommand : CommandBase<CommandsConfiguration>
{
    public SecretCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if ((input.Arguments.Length + input.Quotes.Length < 2) && input.Arguments.Length > 0) throw new MissingFieldException("Two parameters must be provided");
        if (input.Arguments.Length == 0 || input.Arguments[0] == "view") return List(input);

        var method = input.Arguments[0];
        if (method == "get") return Get(input);
        if (method == "create") return Create(input);
        if (method == "remove") return Remove(input);

        return CreateRunResult(input);
    }

    private RunResult List(CommandLineInput input)
    {
        foreach (var secret in Configuration.Secret.Secrets) this.WriteObjectDescription(secret.Name, $"{string.Join(',', secret.Options.Keys)}");
        return CreateRunResult(input);
    }
    private RunResult Get(CommandLineInput input)
    {
        var name = input.SingleQuote;
        var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
        if (secret == null) return CreateBadParameterRunResult(input, $"No secret with name \"{name}\" found.");

        var val = SecretService.Service.GetSecret(name, secret.Options, EncryptionService.Service.DecryptString);
        this.WriteObjectDescription(name, val);

        return CreateRunResult(input);
    }
    private RunResult Create(CommandLineInput input)
    {
        var name = input.SingleQuote;
        Console.Write("Enter secret: ");
        var password = PasswordPromptService.Service.ReadPassword();

        var secret = new SecretItemConfiguration { Name = name };
        var val = SecretService.Service.SetSecret(name, password, secret.Options, EncryptionService.Service.EncryptString);

        Configuration.Secret.Secrets.Add(secret);
        ConfigurationService.Service.SaveChanges(Configuration);
        Console.WriteLine();
        WriteHeadLine("New secret created and stored in configuration file");
        this.WriteObjectDescription(name, val);

        return CreateRunResult(input);
    }
    private RunResult Remove(CommandLineInput input)
    {
        var name = input.SingleQuote;

        var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
        if (secret == null) return CreateBadParameterRunResult(input, $"No secret with name \"{name}\" found.");

        Configuration.Secret.Secrets.Remove(secret);
        ConfigurationService.Service.SaveChanges(Configuration);

        WriteHeadLine("Secret removed from configuration file\nManually remove the secret key from environment variables or vault depending on how they are stored.");
        return CreateRunResult(input);
    }
}