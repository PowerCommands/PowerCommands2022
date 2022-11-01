# Documentation Index DB

The Core framework uses a very simple Documentation Index that is stored as an json file named **DocsDB.data** in the **%USERNAME%\AppData\Roaming\PowerCommands** directory.
You could also add new entries to this index if you want, prefered way is to use the **DocCommand[DocCommand](https://github.com/PowerCommands/PowerCommands2022/blob/main/src/PainKiller.PowerCommands/PainKiller.PowerCommands.MyExampleCommands/Commands/DocCommand.cs)** and use this command.

The example adds a new Doc instans to the DocsDB with a link to google and some tags to it.
```
doc "https://www.google.se/" --name Google-Search --tags tools,search,google
```

## Usage
The DescribeCommand uses this DocsDB to find help for you, if you type anything that matches the name or a tag the application will open that URL for you, every page in this github documentation is added to this index.

## Update the DocsDB Index
Easiest way is to start the applicatin from the bin folder, and use this command
```
powercommands update
```

If you run the same command while debugging in Visual Studio the whole Core Framwork will be updated. (no harm doing that but you need to be aware of it)
The update will merge the content of your local stored file with the file on Github, so you do not lose your own added Docs.

Read more about:

[Basic application configuration](Configuration.md)

[Self documentation using attribute](PowerCommandAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)