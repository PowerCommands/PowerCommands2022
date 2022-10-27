# PowerCommands Attribute
## Always describe your PowerCommands
 Be kind to your consumer as it often turn out to be you that are the consumer, fill in a shourt description of what the PowerCommand is doing and how to use it.
## <a name='UsePowerCommandattribute'></a>Use PowerCommand attribute
This is an example where every property of the attributes is used

![Alt text](../attributes.png?raw=true "Attributes")
Attributes is used to show a nice description of the command with the built in --help flag. 

```
regression --help
```
![Alt text](images/power_command_attribute.png?raw=true "Describe")

But it is just not for displaying the help, the flag property and the suggestion property of the PowerCommandAttribute controls suggestions provided by intellisense, you should really take advantage of that.

Read more about [Flags](Flags.md). 