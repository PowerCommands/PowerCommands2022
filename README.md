# PowerCommands2022
PowerCommands is a concept for creating your own customized command prompt to perform simple or advanced task with the full control from your command environment. That means no time-consuming hassling with a GUI, concentrate on just the code. The concept relays heavily on reflection so adding your own PowerCommands is very simple and fast.

## Main components
 - PowerCommand Client
 - Bootstrap component 
 - Third party components
 - Core componentss
 - Custom components
 - Reusable Commands
 - PowerCommands

 ## Design principles
 The design principles for this project is to keep the Core lightweight and simple. The Core components is components that you probably or very often will use, while Custom Components do not have that characteristic feature. Another restriction for the Core components is that they should avoid to add any third party dependancies, that way you know exactly what code you are running. 
 
 Custom component are aloud to break that rule, custom components should on the other hand avoid to have depandencies to the Core components, they should be design to work as stand-alone components.

 Read more about design principles here: [Design principles](PowerCommands%20Design%20Principles.md)

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

 # Implementations
 
 ## GlitchFinder power commands using GlitchFinder component by Jooachim

 Either compare two different datasets or compare a single dataset towards itself over time.
 If there are glitches in the matrix then you get a report showing lines that are missing or different in the two datasets.

### Comparison
This is for comparing two different data sources, could be two different files, two different DB queries, one file vs one query etc.

### Regression test
This is for tracking a single dataset/source over time. You create a baseline, which is stored on file, and then later compare data towards this baseline.

  