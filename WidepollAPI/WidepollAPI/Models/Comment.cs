namespace WidepollAPI.Models;

public class Comment : DomainEntity
{
    public string Body { get; set; }
    public User Author { get; set; }
    public string PostId { get; set; }
    public string ParentCommentId { get; set; }
    public string[] ReplyIds { get; set; } = Array.Empty<string>();
    public Like[] Likes { get; set; } = Array.Empty<Like>();
}

