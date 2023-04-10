using SQLite;

namespace SetGame.Database.Classes;

public class DatabaseHandler
{
    private SQLiteConnection _db;

    public DatabaseHandler()
    {
        _db = new SQLiteConnection("Database/data.db");
        _db.CreateTable<Users>();
    }

    public bool CheckNicknameUserExists(string nickname)
    {
        List<Users> usersList = _db.Query<Users>($"SELECT * FROM Users WHERE nickname = '{nickname}' LIMIT 1");

        foreach (Users user in usersList)
        {
            if (user.Nickname == nickname) return true;
        }

        return false;
    }


    public void Adduser(string nickname, string password, string token)
    {
        Users user = new Users(nickname, password, token);
        _db.Insert(user);
    }
}