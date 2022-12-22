# PowerCommands2022
You could se PowerCommands as an easy Create your CLI applikation starter kit. It is a structured framework for creating your own customized command prompt to perform simple or advanced task with the full control from your command environment. That means no time-consuming hassling with a GUI, try catch block, validate your input, you do not need to write a zilion test classes either. Just concentrate on the code, create your commands and run the Console applikation.

[Follow progress on twitter](https://twitter.com/PowerCommands) <img src="/raw/main/Docs/images/twitter.png" alt="drawing" width="20"/>

 ### The core components offering this to your custom PowerCommands
 - Logging (using Microsoft.Extensions.Logging.ILogger)
 - Process log (using the standard logger with tags to track a process)
 - Configuration with YAML (built to be very easy to extend)
 - Display your POCO:s easy with a nice Console Table display output
 - Simple to write test rules with attribute
 - Validation rules with attribute
 - Diagnostic 
 - Secret handling to protect sensitive information like password or authentication tokens in the configuration file.
 - Command completion, with history, suggestions, options and support for Path/File navigation and color highlightning when typing a valid command
 - Progressbar
 - Download files
 - Password prompt
 - Run as job
 - Zip with attributes like checksum, filecount and file size
 - Describe your commands with code examples using attributes. 

 ![Alt text](Docs/images/test_all.png?raw=true "Test result")

 ## Start your journey
[Create a new VS Solution](Docs/Create_new_%20project.md)

[Create a new Command](Docs/Create_new_command.md)

## Commands development
[Design your Command](Docs/Design_command.md)

[Power Commands Design attribute](Docs/PowerCommandDesignAttribute.md)

[Command base class](Docs/CommandBase.md)

[Handling the Input](Docs/Input.md)

[Using Options](Docs/Options.md)

[Simple automated test](Docs/Test.md)

[Output to the Console guidline](Docs/ConsoleOutput.md)

## Core framework
[PowerCommandsRuntime](Docs/PowerCommandsRuntime.md)

## Configuration and documentation
[Basic application configuration](Docs/Configuration.md)

[Extend your configuration](Docs/ExtendYourConfiguration.md)

[Configure your environment to use encrypt/decrypt secrets](Docs/Secrets.md)

[Documentation index](Docs/DocumentationIndexDB.md)

## Architecture
[Design principles and guidlines](Docs/PowerCommands%20Design%20Principles%20And%20Guidlines.md)

[Customize your project](Docs/Customize.md)

## Implementations
[Glitchfinder and KnowledgedDB](Docs/Implementations.md)

## Links
[10 design principles for delightful CLIs](https://blog.developer.atlassian.com/10-design-principles-for-delightful-clis/)