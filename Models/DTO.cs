using System.Text.Json;
using System.Text.Json.Serialization;

namespace SetGame.Models;

public class UserJSON
{
    [JsonPropertyName("nickname")] public string Nickname { get; set; }

    [JsonPropertyName("password")] public string Password { get; set; }

    [JsonIgnore]
    [JsonPropertyName("accessToken")]
    public string Token { get; set; } = "";
}