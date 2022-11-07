using System;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.DomainObjects;

public class KnowledgeTableItem
{
    private string _uri = "";
    public KnowledgeTableItem(){}
    public KnowledgeTableItem(string uri) => _uri = uri.Length > 50 ? $"{uri.Substring(0, 40).Length}..." : uri;
    public int Index { get; set; }
    public string Name { get; set; } = "";
    public string SourceType { get; set; } = "";

    public string Uri
    {
        get => _uri;
        set => _uri = value.Length > 50 ? $"{value.Substring(0, 40).Length}..." : value;
    }

    public string Tags { get; set; } = "";
}