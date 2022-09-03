namespace WidepollAPI.Models;

public class User : DomainEntity, IEquatable<User>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }

    public bool Equals(User? other)
    {
        if (other is null) return false;
        return (Name is null && other.Name is null || Name == other.Name);
    }
}

