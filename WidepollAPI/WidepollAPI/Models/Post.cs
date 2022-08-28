namespace WidepollAPI.Models;

public class Post : DomainEntity, IEquatable<Post>
{
    public User Author { get; set; }
    public Statement Statement { get; set; }
    public Like[] Likes { get; set; } = Array.Empty<Like>();

    public bool Equals(Post? other)
    {
        if (other is null) return false;
        return Author.Equals(other.Author) && Statement.Equals(other.Statement) && Likes.SequenceEqual(other.Likes);
    }
}

