using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SetGame.Classes;
using SetGame.Database.Classes;
using SetGame.Models;

namespace SetGame.Controllers
{
    [Route("set/")]
    [ApiController]
    public class InGameController : ControllerBase
    {
        [Route("field")]
        [HttpGet]
        public IActionResult GetField(TokenDto model)
        {
            DatabaseHandler handlerDb = new DatabaseHandler();
            if (!handlerDb.CheckTokenUserExists(model.AccessToken))
            {
                var resp = new
                {
                    success = false,
                    exception = EnumExtensions.ToString(EnumExtensions.Excepеtions.IncorectAuthorization)
                };
                return Ok(resp);
            }
            else
            {
                // TODO : выдать пользователю поле из игры в которой он сидит и только те карты, которые сейчас на поле (сделать проверку что среди них есть сет)
                int userId = handlerDb.GetUserIdByToken(model.AccessToken);
                Classes.SetGame? userRoom =
                    Rooms.AllRooms.FirstOrDefault(room => room != null && room.Users.Any(user => user.Id == userId));
                var resp = new
                {
                    cards = userRoom?.FieldCards
                };
                return Ok(resp);
            }
        }

        // POST: set/pick
        [Route("pick")]
        [HttpPost]
        public IActionResult Post(PickCardsDto model)
        {
            DatabaseHandler handlerDb = new DatabaseHandler();
            if (!handlerDb.CheckTokenUserExists(model.AccessToken))
            {
                var resp = new
                {
                    success = false,
                    exception = EnumExtensions.ToString(EnumExtensions.Excepеtions.IncorectAuthorization)
                };
                return Ok(resp);
            }
            else if (model.Cards.Length != 3)
            {
                var resp = new
                {
                    success = false,
                    exception = EnumExtensions.ToString(EnumExtensions.Excepеtions.IncorectCountCards)
                };
                return Ok(resp);
            }
            else
            {
                // TODO : нужна проверка по id карт в field
                int userId = handlerDb.GetUserIdByToken(model.AccessToken);
                Classes.SetGame? userRoom =
                    Rooms.AllRooms.FirstOrDefault(room => room != null && room.Users.Any(user => user.Id == userId));

                User oldUser = userRoom.Users.Find(user => user.Id == userId);
                User newUser = oldUser;


                if (userRoom.CheckSet(model.Cards[0], model.Cards[1], model.Cards[2]))
                {
                    newUser.Score += 3;
                    userRoom.Users[userRoom.Users.IndexOf(oldUser)] = newUser;
                    var resp = new
                    {
                        success = false,
                        exception = "null",
                        isSet = true,
                        score = newUser.Score
                    };
                    return Ok(resp);
                }
                if (userRoom != null &&
                         userRoom.CheckSet(model.Cards[0], model.Cards[1], model.Cards[2]) == false &&
                         userRoom.Cards.Count < 12 && Card.FindSet(userRoom.FieldCards).Count == 0)
                {
                    var resp = new
                    {
                        success = false,
                        exception = "null",
                        message = "Game ended."
                    };
                    userRoom.AddScoreAllUsers();
                    Rooms.AllRooms.RemoveAll(game => game.IdGame == userRoom.IdGame);
                    return Ok(resp);
                }
                else
                {
                    newUser.Score -= 1;
                    userRoom.Users[userRoom.Users.IndexOf(oldUser)] = newUser;
                    var resp = new
                    {
                        success = false,
                        exception = "null",
                        isSet = false,
                        score = newUser.Score
                    };
                    return Ok(resp);
                }
            }
        }
    }
}