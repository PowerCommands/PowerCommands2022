# Secrets

## Configure your environment to use Encryption/Decryption and Secrets functionallity
## Good to know
Secret is initialized the fist time you startup a new power command project on your machine, the same keys for encryption/decryption is used by all your PowerCommands based applications on the same machine.
If you setup PowerCommands on a diffrent machine, a new key for encryption/decryption will be used, meaning you have to create the secrets again on that machine. But you can move the keys manually, but I do not think that you should do that.
## Use the secrets built in functionallity 
Create secret with built in commands named secret, like this:
```
secret create "localDB"
```
This will create a secret item in configuration, and a encryptet secret will be stored in a EnvironmentVariable with the proviede name.
In you command you could get the decrypted secret like this.
```
var cn = Configuration.Secret.DecryptSecret("Server=.;Database=timelineLocalDB;User Id=sa;Password=##localDB##;"); 
```

Another simple usa case where you decrypt your secret access token. Lets pretend that you have created a secret named **AWS_TOKEN**.

```
var accessToken = Configuration.Secret.DecryptSecret("##AWS_TOKEN##");
```

Sometimes you have to pass a configuration element to a thirdparty or custom module, then this could be usefull, it creates a new clone of the configuration and you pass that, its a good pattern ratcher then pass the runtime configuration instance that could lead to unpredictable result and in worst case revealing the decrypted secret by mistake.
```
var decryptedCloneConfiguration = Configuration.Secret.DecryptSecret(config.SourceSetting, nameof(config.SourceSetting.ConnectionString));
```
You pass in the configuration and the property name that has a tagged secret and you get a clone of the configuration back where the property value decrypted. In the example above it is the **ConnectionString** property that is decrypted.


## Be carefull when decrypting, be sure to protect your secrets in runtime
It is very easy to expose a decrypted value by mistake, the decryption should be in the same scope or in very near scope of the usage. It should not be passed around with the other configuration values and reside in runtime as long as the application executes. The risk is that for example the decrypted value is logged for some reason you cant predict and the secret is logged as clear text, in other words it is revealed and must be changed.

### Recomended pattern for custom Compenents using secrets, pass the DecryptSecret function
A pattern to reduce this risk could be to send the DecryptSecret function to the target rather then send the decrypted configuration, like this example below. In this real world use case I using a Custom Component, I want to implement the secret handling as late as possible but avoid to create a depandancy between the custom component and PowerCommands. I have to modify the Custom component a bit but no dependancy is needed. 
```
//First I add this to the class in the custom component that will use the connction string.
private static Func<string, string> _decryptSecretsFunc;
public static void SetDecryptSecretFunction(Func<string, string> decryptSecretsFunc) => _decryptSecretsFunc = decryptSecretsFunc;

//In the same class when the connection string needs to be decrypted the function will be invoked.
var cnString = _decryptSecretsFunc == null ? dss.ConnectionString : _decryptSecretsFunc.Invoke(dss.ConnectionString);
using (var connection = new SqlConnection(cnString))

//From the PowerCommand class you need to pass the DecryptSecret function like this
SqlImport.SetDecryptSecretFunction(Configuration.Secret.DecryptSecret);
```

Read more about:

[Basic application configuration](Configuration.md)

[Extend your configuration](ExtendYourConfiguration.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)