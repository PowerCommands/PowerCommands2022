using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Documentation;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;


[PowerCommand(  description: "Add a document to the DocDB that is used as an KnowledgeDB internally by PowerCommands",
                example: "/*Add a new document*/|doc \"https://www.google.se/\" --name Google-Search --tags tools,search,google")]
public class DocCommand : CommandBase<CommandsConfiguration>
{
    public DocCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration)  { }

    public override RunResult Run()
    {
        var item = new Doc { DocID = Guid.NewGuid(), Name = Input.GetFlagValue("name"), Version = 1, Tags = Input.GetFlagValue("tags"), Updated = DateTime.Now, Uri = Input.SingleQuote };
        Print(item);
        if (!DialogService.YesNoDialog("Is this information correct?")) return CreateRunResult();

        var db = StorageService<DocsDB>.Service.GetObject();
        if (db.Docs.Any(i => i.Name == item.Name))
        {
            WriteLine($"Warning, there is already a doc with the same name stored in DB");
            return CreateRunResult();
        }
        db.Docs.Add(item);
        StorageService<DocsDB>.Service.StoreObject(db);
        WriteLine($"Document {item.Name} has successfully been added.");
        return CreateRunResult();
    }

    private void Print(Doc doc)
    {
        Console.WriteLine($"{nameof(doc.Name)}: {doc.Name} ");
        Console.WriteLine($"{nameof(doc.Uri)}: {doc.Uri} ");
        Console.WriteLine($"{nameof(doc.Tags)}: {doc.Tags} ");
    }
}