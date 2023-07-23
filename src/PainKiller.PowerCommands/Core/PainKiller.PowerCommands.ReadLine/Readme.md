# ABOUT READLINE
A Pure C# GNU-Readline like library for .NET/.NET Core

PowerCommands is using an extended version of [ReadLine](https://github.com/tonerdo/readline) published on Github by [Toni Solarin-Sodara](https://github.com/tonerdo).

The core functionality is the same, but some code is added to suit the need for PowerCommands, the license model is still the same.

## MIT License permissions
- Commercial use
- Modification
- Distribution
- Private use

## Events
Events related to keyboard input is exposed as static event by the ```ReadLineService``` this events is possible to add listeners to.

### OpenShortCutPressed
Occurs when user press [`Ctrl + O`].
```
public DemoCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) => ReadLineService.OpenShortCutPressed += OpenShortCutPressed;
    private void OpenShortCutPressed() => WriteSuccessLine("You pressed [CTRL]+[O]");
```
### SaveShortCutPressed
Occurs when user press [`Ctrl + S`].

### **CmdLineTextChanged**
Listen to every key input, you will get the current input not just the pressed key.

**Example**
This is a real world example, with this command the user is guided for every input the user is doing a toolbar will help the user what to input in the next step.
I have removed the Run() implementation code to make the example smaller, but you can se how the event is used.
```
public class AddCommand : DisplayCommandsBase
{
    public AddCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) => ReadLineService.CmdLineTextChanged += ReadLineService_CmdLineTextChanged;
    private void ReadLineService_CmdLineTextChanged(object? sender, ReadLine.Events.CmdLineTextChangedArgs e)
    {
        if(!e.CmdText.StartsWith("add")) return;
        var args = e.CmdText.Split(' ');
        var labels = new []{"Press [Space]"};
        switch (args.Length)
        {
            case 2:
                labels = new[] { "<type> (press [tab])","(url,onenote,path,file)"};
                break;
            case 3:
                labels = new[] { args[1],"\"<value>\""};
                break;
            case 4:
                labels = new []{"[Option] (press - then [tab])","--name"};
                break;
            case 5:
                labels = new []{"[Option]","--name","\"<value>\""};
                break;
            case 6:
                labels = new []{"[Option] (press - then [tab])","--tags"};
                break;
            case 7:
                labels = new []{"[Option]","--tags","<comma separated values>"};
                break;
        }
        ToolbarService.DrawToolbar(labels);
    }    
}
```

### **CommandHighlighted**
Works just like the **CmdLineTextChanged** event but triggers when a valid command has been typed by the user.

## ReadLine Shortcut Guide

| Shortcut                       | Comment                           |
| ------------------------------ | --------------------------------- |
| `Ctrl`+`A` / `HOME`            | Beginning of line                 |
| `Ctrl`+`B` / `←`               | Backward one character            |
| `Ctrl`+`C`                     | Send EOF                          |
| `Ctrl`+`E` / `END`             | End of line                       |
| `Ctrl`+`F` / `→`               | Forward one character             |
| `Ctrl`+`H` / `Backspace`       | Delete previous character         |
| `Tab`                          | Command line completion           |
| `Shift`+`Tab`                  | Backwards command line completion |
| `Ctrl`+`J` / `Enter`           | Line feed                         |
| `Ctrl`+`K`                     | Cut text to the end of line       |
| `Ctrl`+`L` / `Esc`             | Clear line                        |
| `Ctrl`+`M`                     | Same as Enter key                 |
| `Ctrl`+`N` / `↓`               | Forward in history                |
| `Ctrl`+`P` / `↑`               | Backward in history               |
| `Ctrl`+`U`                     | Cut text to the start of line     |
| `Ctrl`+`W`                     | Cut previous word                 |
| `Backspace`                    | Delete previous character         |
| `Ctrl` + `D` / `Delete`        | Delete succeeding character       |
| `Ctrl` + `↓`                   | Move cursor down one line         |
| `Ctrl` + `↑`                   | Move cursor up one line           |