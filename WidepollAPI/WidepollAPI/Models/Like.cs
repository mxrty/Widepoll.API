namespace WidepollAPI.Models;

public class Like : DomainEntity, IEquatable<Like>
{
    public User Author { get; set; }
    public string PostId { get; set; }
    public string CommentId { get; set; }

    public bool Equals(Like? other)
    {
        if (other is null) return false;
        return (Author is null && other.Author is null || Author.Equals(other.Author))
            && (PostId is null && other.PostId is null || PostId == other.PostId)
            && (CommentId is null && other.CommentId is null || CommentId == other.CommentId);
    }
}

