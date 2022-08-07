using System.ComponentModel.DataAnnotations;

namespace WidepollAPI.Ports;

public class UserDto
{
    [Required]
    public string Name { get; set; }
}

