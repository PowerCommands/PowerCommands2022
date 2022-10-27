# Design your command

Think of your command as an one line command with some parameters in a cmd prompt environment, it should do a single isolated task, for example lets say you want to convert yaml file to json or xml. A good name could be **ConvertCommand**, as parameters you have a path to the input file, and a flag for the format and a value to that flag. That is a pretty good design for one Command. 
 
 The usage of this command will look like this if I want to convert my yaml fil to json.

```convert "C:\temp\myYaml.yaml" --format json```

If you want the input to be more self described, which the framework encourages you to do for clarity, you can choose to add a flag for the filepath like this:

```convert --file "C:\temp\myYaml.yaml" --format json```
 
There are other ways you can solve the design to, you can solve it with two Commands instead, one command named **XmlCommand** and another named **JsonCommand**, it is all up to you.

## Example code
### Use case
You want a simple Command that converts a yaml file to json or xml format. Your first architecture descision is, one command or two? Well, it is a matter of taste but for this example the choise is one single command that handles the conversion to both formats. We use two flags, one for the filename, named path and one for the format named format. I exclude the implementation code to keep the code example short. We pretend that we have a static class handling this format conversions.

```
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(  description: "Converting yaml format to json or xml format",
                flags:"path|format",
                example: "convert --path \"c:\\temp\\test.yaml\" --format json")]
public class ConvertCommand : CommandBase<PowerCommandsConfiguration>
{
    public ConvertCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        YamlFormatManger.Convert(Input.GetFlagValue("path"), Input.GetFlagValue("format"));
        return CreateRunResult();
    }
}
```

Notice the practical PowerCommand attribute that let´s you decribe the usage for the consumer of this command.
If the consumer of this command types 
```
convert --help
```
This help will be automatically displayed!

![Alt text](images/DemoCommand.png?raw=true "Convert command example")

## But must I use the PowerCommand attribute on every command I create?
No that is not mandatory but it is recommended, note that when you declare the flags, they will be available for code completion, wich means that when the consumer types - and hit the tab button the user will can se what flags there are that could be used. That is really nice, you could read more about design of good Command Line Inter fade design here:

[10 design principles for delightful CLIs](https://blog.developer.atlassian.com/10-design-principles-for-delightful-clis/)