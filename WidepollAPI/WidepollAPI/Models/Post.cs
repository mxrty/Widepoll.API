namespace WidepollAPI.Models;

public class Post : DomainEntity, IEquatable<Post>
{
    public User Author { get; set; }
    public Statement Statement { get; set; }
    public List<Like> Likes { get; set; } = new List<Like>();

    public bool Equals(Post? other)
    {
        if (other is null) return false;
        return (Author is null && other.Author is null || Author.Equals(other.Author))
            && (Statement is null && other.Statement is null || Statement.Equals(other.Statement))
            && Likes.SequenceEqual(other.Likes);
    }
}

