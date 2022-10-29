# Configuration

## PowerCommandsConfiguration.Yaml
The content in these file has two parts, the first part is the core configuration, that part all PowerCommand application has.
Second part is the extended configuration parts, this part is optional if you need custom configuration you need to implement this by your self. The conten of this part is then merged into one configuration file. 

As already mentioned if you do not need any more configuration you do not neeed to do anything.

## The base configuration class diagram:

![Alt text](images/CommandsConfiguration.png?raw=true "Command Base")

## Properties

### BackupPath
The path to use by a command that needs to store backups of any kind, the Core framework uses the backup for it´s internal [documentation index](DocumentationIndexDB.md).

The Example project on Github has extended the configuration and will therefore be the use case for this documentation.

### CodeEditor
The path to your favorite code editor, that you could start with the ConfigCommand, this command is shipping with the creation of new PowerCommand Visual Studio solution.

### Components
Here you declare those dll components that the Core framework will use to find all available Commands.

## Environment
Environment variables that the application needs to be aware about.

## Log
Details about the loggning.

## Metadata
Description about your PowerCommand project.

## Repository
URL to this repsository, used internally to create new VS solutions and Commands using PowerCommands applikation.

## Secret
Name of secrets that the application needs to be aware about.

## ShowDiagnosticInformation
Enable or disable the display of diagnostic output.

## Example

``` 
version: 1.0
configuration:
  showDiagnosticInformation: false
  defaultCommand: commands  
  codeEditor: C:\Users\%USERNAME%\AppData\Local\Programs\Microsoft VS Code\Code.exe
  repository: https://github.com/PowerCommands/PowerCommands2022
  backupPath: ..\..\..\..\Core\PainKiller.PowerCommands.Core\  
  metadata:
    name: Test
    description: En exempelbeskrivning
  log:
    fileName: powercommands.log
    filePath: logs
    rollingIntervall: Day
    restrictedToMinimumLevel: Information
    component: PainKiller.SerilogExtensions.dll
    checksum: 13b9944b55efe60362720a679f17a22c
    name: Serialog
  components:
  - component: PainKiller.PowerCommands.Core.dll
    checksum: 4f04313db8e67b2bc4b059c478a900af
    name: PainKiller Core
  - component: PainKiller.PowerCommands.MyExampleCommands.dll
    checksum: a2df61ea89e4f9ec265d921bfad87193
    name: My Example Command
  secret:
    secrets:
    - name: localDB
      options:
        target: User
  environment:
    variables:
    - name: VAULT_NAME
      environmentVariableTarget: User
    - name: CLIENT_ID
      environmentVariableTarget: User
```

Read more about:

[Design your Command](Design_command.md)

[Documentation index](DocumentationIndexDB.md)