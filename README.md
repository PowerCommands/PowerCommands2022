# PowerCommands2022
You could see PowerCommands as your CLI applikation starter kit. It is a structured framework for creating your own customized command prompt to perform tasks with the full control from your command environment. That means no time-consuming hassling with a GUI, try catch block, validate your input, you do not need to write a zilion test classes either. Just concentrate on the code, create your commands and run the Console applikation.

[Follow progress on twitter](https://twitter.com/PowerCommands) <img src="https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/images/Twitter.png?raw=true" alt="drawing" width="20"/>

 ### The core components offering this to your custom PowerCommands
 - Command completion, with history, suggestions, options and support for Path/File navigation and color highlightning when typing a valid command
 - Secret handling to protect sensitive information like password or authentication tokens in the configuration file. 
 - Configuration with YAML (built to be very easy to extend)
 - Logging (using Microsoft.Extensions.Logging.ILogger)
 - Built in help system with the use of attributes
 - Run as job
 - Validation rules with attribute
 - Toolbar labels to guide the user
 - Diagnostic  
 - Progressbar
 - Dialog service (with password prompting) 
 - Navigation with working directory and familiar cd and dir commands

https://github.com/PowerCommands/PowerCommands2022/assets/102176789/d8ad92f0-93aa-4350-b37b-d0961108117d 

 ## Start your journey
[Create a new VS Solution](Docs/Create_new_%20project.md)

[Create a new Command](Docs/Create_new_command.md)

## Commands development
[Design your Command](Docs/Design_command.md)

[Power Commands Design attribute](Docs/PowerCommandDesignAttribute.md)

[Patterns to use the Toolbar with dynamic content](Docs/PowerCommandToolbarAttribute.md)

[Command base class](Docs/CommandBase.md)

[Handling the Input](Docs/Input.md)

[Using Options](Docs/Options.md)

[Simple automated test](Docs/Test.md)

[Output to the Console guidline](Docs/ConsoleOutput.md)

## Automation
[Run your command as job](Docs/Job.md)

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
