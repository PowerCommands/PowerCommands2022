# POWERCOMMANDS ARCHITECTURE AND DESIGN PRINCIPLES
# Components
## Main components
 - PowerCommand Client
 - Bootstrap component 
 - Third party component
 - Core component
 - Custom components 
 - PowerCommands

 ![Alt text](PowerCommands-Overview.png?raw=true "Component Diagram")

 # CREATE YOUR OWN POWER COMMANDS IMPLEMENTATION

 ## PowerCommand Console
 ### Keep the Console appliakation as clean as possible
 The console application should be used as is so that the look and feel of Power Commands is consistent, it is nicer when using a combination of PowerCommands grouped togher in one directory sharing the same configuration where you could change font color and background color and other core behaivours. Be restrictive in implementing your own custom code here, use the Bootstrap project instead.

 The default PowerCommand Console contains two lines only.
 
 ```
 Console.WriteLine("Power Commands 1.0");
 PainKiller.PowerCommands.Bootstrap.Startup.ConfigureServices().Run(args);
 ```
 
 Simple guidline is, don´t do anything except maybe change the title, it could be practical, over time you could have many different implementations.

## Bootstrap component
The bootstrap component is the glue between the Console and the other modules, it has a Startup class whos purpose is to initialize the application and customize its behavoiur if you want to. The main class to edit is in that case the PowerCommandsManager class.

## Your custom commands project(s)
The whole purpose of PowerCommands framework is so that you can write your own commands, and every implementation must therefore have at least one project containing this commands. This project also needs a couple of mandatory classes, simply copy the classes from the "MyExampleCommands" project in this github repository.
[My Example Commands](tree/main/src/PainKiller.PowerCommands/PainKiller.PowerCommands.MyExampleCommands)
The files needed is:
 - PowerCommandServices   
 - PowerCommandsConfiguration

 Not needed but needed at runtime is the configuration file PowerCommandsConfiguration.yaml, and your custom commands project is the most suitable project for this file.

 Each file explained:
 ### PowerCommandServices
 The main service class that contains all the main services for the framework, this services are.
 - ExtendedConfiguration
 - Diagnostic
 - Runtime
 - Logger
 - ReadLineService

 So if you want to swap a service for something else, for example the Logger component, here is the place to do that.

### PowerCommandsConfiguration
This class purpose is to maket it easy for you to extend the configuration with your own elements, if you do not need that, just leave the class empty.

```
public class PowerCommandsConfiguration : CommandsConfiguration
{
    //Here is the placeholder for your custom configuration, you need to add the change to the PowerCommandsConfiguration.yaml file as well
}
```
If you want to extend the configuration, it could look something like this: (code from the Example project)
```
public class PowerCommandsConfiguration : CommandsConfiguration
{
    //Here is the placeholder for your custom configuration, you need to add the change to the PowerCommandsConfiguration.yaml file as well
    public string DefaultGitRepositoryPath { get; set; } = "C:\\repos";
    public FavoriteConfiguration[] Favorites { get; set; } = new[] {new FavoriteConfiguration {Name = "Music", NameOfExecutable = "spotify"}, new FavoriteConfiguration { Name = "Games", NameOfExecutable = "steam" } };
}
```
The configuration element class FavoriteConfiguration  must of course be added, place the file in a directory named Configuration. (just a naming convetion guidline)

## PowerCommandsConfiguration.yaml
You should have this file in one of the projects and this project is most suitable for that, the configuration file is needed at runtime by the Console application. If you customize the PowerCommandsConfiguration class you also need to add this in the yaml configuration file so that the application can use it. If something is wrong in the file a default instanse will be used and a default.yaml file will be created in the application root directory. You could use that to correct the problem of the yaml structure.

### More then one PowerCommand project in the same implemantation? (no problem but...)
If your PowerCommand implementation contains more then one PowerCommands project only one of them should contain this classes, it dosent matter wich one.
 
## Extend PowerCommand
### Avoid changes in the PowerCommand Core, extend instead
 PowerCommand Framework is distributed as a Nuget Package and as open source code at Github, if you use the source code, avoid to change anything extend the functionallity instead if you feel that you need to do that. That way it is easier for you when something has changed in the Core.

# REFLECTION
## Naming conventions
PowerCommands rely on reflection to find all existing PowerCommands that are valid runnable commands, to make this work some naming convention rules are used:
 - A valid PowerCommand class name should end with "Command", for example RenameCommand or SendMailCommand otherwise they will be ignored by the runtime and could not be used.
 - Some base PowerCommands exists in Core, their name ends with "Base" and are defined as abstract and intended to be inherited by other PowerCommands classes.
### Reserved Command names
As the name of the PowerCommand class is used as an identifier, their name must be uniquee, this are the core Commands that you should not use in your custom PowerCommands project
 - ConfigurationCommand
 - ExitCommand

 If you have ShowDiagnostic enabled all defined PowerCommands will be shown on program start.

 # NAMING CONVENTIONS
 ## Directories
 Guidlines for naming directories
 - **Contracts** contains interfaces 
 - **Managers** contains business logic classes with the postfix Manager, for exemple DiagnosticManager, ReflectionManager
 - **Services** contains static och or Singleton classes acting as services to Managers or Commands
 - **Enums** contain Enums
 - **BaseClasses** contains base classes
 - **Commands** contain valid PowerCommand classes
 - **DomainObjects** contain simple classes for data transfer (POCO, DTO, Record), could contain subdirectories for different domains
 - **Extensions** contains extensions classes
 - **Events** contain custom events
 - **Exceptions** contais custom exceptions  

# SECURITY
## Always encrypt secrets 
### Use EncryptManager
 PowerCommands has encryption built in, see patterns in the Developer Guidelines documentation
### Store secrets outside the application path
 PowerCommands handles export and imports of environment variables using YAML files it is the preferred way to store secrets. Dont store sensitive information inside the application path as it will be to easy to steal with a simple copy and paste operation.

# EXCEPTIONS HANDLING, DIAGNOSTIC AND LOGGING
## Reduce coad bloat by avoiding try and catch
No need for try catch in PowerCommands Run method as the call already is encapsulated in a try catch block, to reduce coad bload let custom code just crasch and handle that by the PowerCommands runtime, it will be logged, it will be presented for the user in a generic way that not reveal sensitive informaiton that could be the case if you just use Console.WriteLine(ex.Message).
## Reduce coad bloat by avoiding logging
The runtime always logg information about the input and output from a PowerCommand execution, if you want to pass information from the PowerCommand to the log, you could use Output to to that.
## Use Diagnostic to display information to user
The Diagnostic service has the purpose to print out messages so use that rather then use Console.WriteLine, that way you can controll just how this messages should be handled. You could easaly swap or extend the standard Diagnostic service component. The Diagnostic service also has the funtionallity to time a method call and display it to the user.

# CONFIGURATION
## Use YAML
 The PowerCommands.Configuration component has generic support for reading and writing object as YAML and that format should always be used for configuration files. 
## Configuration could be shared by clients
The main configuration should be named PowerCommandsConfiguration.yaml and be a valid YAML file, it could be shared by many clients residing in the same directory.
## Location of configuration files
The core configuration file PowerCommandsConfiguration.yaml should be located in the Bootstrap project, custom configuration files is placed in the PowerCommandsClient project to make the disctinction between them clearer.

# Documentation
## Use Markdown format
 The format of dokumentation in text should use the markdown format.
## Always describe your PowerCommands
 Be kind to your consumer as it often turn out to be you that are the consumer, fill in a shourt description of what the PowerCommand is doing and how to use it.
## Use Desription attribute
