# Create a Hello world Command

## There is two ways of doing that, manually our let PowerCommand create one for you, using a template

### Manually
 Create a new class in the Commands directory or copy one existing and just rename it, a command that outputs "Hello World!" should look like this.

``` 
[PowerCommand(  description: "The Hello World classic!", example: "helloworld")]
public class HelloWorldCommand : CommandBase<PowerCommandsConfiguration>
{
    public HelloWorldCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine("Hello World!");
        return Ok();
    }
}
```

When you start the PowerCommand application and type
```
helloworld
```
This is what you get:

![Alt text](images/HelloWorld.png?raw=true "Hello World")

The PowerCommand Core finds your new Command classes automatically, and highlight the name of the command in blue color, you can cycle trough all available commands with the tab key.

## Use the PowerCommand CLI to create a new Command
The PowerCommands framework has creation of new commands to your Visual Studio Solution built in, so you could just start your solution in Visual Studio and type:

```
powercommand new --command HelloWorld
```
And a new command with the name HelloWorldCommand.cs is created in you Commands project.

![Alt text](images/NewCommand.png?raw=true "New Command")

Next step is to read about how you can [Design your Command](Design_command.md).

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)