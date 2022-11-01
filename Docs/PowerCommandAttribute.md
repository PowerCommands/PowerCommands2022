# PowerCommands Attribute
## Always describe your Commands
Be kind to your consumer as it often turn out to be you that are the consumer, fill in a shourt description of what the PowerCommand is doing and how to use it.
This is easy with the usage of the PowerCommandsAttribute, just apply them on the top of your class definition.

Now lets take a practical example...
## Use case
You want to create a simple Command that converts a yaml file to json or xml format. Your choose to solve this with one Command that handles both this formats, you will use flags. You can read more about how the class will look like here: [Design your command](Design_command.md)

This is how the PowerCommandAttribute look like, to help the user consume this command.

![Alt text](images/power_command_attribute.png?raw=true "Attribute")

When the user running this applikcation and wants to now more about the Convert command, he or she just types either:

```describe convert```

Or use the help flag
```convert --help```

And this help will be displayed:

![Alt text](images/help_convert_command.png?raw=true "Describe convert command")

### A clooser look on the syntax
```
[PowerCommand(  description: "Converting yaml format to json or xml format",
                flags: "path|format",
                example: "//Convert to json format|convert --path \"c:\\temp\\test.yaml\" --format json|//Convert to xml format|convert --path \"c:\\temp\\test.yaml\" --format xml")]
```
The user can use this command like this (note that argument and quotes are not described in the attribute beacause this command do not use them):

![Alt text](images/Command_line_input_described.png?raw=true "Describe convert command")

## Properties described
### arguments
Describe your arguments, separate them with | use // to display a commented line over the actual argument.
### argumentMandatory
Gives the user a hint that at least one argument is mandatory, it is only to help the user, the validation of the input is up to the command programmer to implement.
### quotes
Describe your quotes, separate them with | use // to display a commented line over the actual quote.
### quoteMandatory
Gives the user a hint that at least one quote is mandatory, it is only to help the user, the validation of the input is up to the command programmer to implement.
### suggestion
Suggestion will be added to the autocomplete functionallity so the user could use tab to cycle trough the commands and commands with a suggested argument.
### flags
Describe your flags, separate them with | use // to display a commented line over the actual flag, all flags that are described will also be used by the autocomplete functionallity to help the user find the right flag.
Read more about [Flags](Flags.md).
### example
Describe examples on how to use the command, separate them with | use // to display a commented line over the actual example.


### Separator and comments
Flags, quotes, arguments and example all have a property on the PowerCommandAttribute, each item are separeted with | and if you want to put a comment row before your item add this with the prefix //, if you look at the example it is used to describe two examples of usage.

example: "**//Convert to json format**|convert --path \"c:\\temp\\test.yaml\" --format json|**//Convert to xml format**|convert --path \"c:\\temp\\test.yaml\" 

This will hopefully give you more understanding on how to describe your command and design them.

Read more about:

[Design your Command](Design_command.md)

[Input](Input.md)

[Flags](Flags.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)