using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WidepollAPI.Controllers.Authentication;
using WidepollAPI.Controllers.Translators;
using WidepollAPI.DataAccess;
using WidepollAPI.Models;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IDBWriter _writer;
    private readonly IDBReader _reader;
    private readonly IUserTranslator _userTranslator;

    public UserController(IDBReader reader, IDBWriter writer, IUserTranslator userTranslator, ILogger<UserController> logger)
    {
        _writer = writer;
        _reader = reader;
        _logger = logger;
        _userTranslator = userTranslator;
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {

        var result = await _reader.GetByIdAsync<User>(id);
        if (result is null) return NotFound();
        return _userTranslator.ToDto(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] UserCredentialsDto dto)
    {
        CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var newUser = new User { Email = dto.Email, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
        var result = await _writer.InsertAsync(newUser);
        var createdUser = _userTranslator.ToDto(result);
        return Ok(createdUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserCredentialsDto dto)
    {
        var existingUser = await _reader.GetUserByEmailAsync(dto.Email);
        if (existingUser is null) return BadRequest("User not found.");

        if (!VerifyPasswordHash(dto.Password, existingUser.PasswordHash, existingUser.PasswordSalt))
        {
            return BadRequest("Wrong password.");
        }

        var token = CreateToken(existingUser);

        var refreshToken = GenerateRefreshToken();
        await SetRefreshToken(existingUser, refreshToken);

        return Ok(token);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken(string email)
    {
        var existingUser = await _reader.GetUserByEmailAsync(email);
        if (existingUser is null) return BadRequest("User not found.");

        var refreshToken = Request.Cookies["refreshToken"];

        if (!existingUser.RefreshToken.Equals(refreshToken))
        {
            return Unauthorized("Invalid Refresh Token.");
        }
        else if (existingUser.TokenExpires < DateTime.Now)
        {
            return Unauthorized("Token expired.");
        }

        string token = CreateToken(existingUser);
        var newRefreshToken = GenerateRefreshToken();
        await SetRefreshToken(existingUser, newRefreshToken);

        return Ok(token);
    }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }

    private async Task SetRefreshToken(User user, RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;

        await _writer.InsertAsync(user);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("my top secret key"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        //TODO: What is this
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
