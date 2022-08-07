using Microsoft.AspNetCore.Mvc;
using WidepollAPI.DataAccess;
using WidepollAPI.Models;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers;

[ApiController]
[Route("[controller]s")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IDBWriter _writer;
    private readonly IDBReader _reader;

    public PostController(IDBReader reader, IDBWriter writer, ILogger<PostController> logger)
    {
        _writer = writer;
        _reader = reader;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<Post>> GetPost(string id)
    {
        var result = await _reader.GetByIdAsync<Post>(id);
        if (result is null) return NotFound();
        return result;
    }

    [HttpGet("Recent")]
    public async Task<ActionResult<Post[]>> GetMostRecentPosts(int quantity)
    {
        if (quantity < 1) return BadRequest($"{quantity} is not a valid number of posts to get. Must be atleast 1.");
        if (quantity > 100) return BadRequest($"{quantity} is too many. Max is 100.");

        var posts = _reader.GetRecentPosts(quantity);

        return Ok(posts);
    }

    [HttpPut]
    public async Task<ActionResult> CreatePost(string authorId, [FromBody] PostDto dto)
    {
        var user = await _reader.GetByIdAsync<User>(authorId);
        if (user is null) return BadRequest($"User {authorId} was not found. Must be a valid user.");

        var statement = new Statement
        {
            Author = user,
            Left = dto.Statement.Left,
            Link = dto.Statement.Link,
            Right = dto.Statement.Right
        };

        await _writer.InsertAsync(statement);

        var post = new Post { Author = user, Statement = statement };

        await _writer.InsertAsync(post);

        return Ok(post.ID);
    }
}
