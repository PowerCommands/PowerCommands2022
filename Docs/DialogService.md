# DialogServices

The **DialogService** contains some static helper for you to present dialogs 

## ListDialog

The ```ListDialog``` is an easy way to let the user select items from a list of strings.

https://github.com/PowerCommands/PowerCommands2022/assets/102176789/10945ee0-947d-4af3-aef5-27ad8ff6be4b

```
[PowerCommandDesign( description: "A demo of the DialogService.List.",
                         example: "list")]
public class ListCommand : CommandBase<PowerCommandsConfiguration>
{
    public ListCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var selectedItems = DialogService.ListDialog("Which teams participated in the Stanley Cup final season 2022/23?", new() { "New York Rangers", "Colorado Avalanche", "Vegas Knights", "Pittsburgh Penguins", "Florida Panthers", "Detroit Red Wings", "Toronto Maple Leafs" });
        WriteHeadLine("You selected");
        foreach (var item in selectedItems) WriteLine(item);
        if (selectedItems.Count == 2 && selectedItems.Any(t => t == "Vegas Knights") && selectedItems.Any(t => t == "Florida Panthers"))
        {
            WriteSuccessLine("\nYour absolutely right, and Vegas Knights won the Stanley Cup title!");
        }
        else
        {
            WriteFailureLine("\nIncorrect, the participants in the final was Vegas Knights Vs Florida Panthers and Vegas won the Stanley Cup title!");
        }
        return Ok();
    }
}
```
## SecretPromptDialog
The ```SecretPromptDialog``` is a helper dialog to collect password, tokens or other sensitive stuff. The input is masked, the user must confirm the secret, and the secret is not logged to the log file. 
It is very simple to use, just like this.
```
var password = DialogService.SecretPromptDialog("Enter secret:");
```

## QuestionAnswerDialog and YesNoDialog
This dialog either prompt a question and returns the answer as an string or with the ```YesNoDialog``` prompts a question and returns a bool.




Read more about:

[Design your Command](Design_command.md)

[Input](Input.md)

[PowerCommands Attribute](PowerCommandAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)