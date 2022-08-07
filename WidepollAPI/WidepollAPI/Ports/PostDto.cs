using System.ComponentModel.DataAnnotations;

namespace WidepollAPI.Ports;

public class PostDto
{
    [Required]
    public StatementDto Statement { get; set; }
}

