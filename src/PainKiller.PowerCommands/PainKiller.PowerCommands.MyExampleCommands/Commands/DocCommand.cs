﻿namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(          tests: "--list")]
[PowerCommandDesign(  description: "Add a document to the DocDB that is used as an KnowledgeDB internally by PowerCommands, view document, append tags, edit and delete documents. Backup KnowledgeDB",
                            options: "view|delete|edit|append|list|backup",
                          example: "//Add a new document|doc \"https://www.google.se/\" --name Google-Search --tags tools,search,google|//List all documents|doc --list")]
public class DocCommand : CommandBase<CommandsConfiguration>
{
    private List<Doc> _docs = new();
    private Doc? _selectedItem;
    private readonly IStorageService<DocsDB> _storage;
    public DocCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) => _storage = StorageService<DocsDB>.Service;
    public override RunResult Run()
    {
        //When first argument is a integer, the user want to open, edit or delete an item from a previous search, wrong index will throw an IndexOutOfRange exception and that is ok
        _selectedItem = (int.TryParse(Input.SingleArgument, out var index) ? _docs[index] : null)!;

        if (Input.HasOption("delete") && _selectedItem != null) Delete();
        else if (Input.HasOption("edit") && _selectedItem != null) Edit();
        else if (Input.HasOption("append") && _selectedItem != null) Append();
        else if (Input.HasOption("view") && _selectedItem != null) Details();
        else if (Input.HasOption("list")) List();
        else if (Input.HasOption("backup")) Backup();
        else Add();
        return Ok();
    }
    private void Backup()
    {
        var fileName = _storage.Backup();
        WriteHeadLine($"A backup is created to file [{fileName}]");
    }
    private void List()
    {
        _docs = _storage.GetObject().Docs;
        var table = _docs.Select((i, index) => new DocView { ID = index++, Name = i.Name, Tags = i.Tags });
        ConsoleTableService.RenderTable(table, this);
    }
    private void Delete()
    {
        if (!DialogService.YesNoDialog($"Are you sure you want to delete the item {_selectedItem!.Name} {_selectedItem.Uri} {_selectedItem.Tags}?")) return;
        var db = _storage.GetObject();
        var match = db.Docs.First(i => i.DocID == _selectedItem.DocID);
        db.Docs.Remove(match);
        _storage.StoreObject(db);
        _docs = _storage.GetObject().Docs;
        WriteLine($"Item {_selectedItem.DocID} {_selectedItem.Name} removed.");
    }
    private void Edit()
    {
        var db = _storage.GetObject();
        var match = db.Docs.First(i => i.DocID == _selectedItem!.DocID);
        db.Docs.Remove(match);

        if (!string.IsNullOrEmpty(Input.GetOptionValue("name"))) match.Name = Input.GetOptionValue("name");
        if (!string.IsNullOrEmpty(Input.GetOptionValue("tags"))) match.Tags = Input.GetOptionValue("tags");
        if (!DialogService.YesNoDialog($"Are this update ok? {match.Name} {match.Tags}?")) return;
        match.Version += 1;
        db.Docs.Add(match);
        _storage.StoreObject(db);
        _docs = _storage.GetObject().Docs;
        WriteLine($"Item {match.DocID} {match.Name} updated.");
    }
    private void Append()
    {
        if(_selectedItem == null) return;
        var db = _storage.GetObject();
        var match = db.Docs.First(i => i.DocID == _selectedItem.DocID);
        db.Docs.Remove(match);
        match.Tags = $"{match.Tags},{Input.GetOptionValue("tags")}";
        if (!DialogService.YesNoDialog($"Are this update ok? {match.Name} {match.Tags}?")) return;
        match.Version += 1;
        db.Docs.Add(match);
        _storage.StoreObject(db);
        _docs = _storage.GetObject().Docs;
        WriteLine($"Item {match.DocID} {match.Name} updated.");
    }
    private void Details()
    {
        if (_selectedItem != null) Print(_selectedItem, 0);
    }
    private void Add()
    {
        var item = new Doc { DocID = _docs.Count > 0 ? _docs.Max(d => d.DocID) + 1 : 1, Name = Input.GetOptionValue("name"), Version = 1, Tags = Input.GetOptionValue("tags"), Updated = DateTime.Now, Uri = Input.SingleQuote };
        Print(item, 0);
        if (!DialogService.YesNoDialog("Is this information correct?")) return;
        var db = _storage.GetObject();
        if (db.Docs.Any(i => i.Name == item.Name))
        {
            WriteLine($"Warning, there is already a doc with the same name stored in DB");
            return;
        }
        db.Docs.Add(item);
        _storage.StoreObject(db);
        _docs = _storage.GetObject().Docs;
        WriteLine($"Document {item.Name} has successfully been added.");
    }
    private static void Print(Doc doc, int index) => Console.WriteLine($"{index} {doc.Name.PadRight(50)} {doc.Uri.PadRight(100)}\t[{doc.Tags}] version: {doc.Version}");
}