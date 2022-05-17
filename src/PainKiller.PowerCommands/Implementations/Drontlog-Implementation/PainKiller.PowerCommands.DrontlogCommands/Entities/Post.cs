#nullable disable

namespace PainKiller.PowerCommands.DrontlogCommands.Entities
{
    public class Post
    {
        public Guid PostID { get; set; }
        public Guid ProviderServiceID { get; set; }
        public Guid OwnerProviderServiceID { get; set; }
        public string ProviderSpecificID { get; set; }
        public DateTime ProviderSpecificCreated { get; set; }
        public string Caption { get; set; }
        public string Summary { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public short PublishStatusID { get; set; }
        public string UrlToLogo { get; set; }
        public Guid? ProfileID { get; set; }
        public int CommentCount { get; set; }
    }
}