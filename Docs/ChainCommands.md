# Read, Write and navigate to files and directories

From version 1.0.4.0 of PowerCommands you can call two commands using one line, using the | character, at the moment it is limited to two commands but the plan is to allow as many commands as you like in a call chain.

The code below shows an example on how it could be used, in this case I first run the command `output` which is a really simple command that prints out what you input in the flag `--text`, then we use the | to let the runtime know that one more command should be run, in this case it is the file command, I am adding a --target flag with a file name.

https://github.com/user-attachments/assets/b718cc18-2cbb-4812-b3a2-0d054e948a00

## Handling the output from the previous command
If the target command needs the output from the calling command the target command must implement code pickup the output, let´s take a look on how this is implemented in the FileCommand class.

![Alt text](images/file_command.png?raw=true "Handling the output from the previous command")

The first row shows how a command can grab the result from the previous command.

```var latestCommandResult = IPowerCommandsRuntime.DefaultInstance?.Latest;```

If you want create a command that explore this, it could look like this.

```
[PowerCommandDesign(description: "Run commands that supports pipe functionality.",
                        example: "//First run this command and then the version command|version [PIPE] pipe")]
public class PipeCommand(string identifier, PowerCommandsConfiguration configuration) : CdCommand(identifier, configuration)
{
    public override RunResult Run()
    {
        Console.Clear();
        var result = IPowerCommandsRuntime.DefaultInstance?.Latest;
        if (result == null) return Ok();
        WriteLine($"Previous command: {result!.ExecutingCommand.Identifier}");
        WriteLine($"Previous output length: {result!.Output.Length}");
        WriteLine($"Previous command status: {result!.Status}");
        WriteLine($"Previous command input: {result!.Input.Raw}");
        var retVal = Ok();
        return retVal;
    }
}
```

Read more about:

[Read and write files with FileCommand](ReadWriteFileHandler.md)

[Design your Command](Design_command.md)

[Input](Input.md)

[PowerCommands Design Attribute](PowerCommandDesignAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)