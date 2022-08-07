using Microsoft.AspNetCore.Mvc;
using WidepollAPI.DataAccess;
using WidepollAPI.Models;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly IDBWriter _writer;
        private readonly IDBReader _reader;

        public CommentController(IDBReader reader, IDBWriter writer, ILogger<CommentController> logger)
        {
            _writer = writer;
            _reader = reader;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Comment>> GetComment(string id)
        {

            var result = await _reader.GetUser(id);
            if (result is null) return NotFound();
            return result;
        }

        [HttpPut]
        public async Task<ActionResult> CreateComment(string authorId, [FromBody] CommentDto dto)
        {
            if (dto.ParentCommentId is null && dto.PostId is null) return BadRequest($"Comment must have a parent");
            if (dto.ParentCommentId is not null && dto.PostId is not null) return BadRequest($"Comment can only have one parent");

            var user = await _reader.GetUser(authorId);
            if (user is null) return BadRequest($"User {authorId} was not found. Must be a valid user.");

            Comment? parentComment = null;
            if (dto.ParentCommentId is not null)
            {
                parentComment = await _reader.GetComment(dto.ParentCommentId);
                if (parentComment is null) return BadRequest($"Comment {dto.ParentCommentId} was not found. Must be a valid comment.");
            }

            Post? parentPost = null;
            if (dto.PostId is not null)
            {
                parentPost = await _reader.GetPost(dto.PostId);
                if (parentPost is null) return BadRequest($"Post {dto.PostId} was not found. Must be a valid post.");
            }

            var comment = new Comment
            {
                Author = user,
                Body = dto.Body,
                ParentCommentId = parentComment.Id,
                PostId = parentPost.Id
            };

            //Return written document
            var writtenComment = await _writer.Insert(comment);

            if (parentComment is not null)
            {
                _writer.AddToParent(parentComment, writtenComment.Id);
            }

            return Ok();
        }
    }
}