namespace WidepollAPI.Models;

public class Comment : DomainEntity, IEquatable<Comment>
{
    public string Body { get; set; }
    public User Author { get; set; }
    public string PostId { get; set; }
    public string ParentCommentId { get; set; }
    public List<string> ReplyIds { get; set; } = new List<string>();
    public List<Like> Likes { get; set; } = new List<Like>();

    public bool Equals(Comment? other)
    {
        if (other is null) return false;
        return (Body is null && other.Body is null || Body == other.Body)
            && (Author is null && other.Author is null || Author.Equals(other.Author))
            && (PostId is null && other.PostId is null || PostId == other.PostId)
            && (ParentCommentId is null && other.ParentCommentId is null || ParentCommentId == other.ParentCommentId)
            && ReplyIds.SequenceEqual(other.ReplyIds)
            && Likes.SequenceEqual(other.Likes);
    }
}