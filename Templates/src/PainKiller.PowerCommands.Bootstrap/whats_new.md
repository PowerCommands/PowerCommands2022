# What is new?
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
- A new service to be used when running a Command from a command.
- Improved the `ClsCommand` so that the `cls` really clears all the input, which is a security improvement.
- Fixed a bug that duplicated the output to the RunResult instance.
- Added the feature to Commands command to pickup and display diagnostics about the latest RunResult (if any)
- Log view is now the default action instead of viewing log files
- Improved `cd` and `dir` command, added som nice new features to them
- Nuget packages has been updated.
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
## Version 1.0.3.0
**Released 2023-11-18**
- Updated to .NET 8
- Using C# 12
- Updated YamlDotNet to current latest stable version
- Updated Microsoft.Extensions.Logging.Abstractions to current latest stable version
- Updated Serilog and Serilog.Sinks.File to current latest stable version
## Version 1.0.2.2
**Released 2023-09-09**
- Some of the values set with ```PowerCommandsDesignAttribute``` is now possible to override in the **PowerCommandsConfiguration.yaml** file, look at the example that comes with the VS template or read the documentation on GitHub.
- Removed checksum control at startup as it achieves no really security purpose.
- Minor fixes in core dialogs.
## Version 1.0.2.1
**Released 2023-07-24**
- ```ReadLineService``` now has two static events by the ```OpenShortCutPressed``` Occurs when user press [`Ctrl + O`], ```SaveShortCutPressed``` Occurs when user press [`Ctrl + S`].
- ```DialogService.ListDialog``` now returns a ```Dictionary<int, string>``` instead of ```List<string>``` so string value and index is returned for each selected item.
- ```ChecksumManager``` now exposing it´t functions for calculating MDF checksum.
- ```DialogService``` standard dialogs has some minor improvements.
- ```PowerCommandsManager.RunCustomCode``` now has a parameter ```RunFlowManager runFlow```.
- Adjusted creation of commands using ```powercommand new --command Name``` so that the new command class now has the correct namespace. (removed PainKiller.PowerCommands.)
## Version 1.0.2.0
**Released 2023-07-01**
- Toolbar functionality moved to own ```ToolbarService``` and reworked it completely, not using timers anymore that caused problems, so it is now a more stable feature (but still a bit experimental).
- Added PasswordPromptDialog to the ```DialogService```.
- New List feature, display a list which selectable items.
- ```DirCommand``` is now a Core command instead of a Demo command.
- It is now possible to move the cursor up and down with ```CTRL``` + (```⬆️```  or ⬇️).
## Version 1.0.1.0
**Released 2023-06-20**
### Toolbar styled Commands
- Added new base command class ```CommandWithToolbarBase``` that opens upp for a new way of designing your commands with a displayed toolbar in the bottom right corner. Can either be display suggestions or be set programmatically listening and reacting on to the cmd line input.
- Added the constant for array splitter to the ```ConfigurationGlobals```, and refactored all code to use it, makes it easier to swap that if necessary.
- Adjusted the run process so that RunCompleted is always triggered, when running async or when a exception is thrown.
## Version 1.0.0.1
**Released 2023-06-12**
### Capability to Run PowerCommand with a service account and using secrets
If you want to run your PowerCommands application as a Windows scheduled task started by a service accounts that is not allowed to login on the machine, you need to do a couple of steps.

- You need to copy the PainKiller directory from your %User%\AppData\Roaming to the corresponding one for the service account. 
- Copy the environment variable _encryptionManager and create the same as a system environment variable. EncryptionService will look for the system environment variable if the user environment variable is not there. 
- For your stored secrets you will need to to do the same thing and change target: User to target: Machine in the **PowerCommandsConfiguration.yaml** file.
### Improved logging
Every log post now includes the current user running the PowerCommand application, the Log commands has been re-designed to use suggestions instead of options where more appropriate.

