using System.Text.Json;
using System.Text.Json.Serialization;

namespace SetGame.Models;

public class UserJson
{
    [JsonPropertyName("nickname")] public string Nickname { get; set; }

    [JsonPropertyName("password")] public string Password { get; set; }

    [JsonIgnore]
    [JsonPropertyName("accessToken")]
    public string Token { get; set; } = "";
}

public class TokenDto
{
    [JsonPropertyName("accessToken")] public string AccessToken { get; set; }
}

public class EnterInGameDto
{
    [JsonPropertyName("accessToken")] public string AccessToken { get; set; }
    [JsonPropertyName("gameId")] public int GameId { get; set; }
}

public class PickCardsDto
{
    [JsonPropertyName("accessToken")] public string AccessToken { get; set; }

    [JsonPropertyName("cards")] public int[] Cards { get; set; }
}