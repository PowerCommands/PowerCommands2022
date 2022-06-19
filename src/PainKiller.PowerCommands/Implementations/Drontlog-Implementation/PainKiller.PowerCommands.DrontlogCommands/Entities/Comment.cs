namespace PainKiller.PowerCommands.DrontlogCommands.Entities;

public class Comment
{
    public Guid CommentID { get; set; }
    public Guid PostID { get; set; }
    public Guid ProfileID { get; set; }
    public string MainBody { get; set; }
    public DateTime Created { get; set; }
    public short PublishStatusID { get; set; }
    public Guid? CommentToProfileID { get; set; }
}