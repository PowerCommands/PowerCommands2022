#nullable disable

using PainKiller.Data.SqlExtension.DomainObjects;

namespace PainKiller.PowerCommands.DrontlogCommands.Entities
{
    [TableMetadata(nameof(PostContent),nameof(PostID))]
    public class PostContent
    {
        public Guid PostID { get; set; }
        public string MainBody { get; set; }
        public string Tags { get; set; }
    }
}