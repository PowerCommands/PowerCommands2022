namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

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