# Design your command

Think of your command as an one line command with some parameters in a cmd prompt environment, it should do a small amount of isolated task in a specific context, for example lets say you want to convert yaml file to json or xml. A good name could be **ConvertCommand**, as parameters you have a path to the input file, and an option flag for the format and a value to that option. That is a pretty good design for one Command. 
 
 The usage of this command will look like this if I want to convert my yaml fil to json.

```convert "C:\temp\myYaml.yaml" --format json```

If you want the input to be more self described, which the framework encourages you to do for clarity, you can choose to add a option for the filepath like this:

```convert --file "C:\temp\myYaml.yaml" --format json```
 
There are other ways you can solve the design to, you can solve it with two Commands instead, one command named **XmlCommand** and another named **JsonCommand**, it is all up to you.

## Example code
### Use case
You want a simple Command that converts a yaml file to json or xml format. Your first architecture decision is, one command or two? Well, it is a matter of taste but for this example the choice is one single command that handles the conversion to both formats. We use two [Options](Options.md), one for the filename, named path and one for the format named format. I exclude the implementation code to keep the code example short. We pretend that we have a static class handling this format conversions.

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
Have a closer look at the !PATH option, it starts with a **!** and it consists of upper letter cases only **PATH**, with the use of the **!** symbol the option must have a value. With the use of uppercase letter the option it self is **mandatory**.  

If I run the command empty I trigger a validation error.

![Alt text](images/convert_validation_error.png?raw=true "Describe convert command")

## The user input
Lets have look of how the user input is designed.

![Alt text](images/Command_line_input_convert.png?raw=true "Describe convert command")

This input will first be handled by the Core Framework, using the Identifier an instance of the ConvertCommand will be created and the framework will also add the [Input](Input.md) instance to the ConvertCommand instance which give your convert command two options named **path** and **format** to handle programmatically to create a file on the given path with the given format.

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
The user can use the tab to cycle trough the different formats.

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
**Please Note** that the name of the arguments in the design attribute is not important in code, it is useful thou when help about the command is displayed. Suggestions is what it sounds like only suggestions to guide the user to the right input.

## My final design with file/dir code completion
If you are design a command that will handle files or directories you may want to help the user with code completion, you can implement this easily, just inherit the CdCommand and then your Command will support just that!
The Command below wil handle navigation trough files and directories (in current working folder) just using tab. PowerCommand has `cd` command and `dir` commands out of the box.

So here is my final design for the ConvertCommand class where I have implemented the conversion code also so this example is fully working, I have done some rearrangement of the parameters.
I want the target file to be the first thing you input so that the user can use the code completion functionality, for the format I use a option flag --format where the user values can be json or xml. This example is simplified and only support the json format, but you get the idea I hope.
```
using System.Text.Json;
using PainKiller.PowerCommands.Core.Commands;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(  description: "Converting yaml format to json or xml format",
    arguments: "<filename>",
      options: "format",
  suggestions: "xml|json",
      example: "//Convert to json format|convert \"c:\\temp\\test.yaml\" --format json|//Convert to xml format|convert \"c:\\temp\\test.yaml\" --format xml")]
public class ConvertCommand(string identifier, PowerCommandsConfiguration configuration) : CdCommand(identifier, configuration)
{
    public override RunResult Run()
    {
        var yamlInput = File.ReadAllText(Input.SingleArgument);
        var format = GetOptionValue("format");
        if (format == "json")
        {
            var jsonOutput = ConvertYamlToJson(yamlInput);
            WriteLine(jsonOutput);
        }
        return Ok();
    }
    public static string ConvertYamlToJson(string yamlInput)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var yamlObject = deserializer.Deserialize<object>(yamlInput);

        var json = JsonSerializer.Serialize(yamlObject, new JsonSerializerOptions { WriteIndented = true });
        return json;
    }
}
```
![Alt text](images/convert_sample.png?raw=true "Describe convert command")

### User wants to write the converted output to file
You could add this to the ConvertCommand class but... you do not have to do that, PowerCommands support to call a second command on the same commandline and that second command can handle the output from the "calling" command, so if you want the output that is displayed in the console by the ConvertCommand to be written to file you could write this in the console and use the existing `file` command, you need to specify a target file to the `file` command with the option flag `--target` like this:

```convert PowerCommandsConfiguration.yaml --format json | file --target "PowerCommandsConfiguration.json"```

This will write a file named **PowerCommandsConfiguration.json** in the current working directory.


## Must I use the PowerCommandDesign attribute on every command I create?
No that is not mandatory but it is recommended, note that when you declare the [Options](Options.md), they will be available for code completion, which means that when the consumer types - and hit the tab button the user will can se what options there are that could be used, with a simple ! character you tell that the argument, quote, option or secret is required and then the Core runtime will validate that automatically for you.

Read more about CLI design here: [10 design principles for delightful CLIs](https://blog.developer.atlassian.com/10-design-principles-for-delightful-clis/)

Next step is to understand the [Power Commands Design attribute](PowerCommandDesignAttribute.md)


Read more about:

[Chain command execution)](ChainCommands.md)

[Read and write files with FileCommand](ReadWriteFileHandler.md)

[Input](Input.md)

[Options](Options.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)