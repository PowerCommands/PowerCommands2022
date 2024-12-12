# CommandBase

CommandBase is the base class for most of your Command classes, it has some practical help classes that you could use in your Command classes.

But before that lets have a look on the contract that all Commands must implement to work properly and... of course to even compile.

## IConsoleCommand
```
public interface IConsoleCommand
{
    string Identifier { get; }
    void InitializeRun(ICommandLineInput input);
    RunResult Run();
    Task<RunResult> RunAsync();
}
```
The Identifier and the InitializeRun will be used by the Core Framework, your will use Run or RunAsync when you are either imlementing them in your Command directly but more probably indiractly when you are using the CommandBase as your base class and overrride the Run/RunAsync method.

## Class diagram
![Alt text](images/CommandBase.png?raw=true "Command Base")

As you could see there are helper methods and the important properties Configuration and Input. Input is crucial for the Run methods without access to the Input, your code is "blind" and could only do static things that ignores the console input. The helper methods is for creating the return result for the Run methods and other helper is for [output](ConsoleOutput.md) to the console.

## Create your own BaseCommand

There are many use cases where it makes sense to create a base class for your other commands in your CLI application, lets say that you always need access to a keyvault and you do not want to repeat that code in every command you do. Here are som guidelines to help you.
 - The name should not end with Command, some examples on good naming could be VaultCommandBase or EntityFrameworkCommandBase
 - If the class only is used to be inherited by other commands, it is good ide to declare it as an abstract class.
 - Let your base class implement CommandBase, that way you have all the helpers methods.

 Read more about:

[Design your Command](Design_command.md)

[Output to the Console guideline](ConsoleService.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)