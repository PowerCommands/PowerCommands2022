# Create a new Command

## There is two ways of doing that, the first one is to create a new class in the Commands directory, the class shoult look like this.

``` 
[PowerCommand(  description: "The Hello World classic!", example: "helloworld")]
public class HelloWorldCommand : CommandBase<PowerCommandsConfiguration>
{
    public HelloWorldCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine("Hello World!");
        return CreateRunResult();
    }
}
```

When you start the PowerCommand applikcation and type
```
helloworld
```
This is what you get:

![Alt text](images/HelloWorld.png?raw=true "Hello World")

The PowerCommand Core finds your new Command class automatically, and can  there fore hightligt the name of the command in blue color and of course execute the command.

## Use the PowerCommand CLI to create a new Command
The Core has creation of new commands to your Visual Studio Solution built in, just type:

![Alt text](images/NewCommand.png?raw=true "New Command")
And a new command with the name HelloWorldCommand.cs is created in you Commands project.