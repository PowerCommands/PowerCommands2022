# POWERCOMMANDS DESIGN PRINCIPLES AND GUIDLINES

- [POWERCOMMANDS DESIGN PRINCIPLES AND GUIDLINES](#powercommands-design-principles-and-guidlines)
- [Design principles](#design-principles)
- [Components](#components)
  - [Main components](#main-components)
- [CREATE YOUR OWN POWER COMMANDS IMPLEMENTATION](#create-your-own-power-commands-implementation)
  - [PowerCommand Console](#powercommand-console)
    - [Keep the Console appliakation as clean as possible](#keep-the-console-appliakation-as-clean-as-possible)
  - [Bootstrap component](#bootstrap-component)
  - [Your custom commands project(s)](#your-custom-commands-projects)
    - [PowerCommandServices](#powercommandservices)
    - [PowerCommandsConfiguration](#powercommandsconfiguration)
  - [PowerCommandsConfiguration.yaml](#powercommandsconfigurationyaml)
    - [More then one PowerCommand project in the same implemantation? (no problem but...)](#more-then-one-powercommand-project-in-the-same-implemantation-no-problem-but)
  - [Create your first Hello World Command](#create-your-first-hello-world-command)
    - [Clone this repo and copy this from the src folder](#clone-this-repo-and-copy-this-from-the-src-folder)
    - [Create a new solution and add this as existing projects, add a new .NET class project, name it to what you want.](#create-a-new-solution-and-add-this-as-existing-projects-add-a-new-net-class-project-name-it-to-what-you-want)
    - [Add this project references for the Bootstrap project file.](#add-this-project-references-for-the-bootstrap-project-file)
    - [Add a referance for the PowerCommandConsole project to the Bootstrap project](#add-a-referance-for-the-powercommandconsole-project-to-the-bootstrap-project)
  - [Extend PowerCommand](#extend-powercommand)
    - [Avoid changes in the PowerCommand Core, extend instead](#avoid-changes-in-the-powercommand-core-extend-instead)
- [NAMING CONVENTIONS](#naming-conventions)
  - [Reflection](#reflection)
  - [Reserved Command names](#reserved-command-names)
  - [Directories](#directories)
- [SECURITY](#security)
  - [Store secrets outside the application path](#store-secrets-outside-the-application-path)
  - [Use the secrets built in functionallity](#use-the-secrets-built-in-functionallity)
- [LOGGING](#logging)
  - [Reduce coad bloat by avoiding try and catch](#reduce-coad-bloat-by-avoiding-try-and-catch)
  - [Reduce coad bloat by avoiding logging](#reduce-coad-bloat-by-avoiding-logging)
  - [Use CommandBase Write methods](#use-commandbase-write-methods)
  - [But? I really want to write to the log directly in my Command class!](#but-i-really-want-to-write-to-the-log-directly-in-my-command-class)
- [CONFIGURATION](#configuration)
  - [Use YAML](#use-yaml)
  - [Configuration could be shared by clients](#configuration-could-be-shared-by-clients)
- [Documentation](#documentation)
  - [Use Markdown format](#use-markdown-format)
  - [Always describe your PowerCommands](#always-describe-your-powercommands)
  - [Use Tags and PowerCommand attributes](#use-tags-and-powercommand-attributes)

# Design principles
 The design principles for this project is to keep the Core lightweight and simple. The Core components is components that you probably or very often will use, while Custom Components do not have that characteristic feature. Another restriction for the Core components is that they should avoid to add any third party dependancies, that way you know exactly what code you are running.

The core components should be reliable and robust and do not change much over time. That statement applies from the day when the project has reached 1.0 status, the current staus I would say is 0.5, hopefully the core part of this project reach level 1.0 before the end of this year (2022). 
 
 Custom component are aloud to break that rule, custom components should on the other hand avoid to have depandencies to the Core components, they should be design to work as stand-alone components.

# Components
## Main components
 - PowerCommand Client
 - Bootstrap component 
 - Third party component
 - Core components
 - Custom components 
 - PowerCommands

 ![Alt text](PowerCommands-Overview.png?raw=true "Component Diagram")

 # CREATE YOUR OWN POWER COMMANDS IMPLEMENTATION

 Before we dig ourselves in to details about how we create a PowerCommands Implementation, a short breafing about the basics is in place...

 The items in the purple boxes is the most interesting part when you are setting upp a new implementation.
 
 ## PowerCommand Console
 ### Keep the Console appliakation as clean as possible
 The console application should be used as is so that the look and feel of Power Commands is consistent, be restrictive in implementing your own custom code here, use the Bootstrap or the Command project(s) instead.

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
[My Example Commands](https://github.com/PowerCommands/PowerCommands2022/tree/main/src/PainKiller.PowerCommands/PainKiller.PowerCommands.MyExampleCommands)
The files needed is:
 - PowerCommandServices   
 - PowerCommandsConfiguration
 - PowerCommandsConfiguration.yaml*

 *Needed at runtime is the configuration file PowerCommandsConfiguration.yaml, and your custom commands project is the most suitable project for this file.

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
You should have this file in one of the projects and the Commands or the Console project is suitable for that, the configuration file is needed at runtime by the Console application. If you customize the PowerCommandsConfiguration class you also need to add this in the yaml configuration file so that the application can use it, same is also true when removing stuff, they should be in sync with each other. 

### More then one PowerCommand project in the same implemantation? (no problem but...)
If your PowerCommand implementation contains more then one PowerCommands project only one of them should contain this classes, it dosent matter wich one.

 ## Create your first Hello World Command
 ### Clone this repo and copy this from the src folder
 - PainKiller.PowerCommands.Bootstrap **project**
 - PainKiller.PowerCommands.PowerCommandsConsole **project**
 - Core **Directory**

### Create a new solution and add this as existing projects, add a new .NET class project, name it to what you want.
Add this files and directories (with content) from the **PainKiller.PowerCommands.MyExampleCommands** project
- PowerCommandServices.cs **file**
- PowerCommandsConfiguration.yaml **file**
- Configuration **Directory**

Take note that the PowerCommandsConfiguration.cs class contains some custom configuration belonging to an example, you could delete all that (leave the class empty) and the **FavoriteConfiguration.cs** class, you should also remove the corresponding configuration in the yaml file.

**Heads up!** If something is wrong in the file a default instanse will be used and a default.yaml file will be created in the application root directory. You could use that to correct the problem of the yaml structure.

Safest in the beginning is to just let it be and things will run smootly, when you are feeling brave later on, then you could start removing unnecessary things.

### Add this project references for the Bootstrap project file.
Adjust the paths if you are using at diffrent structure of your solution. (If so maybe more practical to include them using the Visual Studio IDE)
```
<ItemGroup>
    <ProjectReference Include="..\Core\PainKiller.PowerCommands.Configuration\PainKiller.PowerCommands.Configuration.csproj" />
    <ProjectReference Include="..\Core\PainKiller.PowerCommands.Core\PainKiller.PowerCommands.Core.csproj" />
    <ProjectReference Include="..\Core\PainKiller.PowerCommands.ReadLine\PainKiller.PowerCommands.ReadLine.csproj" />
    <ProjectReference Include="..\Core\PainKiller.PowerCommands.Security\PainKiller.PowerCommands.Security.csproj" />
    <ProjectReference Include="..\Core\PainKiller.PowerCommands.Shared\PainKiller.PowerCommands.Shared.csproj" />        
  </ItemGroup>
  ```
**Heads up!** A reference to your newly created class project is also needed in the Bootstrap project, it is not included above.
### Add a referance for the PowerCommandConsole project to the Bootstrap project

This should be enough, I think you could solve any problem that I have missed here, it is all about setting the referenses correctly for the Bootstrap project, the newly created commands project and the Console project.

Now you are really to create your very first Command!

 This the typical hello world example.
```
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;
public class ExampleCommand : CommandBase<PowerCommandsConfiguration>
{
    public ExampleCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration)
    {

    }

    public override RunResult Run(CommandLineInput input)
    {
        WriteLine("Hello world");
        return CreateRunResult(input);
    }
}
```
This is the bare minimum that you need, to learn more about what you could to, look at sample commands in the Examaples project in this github repositorys.
## Extend PowerCommand
### Avoid changes in the PowerCommand Core, extend instead
 PowerCommand Framework is distributed as open source code on Github, if you use the source code, avoid to change anything extend the functionallity instead if you feel that you need to do that. That way it is easier for you when something has changed in the Core.

# NAMING CONVENTIONS
## Reflection
PowerCommands rely on reflection to find all existing PowerCommands that are valid runnable commands, to make this work some naming convention rules are used:
 - **A valid PowerCommand class name should end with "Command"**
 
   For example RenameCommand or SendMailCommand otherwise they will be ignored by the runtime and could not be used.
 - **Base Commands**
 
   Some base PowerCommands exists in Core, their name ends with "Base" and are defined as abstract and intended to be inherited by other PowerCommands classes, use the same pattern if you add commands with the purpose of acting as base classes.

 ## Reserved Command names
 As the name of the PowerCommand class is used as an identifier, their name must be uniquee, this are the core Commands that you should not use in your custom PowerCommands project, at the moment the reserved commands names are:
 - CommandsCommand
 - ClsCommand
 - LogCommand
 - SecretCommand
 - ExitCommand
 - DescribeCommand

 This could of cours change in the future and the documentation may not be updated, you could easliy check the reserved commands using this command in the Console:

 ```
 commands ! 
 ``` 
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
## Store secrets outside the application path
 PowerCommands handles export and imports of environment variables using YAML files it is the preferred way to store secrets. Do not store sensitive information inside the application path as it will be to easy to steal with a simple copy and paste operation, sensitive information should always be encrypted at rest.

## Use the secrets built in functionallity 
Create secret with built in commands named secret, like this:
```
secret create "localDB"
```
This will create a secret item in configuration, and a encryptet secret will be stored in a EnvironmentVariable with the proviede name.
In you command you could get the decrypted secret like this.
```
var cn = Configuration.Secret.DecryptSecret("Server=.;Database=timelineLocalDB;User Id=sa;Password=##localDB##;"); 
```
# LOGGING
## Reduce coad bloat by avoiding try and catch
No need for try catch in PowerCommands Run method as the call already is encapsulated in a try catch block, to reduce coad bload let custom code just crasch and handle that by the PowerCommands runtime, it will be logged, it will be presented for the user in a generic way that not reveal sensitive informaiton that could be the case if you just use Console.WriteLine(ex.Message).
## Reduce coad bloat by avoiding logging
The runtime always logg information about the input and output from a PowerCommand execution, if you want to pass information from the PowerCommand to the log, you could use Output to to that.
Every time you use a Write method in the Commands class, this will be added to the Output and logged as soon as the method is finished. 
```
var name = $"{input.SingleArgument} {input.SingleQuote}";
WriteLine($"Hello {name}");
```
This will add a row to the log with the output from the WriteLine call automatically.
## Use CommandBase Write methods
The CommandBase class has a couple of help methods to output data to the console, use that rather then Console.WriteLine. The base class Write methods will automatically return all the output during a Run method execution, and this output will be logged by the logger component.
```
WriteHeadLine("Header", addToOutput: true);
WriteLine("Write line, also ends up in the log (default behaivour)");
WriteProcessLog("Write with WriteProcessLog adds a tag do log","process1");
//You could also use extensions methods
this.WriteObjectDescription(name:"Megabytes", description:"A numeric value");
```
## But? I really want to write to the log directly in my Command class!
It is something that is not encouraged, quite the opposite but in some cases you may need to do that, and you can (using an extension method).
```
IPowerCommandServices.DefaultInstance?.Logger.LogInformation($"Log this information please");
```
# CONFIGURATION
## Use YAML
 The PowerCommands.Configuration component has generic support for reading and writing object as YAML and that format should always be used for configuration files. 
## Configuration could be shared by clients
The main configuration should be named PowerCommandsConfiguration.yaml and be a valid YAML file, it could be shared by many clients residing in the same directory.
In such cases they should support the identical configuration structure otherwise there could be trouble, most propably some Console client won´t start.

# Documentation
## Use Markdown format
 The format of dokumentation in text should use the markdown format.
## Always describe your PowerCommands
 Be kind to your consumer as it often turn out to be you that are the consumer, fill in a shourt description of what the PowerCommand is doing and how to use it.
## Use Tags and PowerCommand attributes
This is an example where every property of the attributes is used

![Alt text](attributes.png?raw=true "Attributes")
Attributes is used to show a nice description of the command with the built in describe command.

```
describe cls
```
![Alt text](describe.png?raw=true "Describe")

Tags attribute is used to make searches with the commands command.

```
commands tag "help"
```
![Alt text](tags.png?raw=true "Describe")

