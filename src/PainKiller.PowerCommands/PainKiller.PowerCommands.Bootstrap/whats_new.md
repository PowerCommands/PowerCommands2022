# What is new?

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
