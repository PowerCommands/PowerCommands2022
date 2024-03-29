# Power Commands
You could see PowerCommands as your CLI application starter kit. It is a structured framework for creating your own customized command prompt to perform tasks with the full control from your command environment. That means no time-consuming hassling with a GUI, try catch block, validate your input, you do not need to write a zilion test classes either. Just concentrate on the code, create your commands and run the Console application.

[Follow progress on twitter](https://twitter.com/PowerCommands) <img src="https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/images/Twitter.png?raw=true" alt="drawing" width="20"/>

## Version 1.0.3.2
**Released 2023-12-26**
- `DialogService.ListDialog` has major improvements with paging and select all functionality and have been moved to a new service, named `ListService`.
- `ProxyCommand` now handles suggestion overrides in configuration.
- `ConsoleTableService.RenderTable` generic constraint new() removed since it is not needed.
- Bookmarks in `CdCommand` now handles `%USERNAME%` as a placeholder for current user in paths. 

## Version 1.0.3.1
**Released 2023-11-30**
- Adjusted the Power Command to be run with a service account and use encryption, update the setup to reflect this.
- Added general option `--pc_force_quit` in CommandBase to be used with any command to force application to quit.
- Improved encryption.
- Bug fix DialogService.ListDialog now handles 0 input more gracefully.
- Bug fix CommandBase now return RunResultStatus.Quit when using Quit()

**Release 2023-11-18**
- Updated to .NET 8
- Using C# 12
- Updated YamlDotNet to current latest stable version
- Updated Microsoft.Extensions.Logging.Abstractions to current latest stable version
- Updated Serilog and Serilog.Sinks.File to current latest stable version

 ### The core components offering this to your custom PowerCommands
 - Command completion, with history, suggestions, options and support for Path/File navigation and color highlighting when typing a valid command
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

[Override Design attribute in config file](Docs/OverrideDesignAttribute.md)

[Patterns to use the Toolbar with dynamic content](Docs/PowerCommandToolbarAttribute.md)

[Command base class](Docs/CommandBase.md)

[Handling the Input](Docs/Input.md)

[Dialog services](Docs/DialogService.md)

https://github.com/PowerCommands/PowerCommands2022/assets/102176789/10945ee0-947d-4af3-aef5-27ad8ff6be4b

[Using Options](Docs/Options.md)

[Simple automated test](Docs/Test.md)

[Output to the Console guideline](Docs/ConsoleOutput.md)

## Automation
[Run your command as job](Docs/Job.md)

## Core framework
[PowerCommandsRuntime](Docs/PowerCommandsRuntime.md)

## Configuration and documentation
[Basic application configuration](Docs/Configuration.md)

[Extend your configuration](Docs/ExtendYourConfiguration.md)

[Configure your environment to use encrypt/decrypt secrets](Docs/Secrets.md)

[Documentation index](Docs/DocumentationIndexDB.md)

## Console Input
[Shortcut guide and listening on keyboard events](Docs/ReadLine.md)

## Architecture
[Design principles and guidlines](Docs/PowerCommands%20Design%20Principles%20And%20Guidlines.md)

[Customize your project](Docs/Customize.md)

---

## Implementations

# PainKiller Security Tools

PainKiller Security Tools combines **CycloneDX** and **Dependency Track** to let you create SBOM files from git repos and import them in to OWASP Dependency Track which gives you a nice GUI where you can analyze your repos vulnerabilities. 

<img src="Docs/images/dt_cdxgen_logos.png" alt="cdxgen" width="512"> 

[PainKiller Security Tools on Github](https://github.com/PowerCommands/SecTools)

## Links
[10 design principles for delightful CLIs](https://blog.developer.atlassian.com/10-design-principles-for-delightful-clis/)
