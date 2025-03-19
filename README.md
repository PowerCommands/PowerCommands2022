# Power Commands
You could see PowerCommands as your CLI application starter kit. It is a structured framework for creating your own customized command prompt to perform tasks with the full control from your command environment. That means no time-consuming hassling with a GUI, try catch block, validate your input, you do not need to write a zilion test classes either. Just concentrate on the code, create your commands and run the Console application.

[Follow progress on twitter](https://twitter.com/PowerCommands) <img src="https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/images/Twitter.png?raw=true" alt="drawing" width="20"/>

## Version 1.0.4.402
**Released 2025-03-19**
- Fixed a bug regarding encryption
## Version 1.0.4.401
**Released 2025-03-05**
- OpenFile dialog added
- Fixed bug in EnvironmentService
## Version 1.0.4.4
**Released 2025-02-28**
- Improved diagnostic logging
- Minor fixes
## Version 1.0.4.3
**Released 2025-02-02**
- New basic functionality for translation added.
- Improved documentation functionality.
- Added dialog for file and folder select to DialogService.
## Version 1.0.4.2
**Released 2025-01-27**
- New feature InfoPanel
## Version 1.0.4.1
**Released 2025-01-21**
- PowerCommandPrivacyAttribute to prevent sensitive data leak out to the logfile.
- Improved dialog and toolbar service.
- New function for deserialization of Yaml.
- Improved ZipService.
- Every PowerCommand implementation now has it´s own working folder.
- Filter on dir command is now not case sensitive.
- Nuget packages has been updated.
## Version 1.0.4.0
**Released 2024-12-12**
- Added a new `FileCommand` command to Read, Copy, Move, Delete files and show file properties.
- `FileCommand` also handles to write the output for a provided existing Command to a file.
- Added a new service `RunCommandService` to be used when running a Command from a command.
- Improved the `ClsCommand` so that the `cls` really clears all the input, which is a security improvement.
- Fixed a bug that duplicated the output to the RunResult instance.
- Added the feature to `Commands` command to pickup and display diagnostics about the latest RunResult (if any) for a given command (by name).
- Log view is now the default action instead of viewing log files
- Improved `cd` and `dir` command, added som nice new features to them
- Nuget packages has been updated.

 https://github.com/user-attachments/assets/b718cc18-2cbb-4812-b3a2-0d054e948a00

 ## Start your journey
[Create a new VS Solution](Docs/Create_new_%20project.md)

[Create a new Command](Docs/Create_new_command.md)

## Commands development
[Design your Command](Docs/Design_command.md)

[Chain commands calls](Docs/ChainCommands.md)

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

## File handling
[Handling files and directories with file, cd and dir commands](ReadWriteFileHandler.md)

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
