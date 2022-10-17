# PowerCommands2022
PowerCommands is a concept for creating your own customized command prompt to perform simple or advanced task with the full control from your command environment. That means no time-consuming hassling with a GUI, concentrate on just the code. The concept relays heavily on reflection so adding your own PowerCommands is very simple and fast.

## Create your first Hello World Commandd
  - Clone this repo
  - Open the Solution PainKiller.PowerCommands in the src\PainKiller.PowerCommands folder.
  - Make sure that one of the Console project is marked as startup project, for example the **PainKiller.PowerCommands.PowerCommandsConsole** in the Solution root
  - Build the solution to make sure that the PowerCommandsConfig.yaml file is created in output folder
  - Start the application and run the command 
  ```powercommand new --name nameOfYourCommandsProject --output "Path to directory"```
  - A new solution with all the dependent project is created for you, open the solution, set the PowerCommands Console project as startup project ant hit F5 to try it out.

It's really unfortunate that you have to set up the startup project for the solution, that information is saved in solution user options (*.suo), it's a binary file that you don't want to mess with. But I guess that you already are familiar with that problem.

 ## Core
 ### The core components offering this to your custom PowerCommands
 - Logging (using Microsoft.Extensions.Logging.ILogger)
 - Process log (using the standard logger with tags to track a process)
 - Configuration with YAML (very easy to extend)
 - Diagnostic 
 - Secret handling to protect sensitive information like password or authentication tokens in the configuration file.
 - Command completion, with history, suggestions and support for Path/File navigation and color highlightning when typing a valid command
 - Progressbar
 - Download files
 - Password prompt
 - Run as job
 - Zip with attributes like checksum, filecount and file size
 
 Some the core components is separated and are stand-alone components, they could be reused else where, like security and configuration.
 
 ## Custom Components 
 - HttpClientUtils 
 - AzureKeyVault
 - GlitchFinder (by Jooachim)

 ## Third Party Components
 - Serialog

 PowerCommands uses Serialog as the default logger, it is easy to swith to another one, all logging is done using the abstraction interface Microsoft.Extensions.Logging.ILogger. The ambition is to use as few third party components as possible to avoid compabillity issues and keep the control over the code. 

 ## Design of your Commands
 Think of your command as an one line command with some parameters in a cmd prompt environment, it should do a single isolated task, for example lets say you want to convert yaml file to json or xml. A good name could be **ConvertCommand**, as parameters you have a path to the input file, and a flag for the format and a value to that flag. That is a pretty good design for one Command. 
 
 The usage of this command will look like this if I want to convert my yaml fil to json.

```convert "C:\temp\myYaml.yaml" --format json```

If you want the input to be more self described you can choose to add a flag for the filepath like this:

```convert --file "C:\temp\myYaml.yaml" --format json```
 
There are other ways you can solve the design to, you can solve it with two Commands instead, one command named **XmlCommand** and another named **JsonCommand**, it is all up to you.

You could look here for inspiration:
[10 design principles for delightful CLIs](https://blog.developer.atlassian.com/10-design-principles-for-delightful-clis/)

Read more about design principles for PowerCommands here: [Design principles](PowerCommands%20Design%20Principles%20And%20Guidlines.md)

 # Implementations
 
 ## GlitchFinder power commands using GlitchFinder component by Jooachim

 Either compare two different datasets or compare a single dataset towards itself over time.
 If there are glitches in the matrix then you get a report showing lines that are missing or different in the two datasets.

### Comparison
This is for comparing two different data sources, could be two different files, two different DB queries, one file vs one query etc.

### Regression test
This is for tracking a single dataset/source over time. You create a baseline, which is stored on file, and then later compare data towards this baseline.

  