# POWERCOMMANDS ARCHITECTURE AND DESIGN PRINCIPLES
# Components
## Main components
 - PowerCommand Client
 - Bootstrap component 
 - Third party component
 - Core component
 - Custom components
 - Reusable Commands
 - PowerCommands

 ![Alt text](PowerCommand_component_diagram.png?raw=true "Component Diagram")

 ## PowerCommand Console
 ### Keep the Console appliakation as clean as possible
 The console application should be used as is so that the look and feel of Power Commands is consistent, it is nicer when using a combination of PowerCommands grouped togher in one directory sharing the same configuration where you could change font color and background color and other core behaivours. Be restrictive in implementing your own custom code here, use the Bootstrap project instead.
 ## Bootstrap component
 The bootstrap component is the glue between the Console and the other modules, it has a Startup class whos purpose is to initialize the application, in the bootstrap component you could choose your prefered logging library, you could overide default settings and do other customization setup. In the Bootstrap project the main class for configuration is open for you to extend so that all configuration (except sensitive information) is placed in one single configuration file. It is intended to use for customization to create your PowerCommands project exactly as you want.
 
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
 Directories should always be named in a plural form, like Managers, Enums, Services, Contracts and so on...
 - **Contracts** contains interfaces
 - **Managers** contains business logic classes with the postfix Manager, for exemple DiagnosticManager, ReflectionManager
 - **Services** contains static och or Singleton classes acting as services to Managers or Commands
 - **Enums** contain Enums
 - **BaseClasses** contains base classes
 - **Commands** contain valid PowerCommand classes
 - **DomainObjects** contain simple classes for data transfer (POCO, DTO, Record)
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
