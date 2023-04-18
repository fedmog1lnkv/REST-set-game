namespace SetGame.Models;
public static class EnumExtensions
{
    public enum Excepеtions
    {
        NickinameAlreadyExists,
        IncorectAuthorization,
        GameNotExists,
        UserAlreadyInGame,
        IncorectCountCards,
        UnableAddCard
    }
    public static string ToString(this Excepеtions value)
    {
        switch(value)
        {
            case Excepеtions.NickinameAlreadyExists:
                return "Nickiname already exists.";
            case Excepеtions.IncorectAuthorization:
                return "Token is incorrect. Authorization failed.";
            case Excepеtions.GameNotExists:
                return "A game with this Id does not exist.";
            case Excepеtions.UserAlreadyInGame:
                return "You are already in game.";
            case Excepеtions.IncorectCountCards:
                return "Few/many cards selected (need 3).";
            case Excepеtions.UnableAddCard:
                return "Unable to add card (field contains 21 cards or game ended)";
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
    }
}
