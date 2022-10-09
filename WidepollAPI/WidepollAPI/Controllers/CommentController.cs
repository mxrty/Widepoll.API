using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WidepollAPI.DataAccess;
using WidepollAPI.Models;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers;

[ApiController]
[Route("comments")]
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
        var result = await _reader.GetByIdAsync<Comment>(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("Post")]
    public async Task<ActionResult<IReadOnlyCollection<Comment>>> GetCommentsForPost(string id)
    {
        var results = await _reader.GetCommentsByParentPostIdAsync(id);
        if (results is null || !results.Any()) return NotFound();
        return Ok(results);
    }

    [HttpPut, Authorize]
    public async Task<ActionResult> CreateComment([FromBody] CommentDto dto)
    {
        if (dto.ParentCommentId is null && dto.PostId is null) return BadRequest($"Comment must have a parent");
        if (dto.ParentCommentId is not null && dto.PostId is not null) return BadRequest($"Comment can only have one parent");

        var authorId = dto.AuthorId;
        var user = await _reader.GetByIdAsync<User>(authorId);
        if (user is null) return BadRequest($"User {authorId} was not found. Must be a valid user.");

        Comment? parentComment = null;
        if (dto.ParentCommentId is not null)
        {
            parentComment = await _reader.GetByIdAsync<Comment>(dto.ParentCommentId);
            if (parentComment is null) return BadRequest($"Comment {dto.ParentCommentId} was not found. Must be a valid comment.");
        }

        Post? parentPost = null;
        if (dto.PostId is not null)
        {
            parentPost = await _reader.GetByIdAsync<Post>(dto.PostId);
            if (parentPost is null) return BadRequest($"Post {dto.PostId} was not found. Must be a valid post.");
        }

        var comment = new Comment
        {
            Author = user,
            Body = dto.Body,
            ParentCommentId = parentComment?.ID,
            PostId = parentPost?.ID
        };

        var writtenComment = await _writer.InsertAsync(comment);

        if (parentComment is not null)
        {
            await _writer.AddCommentIdToParentCommentAsync(parentComment, writtenComment.ID);
        }

        return Ok();
    }
}
