using System.ComponentModel.DataAnnotations;

namespace WidepollAPI.Ports;

public class StatementDto
{
    [Required]
    public string Left { get; set; }
    [Required]
    public string Link { get; set; }
    [Required]
    public string Right { get; set; }
}

