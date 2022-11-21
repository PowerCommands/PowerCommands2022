namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Description of your command...",
                         options: "add|remove",
                         example: "//Show all todo:s|todo|//Add selected index to todo list|todo <index> add|//Remove selected index from todo list|todo <index> remove")]
public class TodoCommand : DisplayCommandsBase
{
    public TodoCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        SelectedItem = (int.TryParse(Input.SingleArgument, out var index) ? Items[index] : null)!;
        if (SelectedItem != null)
        {
            if (Input.HasOption("add")) Append(SelectedItem, "todo");
            else if (Input.HasOption("remove")) RemoveTodo(SelectedItem);
            return Ok();
        }
        ShowAll();
        return Ok();
    }
    private void ShowAll()
    {
        Items = Storage.GetObject().Items.Where(i => i.Tags.ToLower().Contains("todo")).ToList();
        Print();
    }
    private void RemoveTodo(KnowledgeItem item)
    {
        var db = Storage.GetObject();
        var match = db.Items.First(i => i.ItemID == item.ItemID);
        if (!match.Tags.ToLower().Contains("todo"))
        {
            WriteLine($"Item {item.Name} does not have a todo tag.");
            return;
        }
        db.Items.Remove(match);
        match.Tags = $"{match.Tags}".ToLower().Replace(",todo","").Replace("todo","");
        if (!DialogService.YesNoDialog($"Are this update ok? {match.Name} {match.SourceType} {match.Tags}?")) return;
        db.Items.Add(match);
        Storage.StoreObject(db);
        WriteLine($"Item {match.ItemID} {match.Name} updated.");
    }
}