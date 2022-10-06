namespace WidepollAPI.Models;

public class Statement : DomainEntity, IEquatable<Statement>
{
    public User Author { get; set; }
    public string Left { get; set; }
    public string Link { get; set; }
    public string Right { get; set; }

    public bool Equals(Statement? other)
    {
        if (other is null) return false;
        return (Author is null && other.Author is null || Author.Equals(other.Author))
            && (Left is null && other.Left is null || Left == other.Left)
            && (Link is null && other.Link is null || Link == other.Link)
            && (Right is null && other.Right is null || Right == other.Right);
    }
}

