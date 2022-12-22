# Secrets

## Configure your environment to use Encryption/Decryption and Secrets functionallity
Run first this command
```
secret
```
This will do a test do encrypt and decrypt a text, this will cause a error that will look something like this.
![Alt text](images/secret_configuration.png?raw=true "Secret configuration")
All you have to do is to run the same command with the --initialize option like this.
```
secret --initialize
```
Now your environment is ready to use Encryption with all your PowerCommands running on that machine. Please note that the name of the environment variable is configurable in the secret.yaml file, **_encryptionManager** is the default name. Also note that values that you encrypt on our machine can not be decrypted on another machine, unless you copy the encryption keys.

Now your environment is ready!

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