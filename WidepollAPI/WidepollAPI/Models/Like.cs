namespace WidepollAPI.Models;

public class Like : DomainEntity
{
    public User Author { get; set; }
    public string PostId { get; set; }
    public string CommentId { get; set; }
}

