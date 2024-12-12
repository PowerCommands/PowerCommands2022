<!-- vscode-markdown-toc -->
* [Main components](#Maincomponents)
* [PowerCommand Console](#PowerCommandConsolee)
	* [Keep the Console application as clean as possible](#KeeptheConsoleappliakationascleanaspossiblee)
* [Bootstrap component](#Bootstrapcomponent)
* [Your custom commands project(s)](#Yourcustomcommandsprojects)
	* [PowerCommandServices](#PowerCommandServicess)
	* [PowerCommandsConfiguration](#PowerCommandsConfiguration)
* [PowerCommandsConfiguration.yaml](#PowerCommandsConfiguration.yaml)
	* [More then one PowerCommand project in the same implementation? (no problem but...)](#MorethenonePowerCommandprojectinthesameimplemantationnoproblembut...)
* [Create your first Hello World Command](#CreateyourfirstHelloWorldCommandd)
* [Design of your Commands](#DesignofyourCommands)
* [Extend PowerCommand](#ExtendPowerCommand)
	* [Avoid changes in the PowerCommand Core, extend instead](#AvoidchangesinthePowerCommandCoreextendinstead)
	* [Write generic custom modules](#Writegenericcustommodules)
* [Reflection](#Reflection)
* [Reserved Command names](#ReservedCommandnamess)
* [Directories](#Directoriess)
* [Store secrets outside the application path](#Storesecretsoutsidetheapplicationpath)
* [Use the secrets built in functionality](#Usethesecretsbuiltinfunctionallity)
* [Be careful when decrypting, be sure to protect your secrets in runtime](#Becarefullwhendecryptingbesuretoprotectyoursecretsinruntime)
	* [Recommended pattern for custom Components using secrets, pass the DecryptSecret function](#RecomendedpatternforcustomCompenentsusingsecretspasstheDecryptSecretfunction)
* [Reduce code bloat by avoiding try and catch](#Reducecoadbloatbyavoidingtryandcatch)
* [Reduce code bloat by avoiding logging](#Reducecoadbloatbyavoidinglogging)
* [Use CommandBase Write methods](#UseCommandBaseWritemethods)
* [But? I really want to write to the log directly in my Command class!](#ButIreallywanttowritetothelogdirectlyinmyCommandclass)
* [Use YAML](#UseYAML)
* [Configuration could be shared by clients](#Configurationcouldbesharedbyclients)
* [Use Markdown format](#UseMarkdownformat)
* [Always describe your PowerCommands](#AlwaysdescribeyourPowerCommands)
* [Use PowerCommand attribute](#UsePowerCommandattribute)
* [options](#options)

<!-- vscode-markdown-toc-config
	numbering=false
	autoSave=true
	/vscode-markdown-toc-config -->
<!-- /vscode-markdown-toc -->

# POWERCOMMANDS DESIGN PRINCIPLES AND GUIDLINES
# Design principles
 The design principles for this project is to keep the Core lightweight and simple. The Core components is components that you probably or very often will use, while Custom Components do not have that characteristic feature. Another restriction for the Core components is that they should avoid to add any third party dependencies, that way you know exactly what code you are running.

The core components should be reliable and robust and do not change much over time. That statement applies from the day when the project has reached 1.0 status, the current status I would say is 0.5, hopefully the core part of this project reach level 1.0 before the end of this year (2022). 
 
 Custom component are aloud to break that rule, custom components should on the other hand avoid to have dependencies to the Core components, they should be design to work as stand-alone components.

# Components
## <a name='Maincomponents'></a>Main components
 - PowerCommand Client
 - Bootstrap component 
 - Third party component
 - Core components
 - Custom components 
 - PowerCommands

 ![Alt text](images/PowerCommands-Overview.png?raw=true "Component Diagram")

 # CREATE YOUR OWN POWER COMMANDS IMPLEMENTATION

 Before we dig ourselves in to details about how we create a PowerCommands Implementation, a short breafing about the basics is in place...

 The items in the purple boxes is the most interesting part when you are setting upp a new implementation.
 
## <a name='PowerCommandConsolee'></a>PowerCommand Consolee
### <a name='KeeptheConsoleappliakationascleanaspossiblee'></a>Keep the Console application as clean as possible
 The console application should be used as is so that the look and feel of Power Commands is consistent, be restrictive in implementing your own custom code here, use the Bootstrap or the Command project(s) instead.

 The default PowerCommand Console contains two lines only.
 
 ```
 Console.WriteLine("Power Commands 1.0");
 PainKiller.PowerCommands.Bootstrap.Startup.ConfigureServices().Run(args);
 ```
 
 Simple guidline is, don´t do anything except maybe change the title, it could be practical, over time you could have many different implementations.

## <a name='Bootstrapcomponent'></a>Bootstrap component
The bootstrap component is the glue between the Console and the other modules, it has a Startup class whos purpose is to initialize the application and customize its behavior if you want to. The main class to edit is in that case the PowerCommandsManager class.

## <a name='Yourcustomcommandsprojects'></a>Your custom commands project(s)
The whole purpose of PowerCommands framework is so that you can write your own commands, and every implementation must therefore have at least one project containing this commands. This project also needs a couple of mandatory classes, if you use the PowerCommand new a whole new solution is set upp for you, it is explained further down the road in this documentation, but you have set up everything manually. simply copy the classes from the "MyExampleCommands" project in this github repository.
[My Example Commands](https://github.com/PowerCommands/PowerCommands2022/tree/main/src/PainKiller.PowerCommands/PainKiller.PowerCommands.MyExampleCommands)
The files needed is:
 - PowerCommandServices   
 - PowerCommandsConfiguration
 - PowerCommandsConfiguration.yaml*

 *Needed at runtime is the configuration file PowerCommandsConfiguration.yaml, and your custom commands project is the most suitable project for this file.

 Each file explained:
### <a name='PowerCommandServicess'></a>PowerCommandServicess
 The main service class that contains all the main services for the framework, this services are.
 - ExtendedConfiguration
 - Diagnostic
 - Runtime
 - Logger
 - ReadLineService

 So if you want to swap a service for something else, for example the Logger component, here is the place to do that.

### <a name='PowerCommandsConfiguration'></a>PowerCommandsConfiguration
This class purpose is to make it easy for you to extend the configuration with your own elements, if you do not need that, just leave the class empty.

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
The configuration element class FavoriteConfiguration  must of course be added, place the file in a directory named Configuration. (just a naming convection guideline)

## <a name='PowerCommandsConfiguration.yaml'></a>PowerCommandsConfiguration.yaml
You should have this file in one of the projects and the Commands or the Console project is suitable for that, the configuration file is needed at runtime by the Console application. If you customize the PowerCommandsConfiguration class you also need to add this in the yaml configuration file so that the application can use it, same is also true when removing stuff, they should be in sync with each other.

A default base configuration file looks like this, format is YAML:
```
version: 1.0
configuration:  
  codeEditor: C:\Users\guest\AppData\Local\Programs\Microsoft VS Code\Code.exe  
  showDiagnosticInformation: false
  metadata:
    name: Example Commands
    description: Example project
  log:
    fileName: powercommands.log
    filePath: logs
    rollingIntervall: Day
    restrictedToMinimumLevel: Information
    component: PainKiller.SerilogExtensions.dll
    checksum: 173831af7e77b8bd33e32fce0b4e646d
    name: Serialog
  components:
  - component: PainKiller.PowerCommands.Core.dll
    checksum: 30cc55b7c6f29f951be3df97f634fb71
    name: PainKiller Core
  - component: PainKiller.PowerCommands.MyExampleCommands.dll
    checksum: c8d128cde99a5ee06b56e8337c10b1e2
    name: My Example Command  
```

### <a name='MorethenonePowerCommandprojectinthesameimplemantationnoproblembut...'></a>More then one PowerCommand project in the same implementation? (no problem but...)
If your PowerCommand implementation contains more then one PowerCommands project only one of them should contain this classes, it doesn't matter which one.

## <a name='CreateyourfirstHelloWorldCommandd'></a>Create your first Hello World Command
  - Clone this repo
  - Open the Solution PainKiller.PowerCommands in the src\PainKiller.PowerCommands folder.
  - Make sure that one of the Console project is marked as startup project, for example the **PainKiller.PowerCommands.PowerCommandsConsole** in the Solution root
  - Build the solution to make sure that the PowerCommandsConfig.yaml file is created in output folder
  - Start the application and run the command 
  ``` powercommand new --name nameOfYourCommandsProject --output "Path to directory" ```
  - A new solution with all the dependent project is created for you, open the solution, set the PowerCommands Console project as startup project ant hit F5 to try it out.

It's really unfortunate that you have to set up the startup project for the solution, that information is saved in solution user options (*.suo), it's a binary file that you don't want to mess with. But I guess that you already are familiar with that problem.


Now you are ready to create your very first Command! You could use that manually or you could use PowerCommands Console it self, start the Console project from your favorite IDE and run this command:

 ```commands create Example```

 This will create a new Command in your Commands project in the folder Commands.
```
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;
public class ExampleCommand : CommandBase<PowerCommandsConfiguration>
{
    public ExampleCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        //insert your code here
        return CreateRunResult();
    }
}
```
This is the bare minimum that you need, to learn more about what you could do, look at sample commands in the Examples project in this github repository.


## Update your Core component projects in your custom PowerCommands solution
You have two options
 - You do it manually, clone this repo, delete everything i the Core directory and copy the new content from the Core directory you just downloaded.
 - You can also do it automatically with this command
 ```powercommand update``` and then reload the solution

## <a name='DesignofyourCommands'></a>Design of your Commands
 Think of your command as an one line command with some parameters in a cmd prompt environment, it should do a single isolated task, for example lets say you want to convert yaml file to json or xml. A good name could be **ConvertCommand**, as parameters you have a path to the input file, and a option for the format and a value to that option. That is a pretty good design for one Command. 
 
 The usage of this command will look like this if I want to convert my yaml fil to json.

```convert "C:\temp\myYaml.yaml" --format json```

If you want the input to be more self described, which the framework encourages you to do for clarity, you can choose to add a option for the filepath like this:

```convert --file "C:\temp\myYaml.yaml" --format json```
 
There are other ways you can solve the design to, you can solve it with two Commands instead, one command named **XmlCommand** and another named **JsonCommand**, it is all up to you.

You could look here for inspiration:
[10 design principles for delightful CLIs](https://blog.developer.atlassian.com/10-design-principles-for-delightful-clis/)

Don´t forget to describe your Commands, read more about that under [Always describe your PowerCommands](#AlwaysdescribeyourPowerCommands)

## <a name='ExtendPowerCommand'></a>Extend PowerCommand

### <a name='AvoidchangesinthePowerCommandCoreextendinstead'></a>Avoid changes in the PowerCommand Core, extend instead
 PowerCommand Framework is distributed as open source code on Github, if you use the source code, avoid to change anything extend the functionality instead if you feel that you need to do that. That way it is easier for you when something has changed in the Core.
### <a name='Writegenericcustommodules'></a>Write generic custom modules
Keep the Command classes and project lightweight, create custom modules if what you are about to solve means a lot of business and data logic, a good example is the custom command GlitchFinder in this repository. With Glitchfinder you could compare datasources with each other. The implementation is divided in two parts, in the custom module all the algorithms is placed that actually do the comparison and regression test, the Commands project is just a administrative "client" you could say, where you could organize the usage of this test and keep track of the results.

# NAMING CONVENTIONS
## <a name='Reflection'></a>Reflection
PowerCommands rely on reflection to find all existing PowerCommands that are valid runnable commands, to make this work some naming convention rules are used:
 - **A valid PowerCommand class name should end with "Command"**
 
   For example RenameCommand or SendMailCommand otherwise they will be ignored by the runtime and could not be used.
 - **Base Commands**
 
   Some base PowerCommands exists in Core, their name ends with "Base" and are defined as abstract and intended to be inherited by other PowerCommands classes, use the same pattern if you add commands with the purpose of acting as base classes.

## <a name='ReservedCommandnamess'></a>Reserved Command namess
 As the name of the PowerCommand class is used as an identifier, their name must be unique, this are the core Commands that you should not use in your custom PowerCommands project, at the moment the reserved commands names are:
 - CommandsCommand
 - PowerCommandCommand
 - LogCommand
 - SecretCommand
 - ExitCommand
 - DescribeCommand
 - NewCommand 

 This could of course change in the future and the documentation may not be updated, you could easily check the reserved commands using this command in the Console:

 ```
 commands --reserved 
 ``` 
## <a name='Directoriess'></a>Directoriess
 Guidlines for naming directories
 - **Contracts** contains interfaces 
 - **Managers** contains business logic classes with the postfix Manager, for example DiagnosticManager, ReflectionManager
 - **Services** contains static och or Singleton classes acting as services to Managers or Commands
 - **Enums** contain Enums
 - **BaseClasses** contains base classes
 - **Commands** contain valid PowerCommand classes
 - **DomainObjects** contain simple classes for data transfer (POCO, DTO, Record), could contain subdirectories for different domains
 - **Extensions** contains extensions classes
 - **Events** contain custom events
 - **Exceptions** contains custom exceptions  

# SECURITY
## <a name='Storesecretsoutsidetheapplicationpath'></a>Store secrets outside the application path
 PowerCommands handles export and imports of environment variables using YAML files it is the preferred way to store secrets. Do not store sensitive information inside the application path as it will be to easy to steal with a simple copy and paste operation, sensitive information should always be encrypted at rest.

## <a name='Usethesecretsbuiltinfunctionallity'></a>Use the secrets built in functionallity 
Secret is initialized the fist time you startup a new power command project on your machine, the same keys for encryption/decryption is used by all your PowerCommands based applications on the same machine.
If you setup PowerCommands on a different machine, a new key for encryption/decryption will be used, meaning you have to create the secrets again on that machine. But you can move the keys manually, but I do not think that you should do that.

Create secret with built in commands named secret, like this:
```
secret create "localDB"
```
This will create a secret item in configuration, and a encrypted secret will be stored in a EnvironmentVariable with the provide name.
In you command you could get the decrypted secret like this.
```
var cn = Configuration.Secret.DecryptSecret("Server=.;Database=timelineLocalDB;User Id=sa;Password=##localDB##;"); 
```

Another simple usa case where you decrypt your secret access token. Lets pretend that you have created a secret named **AWS_TOKEN**.

```
var accessToken = Configuration.Secret.DecryptSecret("##AWS_TOKEN##");
```

Sometimes you have to pass a configuration element to a thirdparty or custom module, then this could be useful, it creates a new clone of the configuration and you pass that, its a good pattern rather then pass the runtime configuration instance that could lead to unpredictable result and in worst case revealing the decrypted secret by mistake.
```
var decryptedCloneConfiguration = Configuration.Secret.DecryptSecret(config.SourceSetting, nameof(config.SourceSetting.ConnectionString));
```
You pass in the configuration and the property name that has a tagged secret and you get a clone of the configuration back where the property value decrypted. In the example above it is the **ConnectionString** property that is decrypted.


## Be careful when decrypting, be sure to protect your secrets in runtime
It is very easy to expose a decrypted value by mistake, the decryption should be in the same scope or in very near scope of the usage. It should not be passed around with the other configuration values and reside in runtime as long as the application executes. The risk is that for example the decrypted value is logged for some reason you cant predict and the secret is logged as clear text, in other words it is revealed and must be changed.

### Recommended pattern for custom Components using secrets, pass the DecryptSecret function
A pattern to reduce this risk could be to send the DecryptSecret function to the target rather then send the decrypted configuration, like this example below. In this real world use case I using a Custom Component, I want to implement the secret handling as late as possible but avoid to create a dependency between the custom component and PowerCommands. I have to modify the Custom component a bit but no dependency is needed. 
```
//First I add this to the class in the custom component that will use the connection string.
private static Func<string, string> _decryptSecretsFunc;
public static void SetDecryptSecretFunction(Func<string, string> decryptSecretsFunc) => _decryptSecretsFunc = decryptSecretsFunc;

//In the same class when the connection string needs to be decrypted the function will be invoked.
var cnString = _decryptSecretsFunc == null ? dss.ConnectionString : _decryptSecretsFunc.Invoke(dss.ConnectionString);
using (var connection = new SqlConnection(cnString))

//From the PowerCommand class you need to pass the DecryptSecret function like this
SqlImport.SetDecryptSecretFunction(Configuration.Secret.DecryptSecret);
```
# LOGGING
## <a name='Reducecoadbloatbyavoidingtryandcatch'></a>Reduce code bloat by avoiding try and catch
No need for try catch in PowerCommands Run method as the call already is encapsulated in a try catch block, to reduce code bloat let custom code just crash and handle that by the PowerCommands runtime, it will be logged, it will be presented for the user in a generic way that not reveal sensitive information that could be the case if you just use Console.WriteLine(ex.Message).
## <a name='Reducecoadbloatbyavoidinglogging'></a>Reduce code bloat by avoiding logging
The runtime always log information about the input and output from a PowerCommand execution, if you want to pass information from the PowerCommand to the log, you could use Output to to that.
Every time you use a Write method in the Commands class, this will be added to the Output and logged as soon as the method is finished. 
```
var name = $"{input.SingleArgument} {input.SingleQuote}";
WriteLine($"Hello {name}");
```
This will add a row to the log with the output from the WriteLine call automatically.
## <a name='UseCommandBaseWritemethods'></a>Use CommandBase Write methods
The CommandBase class has a couple of help methods to output data to the console, use that rather then Console.WriteLine. The base class Write methods will automatically return all the output during a Run method execution, and this output will be logged by the logger component.
```
WriteHeadLine("Header", addToOutput: true);
WriteLine("Write line, also ends up in the log (default behavior)");
WriteProcessLog("Write with WriteProcessLog adds a tag do log","process1");
//You could also use extensions methods
this.WriteObjectDescription(name:"Megabytes", description:"A numeric value");
```
## <a name='ButIreallywanttowritetothelogdirectlyinmyCommandclass'></a>But? I really want to write to the log directly in my Command class!
It is something that is not encouraged, quite the opposite but in some cases you may need to do that, and you can (using an extension method).
```
IPowerCommandServices.DefaultInstance?.Logger.LogInformation($"Log this information please");
```
# CONFIGURATION
## <a name='UseYAML'></a>Use YAML
 The PowerCommands.Configuration component has generic support for reading and writing object as YAML and that format should always be used for configuration files. 
## <a name='Configurationcouldbesharedbyclients'></a>Configuration could be shared by clients
The main configuration should be named PowerCommandsConfiguration.yaml and be a valid YAML file, it could be shared by many clients residing in the same directory.
In such cases they should support the identical configuration structure otherwise there could be trouble, most probably some Console client won´t start.

# Documentation
## <a name='UseMarkdownformat'></a>Use Markdown format
 The format of dokumentation in text should use the markdown format.
## <a name='AlwaysdescribeyourPowerCommands'></a>Always describe your PowerCommands
 Be kind to your consumer as it often turn out to be you that are the consumer, fill in a shourt description of what the PowerCommand is doing and how to use it.
## <a name='UsePowerCommandattribute'></a>Use PowerCommand attribute
This is an example where every property of the attributes is used

![Alt text](images/attributes.png?raw=true "Attributes")
Attributes is used to show a nice description of the command with the built in --help option. 

```
regression --help
```
![Alt text](images/power_command_attribute.png?raw=true "Usage of help option")

But it is just not for displaying the help, the option property and the suggestion property of the PowerCommandAttribute controls suggestions provided by intellisense, you should really take advantage of that.

## <a name='Options'></a>Options
In the example image showing usage of the attributes you could se that the options: property is set to **"mode|name"**, that means that this command has two option one named **mode** and the other one is **name**. This will give the user autocomplete feedback when typing - and using the [Tab] tangent.

Programmatically you can use Options in two ways, you could grab the value, which is the parameter typed after the option, like this (using the RegressionCommand as example, it is a user created command).

``` regression --mode normal --name "My sample project" ```

To get the value of the **mode** option you code like this:

``` var mode = input.GetOptionValue("mode"); ```

Sometimes you just want a option without a value, you can solve that like this:

``` var mode = input.HasOption("xml"); ```

### Do not use the help option, unless you want to override it´s behaviour
It will not harm anything but --help will trigger the Core framework to display generic help (using the PowerCommandDesign attribute).
You can override this behavior if you set the property **overrideHelpOption** to true. Do not do that if your not intend to implement your own show help functionality for that command.


# ARCHITECTURE
## <a name='Dependancydiagram'></a>Dependancy diagram
![Alt text](images/dependancies_example.png?raw=true "Describe")

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)