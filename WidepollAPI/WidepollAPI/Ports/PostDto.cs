using System.ComponentModel.DataAnnotations;

namespace WidepollAPI.Ports;

public class PostDto
{
    [Required]
    public string AuthorId { get; set; }
    [Required]
    public StatementDto Statement { get; set; }
}

