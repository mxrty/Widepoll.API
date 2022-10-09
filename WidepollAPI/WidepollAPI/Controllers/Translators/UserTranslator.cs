using WidepollAPI.Models;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers.Translators;

public interface IUserTranslator
{
    public UserDto ToDto(User model);
}

public class UserTranslator : IUserTranslator
{
    public UserDto ToDto(User model)
    {
        if (model is null) return null;

        return new UserDto
        {
            Email = model.Email,
            Id = model.ID
        };
    }
}
