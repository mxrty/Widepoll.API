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
        return Author.Equals(other.Author) && Left == other.Left && Link == other.Link && Right == other.Right;
    }
}

