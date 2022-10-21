namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TagsAttribute : Attribute
{
    public TagsAttribute(string tags) => Tags = tags;
    public string Tags { get; }
}