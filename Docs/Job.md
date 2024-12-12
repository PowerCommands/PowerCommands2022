# Run your command as a job
A PowerCommands application can be started with arguments from the commandline which is useful when you want to use your PowerCommands implementation to perform something as a automated job.

The code below shos a dummie example of how you design your Command to run as an automated task, notice the **return Quit()** at the end of the Run() method, that will trigger the program to quit the application. 

![Alt text](images/job.png?raw=true "job")

You just startup your PowerCommands application from with the parameter "job", like this:
```
"powercommands.exe job"
```
Your PowerCommands application may of course have so other name and your command could have Input parameters that will be passed on from the command line like this:
```
"powercommands.exe job argument1"
```

## What if the command you want to run just once is not designed to quit?
If the command inherits from the CommandBase class, which most if not all Commands are, you can use the option `--pc_force_quit` to quit the application after the command is finished. If the command is running as async, this could cause unexpected behavior, be aware of that.

## Run PowerCommand with a service account and use secrets
If you want to run your PowerCommands application as a Windows scheduled task started by a service accounts that is not allowed to login on the machine, you need to do a couple of steps to make sure that the service account user can use the decryption functionality.
The easy way is to delete the setup.yaml file in the application folder and start your Power Command application again, this will guide you through the setup.

- First answer `y` to the question if you want to setup encryption.
- Answer `y` to the question that you intend to run your application using a service account.
- Last you need to update the `PowerCommandsConfiguration.yaml` file with the encryption element.

It could look something like this:
```
encryption:
  sharedSecretEnvironmentKey: '_encryptionManager'
  sharedSecretSalt: WJtOL/ZgbHwVhbL76JGFyA==
  iterationCount: 10000
  keySize: 256
```

Read more about:

[Design your Command](Design_command.md)

[Options](Options.md)

[PowerCommands Design Attribute](PowerCommandDesignAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)
