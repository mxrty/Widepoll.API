namespace WidepollAPI.Models;

public class User : DomainEntity, IEquatable<User>
{
    public string Name { get; set; }

    public bool Equals(User? other)
    {
        if (other is null) return false;
        return (Name is null && other.Name is null || Name == other.Name);
    }
}

