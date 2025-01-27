using PainKiller.PowerCommands.Security.Services;

namespace PainKiller.PowerCommands.Core.Commands
{
    [PowerCommandDesign(description: "Get, creates, removes or view secrets, first you need to configure your encryption key with initialize argument",
        options: "create|initialize|configuration|remove|salt",
        disableProxyOutput: true,
        example: "//View all declared secrets|secret|secret --create \"mycommand-pass\"|secret --remove \"mycommand-pass\"|//Initialize your machine with a new encryption key (stops if this is already done)|secret --initialize")]
    public class SecretCommand(string identifier, CommandsConfiguration configuration) : CommandBase<CommandsConfiguration>(identifier, configuration)
    {
        public override RunResult Run()
        {
            if (Input.HasOption("initialize")) return Init();
            if (Input.HasOption("")) return CheckEncryptConfiguration();
            if (Input.HasOption("salt")) return Salt();
            if (Input.HasOption("create")) return Create();
            if (Input.HasOption("remove")) return Remove();
            if ((Input.Arguments.Length + Input.Quotes.Length < 2) && Input.Arguments.Length > 0) throw new MissingFieldException("Two parameters must be provided");
            if (Input.Arguments.Length == 0 || Input.Arguments[0] == "view") return List();

            return BadParameterError("No matching parameter");
        }
        private RunResult CheckEncryptConfiguration()
        {
            try
            {
                var encryptedString = EncryptionService.Service.EncryptString("Encryption is setup properly");
                var decryptedString = EncryptionService.Service.DecryptString(encryptedString);
                WriteLine(encryptedString);
                WriteLine(decryptedString);
            }
            catch
            {
                Console.WriteLine("");
                WriteError("Encryption is not configured properly");
            }
            return Ok();
        }
        private RunResult Salt()
        {
            Console.WriteLine(AESEncryptionManager.GetStrongRandomString());
            return Ok();
        }

        private RunResult Init()
        {
            var sharedSecret = AESEncryptionManager.GetStrongRandomString();
            var salt = AESEncryptionManager.GetStrongRandomString(desiredByteLength: 16);
            Environment.SetEnvironmentVariable("_encryptionManager", sharedSecret, EnvironmentVariableTarget.User);
            var securityConfig = new SecurityConfiguration { Encryption = new EncryptionConfiguration { SharedSecretEnvironmentKey = "_encryptionManager", SharedSecretSalt = salt } };
            var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, ConfigurationGlobals.SecurityFileName);
            ConfigurationService.Service.Create(securityConfig, fileName);
            WriteSuccessLine($"File {fileName} saved OK, you will need to restart the application before the changes take effect.");
            return Ok();
        }

        private RunResult List()
        {
            foreach (var secret in Configuration.Secret.Secrets) ConsoleService.Service.WriteObjectDescription($"{GetType().Name}", secret.Name, $"{string.Join(',', secret.Options.Keys)}");
            return Ok();
        }
        private RunResult Create()
        {
            var name = Input.SingleQuote;
            var password = DialogService.SecretPromptDialog("Enter secret:");
            if (string.IsNullOrEmpty(password)) return BadParameterError("Passwords do not match");

            var secret = new SecretItemConfiguration { Name = name };
            var val = SecretService.Service.SetSecret(name, password, secret.Options, EncryptionService.Service.EncryptString);

            Configuration.Secret.Secrets ??= new();
            Configuration.Secret.Secrets.Add(secret);
            ConfigurationService.Service.SaveChanges(Configuration);
            Console.WriteLine();
            WriteHeadLine("New secret created and stored in configuration file");
            ConsoleService.Service.WriteObjectDescription($"{GetType().Name}", name, val);

            return Ok();
        }
        private RunResult Remove()
        {
            var name = Input.SingleQuote;

            var secret = Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
            if (secret == null) return BadParameterError($"No secret with name \"{name}\" found.");

            Configuration.Secret.Secrets.Remove(secret);
            ConfigurationService.Service.SaveChanges(Configuration);

            WriteHeadLine("Secret removed from configuration file\nManually remove the secret key from environment variables or vault depending on how they are stored.");
            return Ok();
        }
    }
}