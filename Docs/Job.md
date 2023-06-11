# Run your command as a job
A PowerCommands application can be started with arguments from the commandline which is usefull when you want to use your PowerCommands implementation to perform something as a automated job.

The code below shos a dummie example of how you design your Command to run as an automatated task, notice the **return Quit()** at the end of the Run() method, that will trigger the program to quit the application. 

![Alt text](images/job.png?raw=true "job")

You just startup your PowerCommands application from with the parameter "job", like this:
```
"powercommands.exe job"
```
Your PowerCommands application may of course have so other name and your command could have Input parameters that will be passed on from the command line like this:
```
"powercommands.exe job argument1"
```

Read more about:

[Design your Command](Design_command.md)

[Options](Options.md)

[PowerCommands Attribute](PowerCommandAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)
