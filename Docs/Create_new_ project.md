# Create your first PowerCommands project

Download the Visual Studio Template zipfile from [here](https://github.com/PowerCommands/PowerCommands2022/tree/main/Templates)

Copy the .zip file into the user project template directory. By default, this directory is %USERPROFILE%\Documents\Visual Studio %version%\Templates\ProjectTemplates.

Open Visual Studio and write Power in the searchbox, you should find the PowerCommand template.

![Alt text](images/vs_new_solution.png?raw=true "Demo Command")

A now a new solution with all the dependent project is created for you, open the solution, set the PowerCommands Console project as startup project ant hit F5 to try it out. 

Run this command 

```demo```

And this is what you should se!

![Alt text](images/DemoCommand.png?raw=true "Demo Command")

Do not worry about all the drama that an empty demo input creates, the **demo** command demonstrates how you can use options to design your command and achive validation without the need for you to add validation code.
The demo command has an design attribute that looks something like this:

```
[PowerCommandDesign( description: "Demo command just to try out how you could use the input...",
                         options: "!MANDATORY|!pause",
                         example: "//Must provide the MANDATORY option will trigger a validation error otherwise|demo MANDATORY|//Test the pause service|demo --pause 5 MANDATORY")]
```

Read more about designing your commands in the following steps.

Next step, create your first [PowerCommand](Create_new_command.md).

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)