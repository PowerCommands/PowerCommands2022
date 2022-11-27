namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

public abstract class DisplayCommandsBase : CommandBase<PowerCommandsConfiguration>
{
    protected static List<KnowledgeItem> Items = new();
    protected KnowledgeItem? SelectedItem;
    protected readonly IStorageService<KnowledgeDatabase> Storage;

    protected DisplayCommandsBase(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) => Storage = StorageService<KnowledgeDatabase>.Service;

    public override RunResult Run()
    {
        //When first argument is a integer, the user want to open, edit or delete an item from a previous search, wrong index will throw an IndexOutOfRange exception and that is ok
        SelectedItem = (int.TryParse(Input.SingleArgument, out var index) ? Items[index] : null)!;
        if (SelectedItem != null)
        {
            if (Input.HasOption("delete")) Remove(SelectedItem);
            else if (Input.HasOption("edit")) Edit(SelectedItem, Input.GetOptionValue("name"), Input.GetOptionValue("source"), Input.GetOptionValue("tags"));
            else if (Input.HasOption("append")) Append(SelectedItem, Input.GetOptionValue("tags"));
            else if (Input.HasOption("view")) Details(SelectedItem);
            else Open(SelectedItem);
            return Ok();
        }
        return ContinueWith("");
    }

    protected void Print()
    {
        if (Items.Count == 1 && Configuration.ShellConfiguration.Autostart) Open(Items.First());
        WriteHeadLine($"Found {Items.Count} matches.");
        var table = Items.Select((i, index) => new KnowledgeTableItem { Index = index++, Name = i.Name, SourceType = i.SourceType, Uri = i.Uri, Tags = i.Tags });
        ConsoleTableService.RenderTable(table, this);
    }
    protected void Open(KnowledgeItem match)
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
            case "file":
                shellExecuteManager = new BrowserManager();
                break;
            default:
                WriteLine($"The source type {match.SourceType} is not supported, only onenote path or url is valid, you can change that with --edit --source option on the object, see examples");
                break;
        }
        WriteHeadLine($"Opening [{match.Uri}] with [{shellExecuteManager?.GetType().Name}]");
        shellExecuteManager?.Run(Configuration.ShellConfiguration, match.Uri);
    }
    protected void Remove(KnowledgeItem item)
    {
        if (!DialogService.YesNoDialog($"Are you sure you want to delete the item {item.Name} {item.SourceType} {item.Tags}?")) return;
        var db = Storage.GetObject();
        var match = db.Items.First(i => i.ItemID == item.ItemID);
        db.Items.Remove(match);
        Storage.StoreObject(db);
        WriteLine($"Item {item.ItemID} {item.Name} removed.");
    }
    protected void Edit(KnowledgeItem item, string name, string source, string tags)
    {
        var db = Storage.GetObject();
        var match = db.Items.First(i => i.ItemID == item.ItemID);
        db.Items.Remove(match);

        if (!string.IsNullOrEmpty(name)) match.Name = name;
        if (!string.IsNullOrEmpty(source) && "url path onenote".Contains(source)) match.SourceType = source;
        if (!string.IsNullOrEmpty(tags)) match.Tags = tags;
        if (!DialogService.YesNoDialog($"Are this update ok? {match.Name} {match.SourceType} {match.Tags}?")) return;
        db.Items.Add(match);
        Storage.StoreObject(db);
        WriteLine($"Item {match.ItemID} {match.Name} updated.");
    }
    protected void Append(KnowledgeItem item, string tags)
    {
        var db = Storage.GetObject();
        var match = db.Items.First(i => i.ItemID == item.ItemID);
        db.Items.Remove(match);
        match.Tags = $"{match.Tags},{tags}";
        if (!DialogService.YesNoDialog($"Are this update ok? {match.Name} {match.SourceType} {match.Tags}?")) return;
        db.Items.Add(match);
        Storage.StoreObject(db);
        WriteLine($"Item {match.ItemID} {match.Name} updated.");
    }
    protected void Details(KnowledgeItem item)
    {
        WriteHeadLine($"{item.Name} {item.ItemID}");
        WriteLine($"{nameof(item.SourceType)}:{item.SourceType}");
        WriteLine($"{nameof(item.Uri)}:{item.Uri}");
        WriteLine($"{nameof(item.Tags)}:{item.Tags}");
        WriteLine($"{nameof(item.Created)}:{item.Created}");
    }
}