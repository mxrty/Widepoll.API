namespace WidepollAPI.Models;

public class Post : DomainEntity
{
    public User Author { get; set; }
    public Statement Statement { get; set; }
    public Like[] Likes { get; set; } = Array.Empty<Like>();
}

