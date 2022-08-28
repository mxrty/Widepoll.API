namespace WidepollAPI.Models;

public class Comment : DomainEntity, IEquatable<Comment>
{
    public string Body { get; set; }
    public User Author { get; set; }
    public string PostId { get; set; }
    public string ParentCommentId { get; set; }
    public string[] ReplyIds { get; set; } = Array.Empty<string>();
    public Like[] Likes { get; set; } = Array.Empty<Like>();

    public bool Equals(Comment? other)
    {
        if (other is null) return false;
        return Body == other.Body
            && Author.Equals(other.Author)
            && PostId == other.PostId
            && ParentCommentId == other.ParentCommentId
            && ReplyIds.SequenceEqual(other.ReplyIds)
            && Likes.SequenceEqual(other.Likes);
    }
}