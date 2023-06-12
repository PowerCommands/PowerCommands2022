# What is new?

## Version 1.0.0.1
**Released 2023-06-12**
### Capability to Run PowerCommand with a service account
If you want to run your PowerCommands application as a Windows scheduled task started by a service accounts that is not allowed to login on the machine, you need to do a couple of steps.

- You need to copy the PainKiller directory from your %User%\AppData\Roaming to the corresponding one for the service account. 
- Copy the environment variable _encryptionManager and create the same as a system environment variable. EncryptionService will look for the system environment variable if the user environment variable is not there. 
- For your stored secrets you will need to to do the same thing and change target: User to target: Machine in the **PowerCommandsConfiguration.yaml** file.
### Improved logging
Every log post now includes the current user running the PowerCommand application, the Log commands has been re-designed to use suggestions instead of options where more appropriate.
