# Display output to the console
## ConsoleService
The output to the Console should almout always use the ConsoleService to print output out to the console, this could be done in diffrent ways.
### Using the static service directly
![Alt text](images/ConsoleService.png?raw=true "Console Service")
### Using the Write helper methods in the [CommandBase](CommandBase.md) class
 - WriteCritical
 - WriteWarnig
 - WriteError
 - WriteLine
 - and so on...

## Why?
The output will be displayed in a decided way, warnings wil have a certain color, diffrent from errors, and the output will be logged by default. And they will have log level corresponding to the type of output. WriteLine and Write have the options to be printed out to the console without being logged.

### Exceptions
There are situations where Console.Write or WriteLine is usefull, if you want to be sure that it will not be logged by misstake, the output of secrets for instance.

Read more about:

[Design your Command](Design_command.md)

[CommandBase the standard Command base class](CommandBase.md)