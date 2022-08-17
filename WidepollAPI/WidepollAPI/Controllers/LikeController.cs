using Microsoft.AspNetCore.Mvc;
using WidepollAPI.DataAccess;
using WidepollAPI.Models;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers;

[ApiController]
[Route("[controller]s")]
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

    [HttpPost]
    public async Task<ActionResult> CreateLike(string authorId, [FromBody] LikeDto dto)
    {

    }
}
