namespace WidepollAPI.Models;

public class Like : DomainEntity, IEquatable<Like>
{
    public User Author { get; set; }
    public string PostId { get; set; }
    public string CommentId { get; set; }

    public bool Equals(Like? other)
    {
        if (other is null) return false;
        return Author.Equals(other.Author) && PostId == other.PostId && CommentId == other.CommentId;
    }
}

