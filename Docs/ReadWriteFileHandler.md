# Read, Write and navigate to files and directories

PowerCommands have some built in functionality to work with files and directories.

## FileCommand
With `file` command you can do simple tasks like read the file and display the content in the console, you can view file properties, move, copy and delete the file.
You can navigate through files and directories by using the Tab key for auto-completion, this is how you read a file.

### Read
```file test.txt --read```
### View properties
To view the file properties just use `--properties` flag.
### Copy or move
If you want to copy a file you use the `--copy` option flag and give the flag a file name to create a new file, use the `--overwrite` flag if you allow the new file to overwrite an existing one.

```file test.txt --copy "new_file.txt"```

Move is done the same way, just use the `move` flag instead.
### Delete
You delete a file with the `--delete` option flag, if you want to confirm this first just add the `--confirm` flag.

```file test.txt --delete --confirm```
### Write to file, alternative 1
Write a file can be done in different ways, first we start with a scenario where you want to write some input from the console line direct to a specified file.

```file input.txt --write --text "This is my input that I like to write to the file input.txt"```
### Write to file, alternative 2
Now I want to write the output from an other command using the `file` command, in this example I write the output from `version` command.

```file version.txt --write --command version```
### Write to file, alternative 3 using chained execution of commands
The last alternative is very similar to alternative 2 but I will use a new feature to [chain command execution)](ChainCommands.md.md), this is not a feature that could only be used by the `file` command, it could be used by any commands that inherits the BaseCommand which all commands should do. 

You can clearly see the difference in the code below, notice that you call the other command first and "send" the output to the `file` command using the chain execution functionality.
In this example I am using the `--target` flag to provide the file name, you could if you want and if the source command supports it provide the filename as the first parameter, but in many cases the source command may not work properly if it expects som other parameter first, so using the `--target` is the safest way, but not as practical maybe, if the target command expects a file name as first parameter and inherits the `CdCommand` with code completion help it is the most practical solution. 

```version | file --target "version.txt"```

This will also work.

```version version.txt | file```

## CdCommand

You use the cd command pretty much as you would use it in a standard Windows console.
### Change directory
Change directory using a directory name (the directory must exist in current working folder)

```cd logs```

Change directory using a bookmark option flag, bookmark must be added to the [Configuration)](Configuration.md) file.
```cd --bookmark 0```
### Configuration
```
bookmark:
    bookmarks:
    - name: Program
      path: C:\Program Files
      index: 0
```
Change directory to a special directory using a option flag, the special directories are roaming, startup, recent, documents, programs, windows, profile, templates, videos, pictures, music.

```cd --roaming```

This will change working directory to the application roaming directory for PowerCommands.

### Inherit CdCommand to add file/dir code completion
If you are design a command that will handle files or directories you may want to help the user with code completion, you can implement this easily just inherit the CdCommand and then your Command will support just that!
The Command below wil handle navigation trough files and directories (in current working folder) just using tab.
```
[PowerCommandDesign(description: "Run commands that supports pipe functionality.",
                        example: "//First run this command and then the version command|version [PIPE] pipe")]
public class BrowseCommand(string identifier, PowerCommandsConfiguration configuration) : CdCommand(identifier, configuration)
{
    public override RunResult Run()
    {
        var fileName = Input.SingleArgument;
        WriteLine(fileName);
        return Ok();;
    }
}
```


## DirCommand
![Alt text](images/dir_and_file_command.png?raw=true "Dir commands")

Show the content in current working directory

```dir```

Add a filter and show only the content in current working directory that matches the filter

```dir --filter .txt```

Show the content in current working directory and browse the directory in the file browser.

```dir --browse```

Show info about the available drives on your system

```dir --drive-info```

Dir also have som nice features that also CdCommand has, as it inherits that command, but that is undocumented as it is unintentional features.
You could for example change the current working directory by writing ```dir directory-name```

Read more about:

[Configuration (see example on how you can add your bookmarks)](Configuration.md)

[Chain command execution)](ChainCommands.md.md)

[Input](Input.md)


[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)