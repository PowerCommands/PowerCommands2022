namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommand(  description: "Find a knowledge item, use the index value to open, edit or delete it",
                arguments:"<SearchPhrase>",
                flags: "latest|delete|edit|tags|name|source|view|append",
                example: "//Show the latest added documents|find --latest|//Show all documents|find --latest *|//Find something you are looking for, you can use two search arguments, the second argument is a filter on whats found with the first argument.|find mySearh|//Find something you are looking for, you can use two search arguments, the second argument is a filter on whats found with the first argument.|find mySearch myFilter|//Open a document from the latest search with the provided index|find 0|//Delete a document from the latest search with the provided index/|find 0 --delete|//Append tag(s) for a document from the latest search with the provided index|find 0 --append --tags addMyTags|//Edit the document from the latest search with the provided index|find 0 --edit --tags myNewTags --name myNewName --source MyNewSource-url-path-onenote|//Note that find is default command and could be omitted|//So you could just write like this to open the second item listen in the latest search|1|//If autostart is enabled and the search just find 1 item, that item will be opened automatically")]
public class FindCommand : CommandBase<PowerCommandsConfiguration>
{
    private List<KnowledgeItem> _items = new();
    private KnowledgeItem? _selectedItem;
    
    private readonly IStorageService<KnowledgeDatabase> _storage;

    public FindCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) => _storage = StorageService<KnowledgeDatabase>.Service;

    public override RunResult Run()
    {
        //When first argument is a integer, the user want to open, edit or delete an item from a previous search, wrong index will throw an IndexOutOfRange exception and that is ok
        _selectedItem = (int.TryParse(Input.SingleArgument, out var index) ? _items[index] : null)!;
        if (_selectedItem != null)
        {
            if (Input.HasFlag("delete")) Remove(_selectedItem);
            else if (Input.HasFlag("edit")) Edit(_selectedItem, Input.GetFlagValue("name"), Input.GetFlagValue("source"), Input.GetFlagValue("tags"));
            else if (Input.HasFlag("append")) Append(_selectedItem, Input.GetFlagValue("tags"));
            else if (Input.HasFlag("view")) Details(_selectedItem);
            else Open(_selectedItem);
            return Ok();
        }

        _items = _storage.GetObject().Items.Where(i => i.Name.ToLower().Contains(Input.SingleArgument.ToLower()) || i.Tags.ToLower().Contains(Input.SingleArgument.ToLower())).OrderByDescending(i => i.Created).ToList();
        if (Input.Arguments.Length > 1) _items = _items.Where(m => m.Name.ToLower().Contains(Input.Arguments[1].ToLower()) || m.Tags.ToLower().Contains(Input.Arguments[1].ToLower())).OrderByDescending(i => i.Created).ToList();
        if (Input.HasFlag("latest"))
        {
            var latestDate = Input.GetFlagValue("latest") == "*" ? DateTime.Now.AddDays(-10000) : DateTime.Now.AddDays(-7);
            _items = _storage.GetObject().Items.Where(i => i.Created > latestDate).ToList();
        }
        Print();
        
        return Ok();
    }

    private void Print()
    {
        var separator = Configuration.ShellConfiguration.DisplaySeparator.Replace(@"\t", "\t");
        if (_items.Count == 1 && Configuration.ShellConfiguration.Autostart) Open(_items.First());
        WriteHeadLine($"Found {_items.Count} matches.");
        var index = 0;
        foreach (var item in _items)
        {
            Console.WriteLine($"{index} {item.SourceType}{separator}{item.Name} [{item.Tags}]");
            index++;
        }
    }
    private void Open(KnowledgeItem match)
    {
        IShellExecuteManager? shellExecuteManager = null;
        switch (match.SourceType)
        {
            case "onenote":
                shellExecuteManager = new OneNoteManager();
                break;
            case "path":
                shellExecuteManager = new OpenFolderManager();
                break;
            case "url":
                shellExecuteManager = new BrowserManager();
                break;
            default:
                WriteLine($"The source type {match.SourceType} is not supported, only onenote path or url is valid, you can change that with --edit --source flag on the object, see examples");
                break;
        }
        shellExecuteManager?.Run(Configuration.ShellConfiguration, match.Uri);
    }
    private void Remove(KnowledgeItem item)
    {
        if(!DialogService.YesNoDialog($"Are you sure you want to delete the item {item.Name} {item.SourceType} {item.Tags}?")) return;
        var db = _storage.GetObject();
        var match = db.Items.First(i => i.ItemID == item.ItemID);
        db.Items.Remove(match);
        _storage.StoreObject(db);
        WriteLine($"Item {item.ItemID} {item.Name} removed.");
    }
    private void Edit(KnowledgeItem item, string name, string source, string tags)
    {
        var db = _storage.GetObject();
        var match = db.Items.First(i => i.ItemID == item.ItemID);
        db.Items.Remove(match);

        if (!string.IsNullOrEmpty(name)) match.Name = name;
        if (!string.IsNullOrEmpty(source) && "url path onenote".Contains(source)) match.SourceType = source;
        if (!string.IsNullOrEmpty(tags)) match.Tags = tags;
        if (!DialogService.YesNoDialog($"Are this update ok? {match.Name} {match.SourceType} {match.Tags}?")) return;
        db.Items.Add(match);
        _storage.StoreObject(db);
        WriteLine($"Item {match.ItemID} {match.Name} updated.");
    }
    private void Append(KnowledgeItem item, string tags)
    {
        var db = _storage.GetObject();
        var match = db.Items.First(i => i.ItemID == item.ItemID);
        db.Items.Remove(match);
        match.Tags = $"{match.Tags},{tags}";
        if (!DialogService.YesNoDialog($"Are this update ok? {match.Name} {match.SourceType} {match.Tags}?")) return;
        db.Items.Add(match);
        _storage.StoreObject(db);
        WriteLine($"Item {match.ItemID} {match.Name} updated.");
    }
    private void Details(KnowledgeItem item)
    {
        WriteHeadLine($"{item.Name} {item.ItemID}");
        WriteLine($"{nameof(item.SourceType)}:{item.SourceType}");
        WriteLine($"{nameof(item.Uri)}:{item.Uri}");
        WriteLine($"{nameof(item.Tags)}:{item.Tags}");
        WriteLine($"{nameof(item.Created)}:{item.Created}");
    }
}