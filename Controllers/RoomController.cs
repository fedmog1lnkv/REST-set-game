using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SetGame.Classes;
using SetGame.Database.Classes;
using SetGame.Models;

namespace SetGame.Controllers
{
    [Route("set/room/")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        public IActionResult CreateGame(TokenDto model)
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
                int newGameId = 0;
                if (Rooms.AllRooms.Count != 0)
                {
                    newGameId = Rooms.AllRooms[^1].IdGame + 1;
                }

                Classes.SetGame? newGame = new Classes.SetGame(newGameId);
                Rooms.AllRooms.Add(newGame);
                var resp = new
                {
                    success = true,
                    exception = "null",
                    gameId = newGameId
                };
                return Ok(resp);
            }
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetAllGames(TokenDto model)
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
                var gamesList = new List<object>();
                foreach (var game in Rooms.AllRooms)
                {
                    var gameObject = new
                    {
                        id = game.IdGame,
                        countCards = game.GetCountCards(),
                        countUsers = game.GetCountUsers(),
                        userIds = game.Users.Select(u => u.Id).ToArray()
                    };
                    gamesList.Add(gameObject);
                }

                var resp = new
                {
                    success = true,
                    exception = "null",
                    games = gamesList
                };
                return Ok(resp);
            }
        }


        [HttpPost]
        [Route("list/enter")]
        public IActionResult EnterInGame(EnterInGameDto model)
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

            if (!CheckIdGameExists(model.GameId))
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
                int userId = handlerDb.GetUserIdByToken(model.AccessToken);
                var userExistsInAnyRoom = Rooms.AllRooms.Any(room => room.Users.Any(user => user.Id == userId));
                // Rooms.AllRooms.Find(room => room.IdGame == model.GameId).CheckUserInThisGame(userId)
                if (userExistsInAnyRoom)
                {
                    var resp = new
                    {
                        success = false,
                        exception = EnumExtensions.ToString(EnumExtensions.Excepеtions.UserAlreadyInGame)
                    };
                    return Ok(resp);
                }
                else
                {
                    Rooms.AllRooms.Find(room => room.IdGame == model.GameId)
                        .AddUser(userId);
                    var resp = new
                    {
                        success = true,
                        exception = "null",
                        gameId = model.GameId
                    };
                    return Ok(resp);
                }
            }
        }

        bool CheckIdGameExists(int gameId)
        {
            foreach (var game in Rooms.AllRooms)
            {
                if (game.IdGame == gameId) return true;
            }

            return false;
        }
    }
}