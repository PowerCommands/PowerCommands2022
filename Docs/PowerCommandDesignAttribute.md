# PowerCommandsDesign Attribute
## Always describe your Commands
Be kind to your consumer as it often turn out to be you that are the consumer, fill in a shourt description of what the PowerCommand is doing and how to use it.
This is easy with the usage of the PowerCommandsAttribute, just apply them on the top of your class definition.

Now lets take a practical example...
## Use case
You want to create a simple Command that converts a yaml file to json or xml format. Your choose to solve this with one Command that handles both this formats, you will use options. You can read more about how the class will look like here: [Design your command](Design_command.md)

This is how the PowerCommandAttribute look like, to help the user consume this command.

![Alt text](images/power_command_attribute.png?raw=true "Attribute")

When the user running this applikcation and wants to now more about the Convert command, he or she just types either:

```describe convert```

Or use the help option
```convert --help```

And this help will be displayed:

![Alt text](images/help_convert_command.png?raw=true "Describe convert command")

### A clooser look on the syntax
```
[PowerCommandDesign(  description: "Converting yaml format to json or xml format",
                options: "!path|format",
                example: "//Convert to json format|convert --path \"c:\\temp\\test.yaml\" --format json|//Convert to xml format|convert --path \"c:\\temp\\test.yaml\" --format xml")]
```
The user can use this command like this (note that argument and quotes are not described in the attribute beacause this command do not use them):

![Alt text](images/Command_line_input_described.png?raw=true "Describe convert command")

## Mark argument, quote, option or secret as required with **!** character
If you look close at the example above you see two **options** are declared like this **!path** and **format** the **!** character before the option name tells the Core Runtime that this option requires a value and it will be validated before the command Run methods executes, if he user ommits the **path** option value this will occur:

![Alt text](images/convert_validation_error.png?raw=true "Describe convert command")

## Properties described
### arguments
Describe your arguments, separate them with | use // to display a commented line over the actual argument.
### quotes
Describe your quotes, separate them with | use // to display a commented line over the actual quote.
### suggestion
Suggestion will be added to the autocomplete functionallity so the user could use tab to cycle trough the commands and commands with a suggested argument.
### Options
Describe your options, separate them with | use // to display a commented line over the actual option, all options that are described will also be used by the autocomplete functionallity to help the user find the right option.
Read more about [options](options.md).
### secrets
Describe your secrets that the commands needs, you probably want to declare a secret as required with **!** character, in the sample above secret **cnDatabase** is marked as required.
```
[PowerCommandDesign(  description: "Database command",
                          secrets: "!cnDatabase",
                          example: "//Get customers|customer")]
```
Read more about how to manage [Secrets](Secrets.md).
### example
Describe examples on how to use the command, separate them with | use // to display a commented line over the actual example.

### Separator and comments
options, quotes, arguments and example all have a property on the PowerCommandAttribute, each item are separeted with | and if you want to put a comment row before your item add this with the prefix //, if you look at the example it is used to describe two examples of usage.

example: "**//Convert to json format**|convert --path \"c:\\temp\\test.yaml\" --format json|**//Convert to xml format**|convert --path \"c:\\temp\\test.yaml\" 

This will hopefully give you more understanding on how to describe your command and design them.

Read more about:

[Design your Command](Design_command.md)

[Input](Input.md)

[options](options.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)