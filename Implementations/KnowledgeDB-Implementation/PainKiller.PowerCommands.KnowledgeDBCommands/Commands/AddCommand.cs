﻿using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.KnowledgeDBCommands.Configuration;
using PainKiller.PowerCommands.KnowledgeDBCommands.DomainObjects;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommand(  description: "Add a new knowledge item",
                flags: "onenote|url|path|name|tags",
                example: "/*Add url*/|add --url \"https://wiki/wikis?pagePath=%2FMain&wikiVersion=GBwikiMaster&pageId=1\" --name WikiStart --tags wiki,start|/*Add path*/|add --path \"C:\\Repos\\github\" --name GithubRepo --tags repo,github|/*Add link to one note document*/|add --onenote \"onenote:///\\\\scb.intra\\users\\H\\USERNAME\\Mina%20Dokument\\OneNote-anteckningsböcker\\möten.one#Mötesanteckningar-id={AC139D67-12D1-485E-AEFA-8B101A4C4F8B}&page-id={9EEDC531-8CB9-4E5E-9A81-F65F98390F1B}&end\" --name Arkitekturmöten --tags arkitektur,teknik")]
public class AddCommand : CommandBase<PowerCommandsConfiguration>
{
    public AddCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var item = new KnowledgeItem(Input);
        Print(item);
        if (!DialogService.YesNoDialog("Is this information correct?")) return CreateRunResult();

        var db = StorageService<KnowledgeDatabase>.Service.GetObject();
        if (db.Items.Any(i => i.Name == item.Name && i.SourceType == i.SourceType))
        {
            WriteLine($"Warning, there is already a item with the same name and source stored in DB");
            return CreateRunResult();
        }
        db.Items.Add(item);
        StorageService<KnowledgeDatabase>.Service.StoreObject(db);
        WriteLine($"Item {item.Name} has successfully been added.");
        return CreateRunResult();
    }

    private void Print(KnowledgeItem item)
    {
        Console.WriteLine($"{nameof(item.Name)}: {item.Name} ");
        Console.WriteLine($"{nameof(item.SourceType)}: {item.SourceType} ");
        Console.WriteLine($"{nameof(item.Uri)}: {item.Uri} ");
        Console.WriteLine($"{nameof(item.Tags)}: {item.Tags} ");
    }
}