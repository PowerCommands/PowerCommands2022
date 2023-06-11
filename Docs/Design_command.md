# Design your command

Think of your command as an one line command with some parameters in a cmd prompt environment, it should do a small amount of isolated task in a specific context, for example lets say you want to convert yaml file to json or xml. A good name could be **ConvertCommand**, as parameters you have a path to the input file, and an option flag for the format and a value to that option. That is a pretty good design for one Command. 
 
 The usage of this command will look like this if I want to convert my yaml fil to json.

```convert "C:\temp\myYaml.yaml" --format json```

If you want the input to be more self described, which the framework encourages you to do for clarity, you can choose to add a option for the filepath like this:

```convert --file "C:\temp\myYaml.yaml" --format json```
 
There are other ways you can solve the design to, you can solve it with two Commands instead, one command named **XmlCommand** and another named **JsonCommand**, it is all up to you.

## Example code
### Use case
You want a simple Command that converts a yaml file to json or xml format. Your first architecture descision is, one command or two? Well, it is a matter of taste but for this example the choise is one single command that handles the conversion to both formats. We use two [Options](Options.md), one for the filename, named path and one for the format named format. I exclude the implementation code to keep the code example short. We pretend that we have a static class handling this format conversions.

```
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(  description: "Converting yaml format to json or xml format",
                          options: "path|format",
                          example: "//Convert to json format|convert --path \"c:\\temp\\test.yaml\" --format json|//Convert to xml format|convert --path \"c:\\temp\\test.yaml\" --format xml")]
public class ConvertCommand : CommandBase<PowerCommandsConfiguration>
{
    public ConvertCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        YamlFormatManger.Convert(Input.GetOptionValue("path"), Input.GetOptionValue("format"));
        return Ok();
    }
}
```
## Validation of mandatory option and value
Notice that the documentation for how this command will be used is created with the [PowerCommandDesign](PowerCommandDesignAttribute.md) attribute. I realize that the path option should be both **mandatory** and must have a value, I can of course add code to do this check in the Command but I do not need to do that, instead I declare that with the [PowerCommandDesign](PowerCommandDesignAttribute.md) attribute, I just add a **!** before the path option, this will be validated before the execution of the run command. I also spell the option name in UPPERCASE letters which does the option **mandatory**.

The new design look like this.
```
[PowerCommandDesign(  description: "Converting yaml format to json or xml format",
                          options: "!PATH|format",
                          example: "//Convert to json format|convert --path \"c:\\temp\\test.yaml\" --format json|//Convert to xml format|convert --path \"c:\\temp\\test.yaml\" --format xml")]
```                     
Have a closer look at the !PATH option, it starts with a **!** and it consists of upperletter cases only **PATH**, with the use of the **!** sympol the option must have a value. With the use of uppercase letter the option it self is **mandatory**.  

If I run the command empty I trigger a validation error.

![Alt text](images/convert_validation_error.png?raw=true "Describe convert command")

## The user input
Lets have look of how the user input is designed.

![Alt text](images/Command_line_input_convert.png?raw=true "Describe convert command")

This input will first be handled by the Core Framework, using the Identifier an instanse of the ConvertCommand will be created and the framework will also add the [Input](Input.md) instance to the ConvertCommand instans wich give your convert command two options named **path** and **format** to handle progamatically to create a file on the given path with the given format.

# An alternative design using suggestions
Instead of using the option **format** in the previous example you could use argument **xml** and **json** instead, there is nothing wrong or right here, it is up to you to choose what you think is the best approach.
Using argument and suggestion your design attribute could look like this.

```
[PowerCommandDesign(  description: "Converting yaml format to json or xml format",
                        arguments: "format",
                          options: "!PATH",
                      suggestions: "xml|json",
                          example: "//Convert to json format|convert json --path \"c:\\temp\\test.yaml\"|//Convert to xml format|convert xml --path \"c:\\temp\\test.yaml\"")]
```
The user can hus the tab to cycle trough the different formats.

The whole code example below:
```
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(  description: "Converting yaml format to json or xml format",
                        arguments: "format",
                          options: "!PATH",
                      suggestions: "xml|json",
                          example: "//Convert to json format|convert json --path \"c:\\temp\\test.yaml\"|//Convert to xml format|convert xml --path \"c:\\temp\\test.yaml\"")]
public class ConvertCommand : CommandBase<PowerCommandsConfiguration>
{
    public ConvertCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        YamlFormatManger.Convert(Input.GetOptionValue("path"), Input.SingleArgument);
        return Ok();
    }
}
```

## Must I use the PowerCommandDesign attribute on every command I create?
No that is not mandatory but it is recommended, note that when you declare the [Options](Options.md), they will be available for code completion, wich means that when the consumer types - and hit the tab button the user will can se what options there are that could be used, with a simple ! character you tell that the argument, quote, option or secret is required and then the Core runtime will validate that automatically for you. That is really nice, you could read more about design of good Command Line Inter fade design here:

Next step is to understand the [Power Commands Design attribute](PowerCommandDesignAttribute.md)

[10 design principles for delightful CLIs](https://blog.developer.atlassian.com/10-design-principles-for-delightful-clis/)

Read more about:


[Input](Input.md)

[Options](Options.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)