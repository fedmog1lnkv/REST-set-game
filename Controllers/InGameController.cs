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
                int userId = handlerDb.GetUserIdByToken(model.AccessToken);
                Classes.SetGame? userRoom =
                    Rooms.AllRooms.FirstOrDefault(room => room != null && room.Users.Any(user => user.Id == userId));

                var resp = new
                {
                    status = userRoom?.Status,
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

            if (model.Cards.Length != 3)
            {
                var resp = new
                {
                    success = false,
                    exception = EnumExtensions.ToString(EnumExtensions.Excepеtions.IncorectCountCards)
                };
                return Ok(resp);
            }

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

            if (userRoom.Status == "ended")
            {
                var resp = new
                {
                    success = false,
                    exception = "null",
                    message = "Game ended."
                };
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

        // POST: set/add
        [Route("add")]
        [HttpPost]
        public IActionResult AddCard(TokenDto model)
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

            int userId = handlerDb.GetUserIdByToken(model.AccessToken);
            Classes.SetGame? userRoom =
                Rooms.AllRooms.FirstOrDefault(room => room != null && room.Users.Any(user => user.Id == userId));

            if (!userRoom.AddCard())
            {
                var resp = new
                {
                    success = false,
                    exception = EnumExtensions.ToString(EnumExtensions.Excepеtions.GameNotExists)
                };
                return Ok(resp);
            }
            else
            {
                var resp = new
                {
                    success = true,
                    exception = "null"
                };
                return Ok(resp);
            }
        }
    }
}