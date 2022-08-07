using System;
namespace WidepollAPI.Ports
{
    public class CommentDto
    {
        public string? PostId { get; set; }
        public string? ParentCommentId { get; set; }
        public string Body { get; set; }
    }
}

