using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommandDesign(  description: "Find a knowledge item, use the index value to open, edit or delete it",
                arguments:"<SearchPhrase>",
                flags: "delete|edit|tags|name|source|view|append",
                example: "//Show the latest added documents|find --latest|//Show all documents|find --latest *|//Find something you are looking for, you can use two search arguments, the second argument is a filter on whats found with the first argument.|find mySearh|//Find something you are looking for, you can use two search arguments, the second argument is a filter on whats found with the first argument.|find mySearch myFilter|//Open a document from the latest search with the provided index|find 0|//Delete a document from the latest search with the provided index/|find 0 --delete|//Append tag(s) for a document from the latest search with the provided index|find 0 --append --tags addMyTags|//Edit the document from the latest search with the provided index|find 0 --edit --tags myNewTags --name myNewName --source MyNewSource-url-path-onenote|//Note that find is default command and could be omitted|//So you could just write like this to open the second item listen in the latest search|1|//If autostart is enabled and the search just find 1 item, that item will be opened automatically")]
public class FindCommand : DisplayCommandsBase
{
    public FindCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        var editAction = base.Run();
        if (editAction.Status == RunResultStatus.Ok) return Ok();

        Items = Storage.GetObject().Items.Where(i => i.Name.ToLower().Contains(Input.SingleArgument.ToLower()) || i.Tags.ToLower().Contains(Input.SingleArgument.ToLower())).OrderByDescending(i => i.Created).ToList();
        if (Input.Arguments.Length > 1) Items = Items.Where(m => m.Name.ToLower().Contains(Input.Arguments[1].ToLower()) || m.Tags.ToLower().Contains(Input.Arguments[1].ToLower())).OrderByDescending(i => i.Created).ToList();
        
        Print();
        return Ok();
    }
}