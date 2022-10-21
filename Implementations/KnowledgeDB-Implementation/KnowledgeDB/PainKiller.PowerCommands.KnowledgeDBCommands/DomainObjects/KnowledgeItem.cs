using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.KnowledgeDBCommands.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.DomainObjects;

public class KnowledgeItem
{
    public KnowledgeItem(){}

    public KnowledgeItem(ICommandLineInput input)
    {
        ItemID = Guid.NewGuid();
        Name = input.GetFlagValue(nameof(Name).ToLower());
        SourceType = input.ToSourceType();
        Uri = input.SingleQuote;
        Tags = input.GetFlagValue(nameof(Tags).ToLower());
    }
    public Guid? ItemID { get; set; }
    public string Name { get; set; } = "";
    public string SourceType { get; set; } = "";
    public string Uri { get; set; } = "";
    public DateTime Created { get; set; } = DateTime.Now;
    public string Tags { get; set; } = "";
}