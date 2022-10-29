# PowerCommandsRuntime

The PowerCommandsRuntime is the Core engine you could say, this runtime first enumerates all the valid Commands using reflection. It is also responseble to run the interpret the input from the commandline or from args provided at the application startup. 

## The contract

```
public interface IPowerCommandsRuntime
{
    string[] CommandIDs { get; }
    RunResult ExecuteCommand(string rawInput);
    List<IConsoleCommand> Commands { get; }
    public static IPowerCommandsRuntime? DefaultInstance { get; protected set; }
}
```

It is not a complex class at all and you could if you want build your own and change the runtime, the runtime instance is created in the Commands project by the **PowerCommandServices** class.

In short this is what the runtime does.

 - Encapsulates the execution of the Run and RunAsync method, and handles exception if that is the result.
 - Search, enumerates and holds all valid PowerCommands from all the DLL components that are declared in the [configuration](Configuration.md) file.
 - Interprets the input form the commandline and transform that to a [Input](Input.md) instance.

## The rule to find Commands

A valid Command is a class, the name ends with Command and is not abstract.

Read more about:

[Extend your configuration](ExtendYourConfiguration.md)

[Handling the Input](Input.md)