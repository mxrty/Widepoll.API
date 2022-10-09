using System.ComponentModel.DataAnnotations;

namespace WidepollAPI.Ports;

public class LikeDto
{
    [Required]
    public string AuthorId { get; set; }
    public string? PostId { get; set; }
    public string? CommentId { get; set; }
}
