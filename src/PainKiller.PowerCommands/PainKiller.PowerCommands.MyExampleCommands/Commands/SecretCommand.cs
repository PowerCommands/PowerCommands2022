using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Security.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("encryption|secret|example|security")]
[PowerCommand(description: "Get,set or view secrets", 
                arguments: "method: methods are (use,get,remove) or leave empty, set will create a new variabel by the secret provicer service (default is Environment Variable) if omitted, method is view",
                    qutes: "name: name of the secret if get or set is used as method, if method is view, name is ignored",
                  example: "secret|secret get \"mycommand-pass\"|secret set \"mycommand-pass\"")]
public class SecretsCommand : CommandBase<CommandsConfiguration>
{
    public SecretsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        if ((input.Arguments.Length + input.Quotes.Length < 2) && input.Arguments.Length > 0) throw new MissingFieldException("Two parameters must be provided");
        if (input.Arguments.Length == 0) return List(input);

        var method = input.Arguments[0];
        if (method == "get") return Get(input);
        if (method == "set") return Set(input);
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
        var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name);
        if (secret == null) return CreateBadParameterRunResult(input, $"No secret with name \"{name}\" found.");

        var val = SecretService.Service.GetSecret(name, secret.Options, EncryptionService.Service.DecryptString);
        this.WriteObjectDescription(name, val);

        return CreateRunResult(input);
    }
    private RunResult Set(CommandLineInput input)
    {
        var name = input.SingleQuote;
        Console.Write("Enter secret: ");
        var password = PasswordPromptService.Service.ReadPassword();

        var secret = new SecretItemConfiguration {Name = name};
        var val = SecretService.Service.SetSecret(name, password, secret.Options, EncryptionService.Service.EncryptString);

        Configuration.Secret.Secrets.Add(secret);
        ConfigurationService.Service.SaveChanges(Configuration);

        WriteHeadLine("New secret created and stored in configuration file");
        this.WriteObjectDescription(name, val);

        return CreateRunResult(input);
    }
    private RunResult Remove(CommandLineInput input)
    {
        var name = input.SingleQuote;

        var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name);
        if (secret == null) return CreateBadParameterRunResult(input, $"No secret with name \"{name}\" found.");

        Configuration.Secret.Secrets.Remove(secret);
        ConfigurationService.Service.SaveChanges(Configuration);

        WriteHeadLine("Secret removed from configuration file, but is still left in the secret provider storage and has to be removed manually");
        return CreateRunResult(input);
    }
}