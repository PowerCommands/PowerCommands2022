# Configuration

## PowerCommandsConfiguration.Yaml
The content in these file has two parts, the first part is the core configuration, that part all PowerCommand application has.
Second part is the extended configuration parts, this part is optional if you need custom configuration you need to implement this by your self. The content of this part is then merged into one configuration file. 

## The base configuration class diagram:

![Alt text](images/CommandsConfiguration.png?raw=true "Command Base")

## Properties

### BackupPath
The path to use by a command that needs to store backups of any kind, the Core framework uses the backup for it´s internal [documentation index](DocumentationIndexDB.md).

The Example project on Github has extended the configuration and will therefore be the use case for this documentation.

### CodeEditor
The path to your favorite code editor, that you could start with the ConfigCommand, this command is shipping with the creation of new PowerCommand Visual Studio solution.

### Command design overrides
You can override some of the values that are set in the **PowerCommandsDesignAttribute**, read more about this [here](OverrideDesignAttribute.md)

### Components
Here you declare those dll components that the Core framework will use to find all available Commands.

## Environment
Environment variables that the application needs to be aware about.

## Log
Details about the logging.

## Metadata
Description about your PowerCommand project.

## Repository
URL to this repository, used internally to create new VS solutions and Commands using PowerCommands application.

## Secret
Name of secrets that the application needs to be aware about, each secret has an corresponding environment value that is encrypted.

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
  backupPath: ..\..\..\..\..\
  defaultGitRepositoryPath: C:\repos\github\PowerCommands2022
  commandDesignOverrides:
  - name: demo
    description: "Demo command just to try out how you could use the input, do not forget the MANDATORY option, will trigger a validation error otherwise! ;-)\n That is because the option name is typed with UPPERCASE letters, useful when you want a mandatory option\n The pause option on the other hand starts with a ! symbol meaning that if you add the --pause option you must also give it a value, an integer in this case."
    arguments: "!<url>"
    quotes: "!<local file path>"    
    options: "!MANDATORY|!pause"
    examples: "//Must provide the MANDATORY option, will trigger a validation error otherwise|demo MANDATORY|//Test the pause service|demo --pause 5 MANDATORY"
    suggestions: ""
    useAsync: true
    showElapsedTime: false  
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
  - component: PainKiller.PowerCommands.MyExampleCommands.dll
    checksum: a2df61ea89e4f9ec265d921bfad87193
    name: My Example Command  
  - component: PainKiller.PowerCommands.Core.dll
    checksum: 4f04313db8e67b2bc4b059c478a900af
    name: PainKiller Core
  bookmark:
    bookmarks:
    - name: Program
      path: C:\Program Files
      index: 0
  environment:
    variables:
    - name: OS
      environmentVariableTarget: Machine
```

Next step could be to [Extend your configuration](ExtendYourConfiguration.md)

Read more about:

[Design your Command](Design_command.md)

[Override Design attribute](OverrideDesignAttribute.md)

[Documentation index](DocumentationIndexDB.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)