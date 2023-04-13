using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SetGame.Models;
using SetGame.Database.Classes;


[Route("set/user/register")]
[ApiController]
public class RegisterController : ControllerBase
{
    [HttpPost]
    public IActionResult Register(UserJson model)
    {
        DatabaseHandler handlerDb = new DatabaseHandler();

        if (handlerDb.CheckNicknameUserExists(model.Nickname))
        {
            var resp = new
            {
                success = false,
                exception = EnumExtensions.ToString(EnumExtensions.Excepеtions.NickinameAlreadyExists)
            };
            return Ok(resp);
        }
        else
        {
            var resp = new
            {
                success = true,
                exception = "null",
                nickname = model.Nickname,
                accessToken = TokenGenerator.GenerateToken()
            };
            handlerDb.AddUser(model.Nickname, model.Password, resp.accessToken);
            return Ok(resp);
        }
    }
}