using System;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.DomainObjects;

public class KnowledgeTableItem
{
    private string _uri = "";
    private string _tags = "";
    public KnowledgeTableItem(){}
    public int Index { get; set; }
    public string Name { get; set; } = "";
    public string SourceType { get; set; } = "";

    public string Uri
    {
        get => _uri;
        set => _uri = value.Length > 50 ? $"{value.Substring(0, 50)}..." : value;
    }

    public string Tags
    {
        get => _tags;
        set => _tags = value.Length > 70 ? $"{value.Substring(0, 70)}..." : value;
    }
}