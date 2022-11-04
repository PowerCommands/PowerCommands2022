using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Store object with StorageService, create a backup, delete the object",
    arguments: "<mode> (read,create,backup,delete)",
    argumentMandatory: true,
    example: "storage create|storage read|storage backup|storage delete")]
public class StorageCommand : CommandBase<CommandsConfiguration>
{
    
    public StorageCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var mode = Input.SingleArgument;
        if(mode == "create") Create();
        else if(mode == "backup") Backup();
        else if(mode == "delete") Delete();
        else Display();
        return Ok();
    }

    private void Display()
    {
        var items = StorageService<ObjectDB>.Service.GetObject().Items;
        foreach (var item in items) WriteLine($"{item.ID} {item.Name} {item.Created}");
    }
    private void Create()
    {
        var items = new List<Item>();
        for(int i = 1; i<11;i++) items.Add(new Item { ID = i, Created = DateTime.Now, Name = $"Item {i}"});
        var objectDB = new ObjectDB();
        objectDB.Items.AddRange(items);
        var fileName = StorageService<ObjectDB>.Service.StoreObject(objectDB);
        WriteLine($"Objects stored in file [{fileName}]");
    }

    private void Backup()
    {
        var fileName = StorageService<ObjectDB>.Service.Backup();
        WriteLine($"Objects backed up to file [{fileName}]");
    }
    private void Delete()
    {
        if (!DialogService.YesNoDialog($"Are you sure you want to delete the object?\n(this will delete the file and can not be undone)")) return;
        var fileName = StorageService<ObjectDB>.Service.DeleteObject();
        WriteLine($"File [{fileName}] deleted");
    }
}

public record ObjectDB { public List<Item> Items { get; set; } = new(); }
public class Item
{
    public string Name { get; set; } = "";
    public int ID { get; set; }
    public DateTime Created { get; set; }
}