# Extend your configuration

## PowerCommandsConfiguration.Yaml
The content in these file has two parts, the first part is the [basic application configuration](Configuration.md), that part all PowerCommand application has.
Second part is the extended configuration parts, this part is optional if you need custom configuration you need to implement this by your self. The content of this part is then merged into one configuration file. 

## PowerCommandsConfiguration class
In your PowerCommands Visual Studio project you can find the **PowerCommandsConfiguration.cs** file in the **Configuration** directory and from the start it looks like this.

```
public class PowerCommandsConfiguration : CommandsConfiguration
{
    //Here is the placeholder for your custom configuration, you need to add the change to the PowerCommandsConfiguration.yaml file as well    
}
```

Just as the comment says this class is setup for you to extend as you wish, within the limitation of what could be serialized as yaml. 

## The Example project
The sample project in the Github repo has added a custom configuration object in the Configuration named FavoriteConfiguration, it is a simple POCO class.
```
public class FavoriteConfiguration
{
    public string Name { get; set; } = "Music";
    public string NameOfExecutable { get; set; } = "spotify";
    public string FileExtension { get; set; } = "exe";
}
```

This class needs to be added to the empty PowerCommandsConfiguration class.

```
public class PowerCommandsConfiguration : CommandsConfiguration
{    
    public string DefaultGitRepositoryPath { get; set; } = "C:\\repos";
    public FavoriteConfiguration[] Favorites { get; set; } = new[] {new FavoriteConfiguration {Name = "Music", NameOfExecutable = "spotify"}, new FavoriteConfiguration { Name = "Games", NameOfExecutable = "steam" } };
}
```

Note that a simple string property **DefaultGitRepositoryPath** has been added as well. But this is not enough, the yaml file **PowerCommandsConfiguration.yml** must be updated to contain the new configuration content. The new configuration file will now look like this.

```
version: 1.0
configuration:
  showDiagnosticInformation: false
  defaultCommand: commands
  defaultGitRepositoryPath: C:\repos\github\PowerCommands2022
  codeEditor: C:\Users\%USERNAME%\AppData\Local\Programs\Microsoft VS Code\Code.exe
  repository: https://github.com/PowerCommands/PowerCommands2022
  backupPath: ..\..\..\..\Core\PainKiller.PowerCommands.Core\
  favorites:
  - name: Music
    nameOfExecutable: spotify
    fileExtension: exe
  - name: Games
    nameOfExecutable: steam
    fileExtension: exe  
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
    - name: KEY_VAULT_NAME
      environmentVariableTarget: User
    - name: AZURE_CLIENT_ID
      environmentVariableTarget: User
```

## Heads up before you start to debug
If  you edit the configuration file in the Visual Studio project, you need do make sure that the file is copied to the output folder and... use **Rebuild Solution** once before you start debugging, otherwise it is not sure that the updated file is copied to the output directory.

## Unsure about the yaml format
It is not always crystal clear how the yaml file should look when you are adding your custom configuration. With the **ConfigCommand** you can create a new yaml file that is stored in the application directory with the name **default.yaml**, if you just open that you could see the content and structure.

Use the command, if you do not have it, download it from the Example project [ConfigCommand on Github](https://github.com/PowerCommands/PowerCommands2022/blob/main/src/PainKiller.PowerCommands/PainKiller.PowerCommands.MyExampleCommands/Commands/ConfigCommand.cs).

```
config create
```

Read more about:

[Basic application configuration](Configuration.md)

[Documentation index](DocumentationIndexDB.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)