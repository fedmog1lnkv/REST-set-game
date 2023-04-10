using SQLite;

namespace SetGame.Database.Classes;

[Table("Users")]
public class Users
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("nickname"), UniqueAttribute] public string Nickname { get; set; }

    [Column("password")] public string Password { get; set; }

    [Column("token")] public string Token { get; set; }

    public Users(string nickname, string password, string token)
    {
        Nickname = nickname;
        Password = password;
        Token = token;
    }

    public Users()
    {
    }
}