using System.ComponentModel.DataAnnotations;

namespace WidepollAPI.Ports;

public class UserCredentialsDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
