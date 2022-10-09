using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WidepollAPI.DataAccess;
using WidepollAPI.Models;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers;

[ApiController]
[Route("likes")]
public class LikeController : ControllerBase
{
    private readonly ILogger<LikeController> _logger;
    private readonly IDBWriter _writer;
    private readonly IDBReader _reader;

    public LikeController(IDBReader reader, IDBWriter writer, ILogger<LikeController> logger)
    {
        _writer = writer;
        _reader = reader;
        _logger = logger;
    }

    [HttpPost, Authorize]
    public async Task<ActionResult> CreateLike([FromBody] LikeDto dto)
    {
        if (dto.PostId is null && dto.CommentId is null) return BadRequest($"Comment and Post cannot both be null");
        if (dto.PostId is not null && dto.CommentId is not null) return BadRequest($"Like cannot belong to both a post and a comment");

        var authorId = dto.AuthorId;
        var user = await _reader.GetByIdAsync<User>(authorId);
        if (user is null) return BadRequest($"User {authorId} was not found. Must be a valid user.");

        Comment? parentComment = null;
        if (dto.CommentId is not null)
        {
            parentComment = await _reader.GetByIdAsync<Comment>(dto.CommentId);
            if (parentComment is null) return BadRequest($"Comment {dto.CommentId} was not found. Must be a valid comment.");
        }

        Post? parentPost = null;
        if (dto.PostId is not null)
        {
            parentPost = await _reader.GetByIdAsync<Post>(dto.PostId);
            if (parentPost is null) return BadRequest($"Post {dto.PostId} was not found. Must be a valid post.");
        }

        var like = new Like
        {
            Author = user,
            PostId = parentPost?.ID,
            CommentId = parentComment?.ID
        };

        var writtenLike = await _writer.InsertAsync(like);

        if(parentPost is not null)
        {
            await _writer.AddLikeToPostAsync(parentPost, writtenLike);
        }
        else if (parentComment is not null)
        {
            await _writer.AddLikeToCommentAsync(parentComment, writtenLike);
        }

        return Ok(writtenLike);
    }
}
