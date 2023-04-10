namespace SetGame.Models;
public static class EnumExtensions
{
    public enum Excepеtions
    {
        NickinameAlreadyExists,
        IncorectAuthorization,
    }
    public static string ToString(this Excepеtions value)
    {
        switch(value)
        {
            case Excepеtions.NickinameAlreadyExists:
                return "Nickiname already exists";
            case Excepеtions.IncorectAuthorization:
                return "Nickname or password is incorrect";
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
    }
}
