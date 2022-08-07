namespace WidepollAPI.Models;

public class Statement : DomainEntity
{
    public User Author { get; set; }
    public string Left { get; set; }
    public string Link { get; set; }
    public string Right { get; set; }
}

