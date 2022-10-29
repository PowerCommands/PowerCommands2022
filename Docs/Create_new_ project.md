# Create your first PowerCommands project
1. Clone this repo
2. Open the Solution PainKiller.PowerCommands in the src\PainKiller.PowerCommands folder.
3. Make sure that one of the Console project is marked as startup project
4. Build the solution to make sure that the PowerCommandsConfig.yaml file is created in output folder
5. Start the application and run the command ```powercommand new --name nameOfYourCommandsProject --output "Path to directory"```

A new solution with all the dependent project is created for you, open the solution, set the PowerCommands Console project as startup project ant hit F5 to try it out. 

Run this command 

```demo```

And this is what you should se!

![Alt text](images/DemoCommand.png?raw=true "Demo Command")

Next step, create your first [PowerCommand](Create_new_command.md).