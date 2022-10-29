# Customize your project

In your PowerCommands solution you have free hands to build you excellent CLI application, using the Core Framework together with your own homebrew code and code from others.
Here is good things you need to know and think about.

- Do not change code in the Core, use the recommended customizeble area insted, if you do change the core you may break compability and can not use the update functionallity because that will overwrite your changes.
- If your business logic is generall and could be reused by many Commands projekct and maybe by other people, build a Custom Component.
- In the bootstap project you can change some basic behaviour if you want.
- In the Commands project you can customize and extend the Configuration used by your Commands.
- The class **PowerCommandServices.cs** loads basic services like the logging, the runtime and diagnostic handlar, here is the place to swap them if you feeling brave and adventurous.
- **Startup.cs** is a good place to add code that you will run when the application starts.
- **Program.cs** is the main entry for the application, feel free to add stuff here, se example below.

**The Customizable area**

![Alt text](images/CustomizePowerCommands.png?raw=true "Customizable")

## Example, protect your application with a login at startup.
This is the customized **Startup.cs**
```
ConsoleService.WriteHeaderLine(nameof(Program), "Password protected demo 1.0");
var manager = PainKiller.PowerCommands.Bootstrap.Startup.ConfigureServices();

var loginCommando = new LoginCommando("login",(CommandsConfiguration) manager.Services.Configuration);
var loginResult = loginCommando.Run();
if (loginResult.Status != RunResultStatus.Ok) return;

manager.Run(args);
```
## One more example, tweak the code completion preload of values.

Open the **PowerCommandServices.cs** and you will find this code, where the codecompletion loads with name of the commands and existing suggestion defined in the [PowerCommandAttribute](PowerCommandAttribute.md)

```
var suggestions = new List<string>(Runtime.CommandIDs);
suggestions.AddRange(Runtime.CommandIDs.Select(s => $"describe {s}").ToList());
suggestions.AddRange(Runtime.Commands.Where(c => !string.IsNullOrEmpty(c.GetDefaultParameter())).Select(c => $"{c.Identifier} {c.GetDefaultParameter()}").ToList());
```

Read more about:

[Extend your configuration](ExtendYourConfiguration.md)

[Design principles and guidlines](PowerCommands%20Design%20Principles%20And%20Guidlines.md)