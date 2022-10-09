using System.ComponentModel.DataAnnotations;

namespace WidepollAPI.Ports;

public class CommentDto
{
    [Required]
    public string AuthorId { get; set; }
    public string? PostId { get; set; }
    public string? ParentCommentId { get; set; }
    [Required]
    public string Body { get; set; }
}

