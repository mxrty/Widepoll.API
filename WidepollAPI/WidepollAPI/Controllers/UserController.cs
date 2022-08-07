using Microsoft.AspNetCore.Mvc;
using WidepollAPI.DataAccess;
using WidepollAPI.Models;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers;

[ApiController]
[Route("[controller]s")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IDBWriter _writer;
    private readonly IDBReader _reader;

    public UserController(IDBReader reader, IDBWriter writer, ILogger<UserController> logger)
    {
        _writer = writer;
        _reader = reader;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<User>> GetUser(string id)
    {

        var result = await _reader.GetUser(id);
        if (result is null) return NotFound();
        return result;
    }

    [HttpPut]
    public async Task<ActionResult> CreateUser ([FromBody] UserDto dto)
    {
        var user = new User { Name = dto.Name };
        var createdUser = await _writer.InsertAsync(user);
        return Ok(createdUser.Id);
    }
}
