using System.ComponentModel.DataAnnotations;

namespace WidepollAPI.Ports;

public class CommentDto
{
    public string? PostId { get; set; }
    public string? ParentCommentId { get; set; }
    [Required]
    public string Body { get; set; }
}

