# Input

## Description
The input is an object that interprets the user input to the running console.

```
public interface ICommandLineInput
{
    string Raw { get; init; }
    string Identifier { get; init; }
    string[] Quotes { get; init; }
    string[] Arguments { get; init; }
    string[] options { get; init; }
    string SingleArgument { get; }
    string SingleQuote { get; }
    string? Path { get; init; }
}
```
## How is it initialized and used by the command?
After the user hits return the input will be intpreted by the PowerCommands Runtime and if a valid Command could be matched, this Command will be created in runtime and Initialized with the CommandLineInput instance. It will then be programaticallay available in the Commands Run method.

## How is the user input mapped to the CommandLineInput instance?
![Alt text](images/Command_line_input_described.png?raw=true "Describe convert command")

### Path
Path is not shown in the example below, it is a special input when your only input except for the Identifier is a path, it is simply explained "the rest" of the input after the Identifier, and it must be a valid file path.

## Use the CommandLineInput instance in code
It could look something like this.

```
public override RunResult Run()
{
    if(Input.HasOption("hello")) WriteLine("Hello World!");
    return CreateRunResult();
}
```

## Properties and it´s usage
For this example let´s imagine the user has typed in the following input in the console.

``` demo argumentOne "This is a qoute" --demo myOptionValue ```

### Raw
The raw input as it was typed by the user in the Console.
### Identifier
The Id of the command it is the first argument that the user types, the id is the prefix of the command class file, if the id is demo the Command class file is named **DemoCommand** and the file should be named **DemoCommand.cs**. 
### Quotes
All input strings surrounded with " (quotation mark) in this example it is one quote with the value **"This is a quote"** (This value will be returned by the **SingleQuote** property)
### Arguments
All input strings not surrounded with " except for the first one (wich is the Identifier) and every argument starting with -- marks in the example it is one argument with the value **argumentOne** (This value will be returned by the **SingleArgument** property)
### Options
All arguments that is staring with the -- marks is considered as options, and ever argument after the option is considered as that options value, in the example the option **demo** has the value **myOptionValue** a option must not have a value.
### Path
The path is a special value if your command only should have an input that is a path, then it is usefull, you do not need to input the path in quotation marks even if it contains blank space.

Read more about:

[Design your Command](Design_command.md)

[Options](Options.md)

[PowerCommands Attribute](PowerCommandAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)
