namespace WidepollAPI.Models;

public class Like : DomainEntity
{
    public string AuthorId { get; set; }
    public string PostId { get; set; }
    public string CommentId { get; set; }
}

