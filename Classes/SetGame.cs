using System.Linq;
using SetGame.Database.Classes;
using static SetGame.Classes.Card;

namespace SetGame.Classes;

public struct User
{
    public int Id;
    public int Score;

    public User(int id)
    {
        Id = id;
        Score = 0;
    }
}

public class SetGame
{
    public int IdGame;
    public List<Card> Cards = new();
    public List<Card> FieldCards = new();
    public List<User> Users = new();

    public SetGame(int idGame)
    {
        GenerateCards();
        IdGame = idGame;
    }

    public void AddUser(int userId)
    {
        Users.Add(new User(userId));
    }
    
    public void AddScoreAllUsers()
    {
        DatabaseHandler handlerDb = new DatabaseHandler();
        foreach (User user in Users)
        {
            handlerDb.AddScoreToUser(user);
        }
    }


    public bool CheckUserInThisGame(int userId)
    {
        if (Users.Any(user => user.Id == userId))
        {
            return true;
        }

        return false;
    }

    private void GenerateCards()
    {
        int cardId = 0;
        for (int color = 1; color <= 3; color++)
        {
            for (int shape = 1; shape <= 3; shape++)
            {
                for (int fill = 1; fill <= 3; fill++)
                {
                    for (int count = 1; count <= 3; count++)
                    {
                        cardId++;
                        Cards.Add(new Card(cardId, color, shape, fill, count));
                    }
                }
            }
        }

        Random rnd = new Random();
        Cards = Cards.OrderBy(x => rnd.Next()).ToList();
        GenerateField();
    }

    private Card GetCardById(int cardId)
    {
        foreach (Card card in FieldCards)
        {
            if (card.Id == cardId)
            {
                return card;
            }
        }

        return new Card(0, 0, 0, 0, 0);
    }

    private void GenerateField()
    {
        while (FieldCards.Count < 12 || FindSet(FieldCards).Count == 0)
        {
            Random rnd = new Random();
            for (int i = 0; i < 12; i++)
            {
                i = rnd.Next(0, Cards.Count);
                if (i >= Cards.Count || FieldCards.Count == 12)
                {
                    break;
                }

                if (FieldCards.Contains(Cards[i]))
                {
                    continue;
                }

                FieldCards.Add(Cards[i]);
            }

            if (FindSet(FieldCards).Count == 0 && FieldCards.Count == 12)
            {
                FieldCards.Clear();
                continue;
            }

            if (Cards.Count == 0)
            {
                break;
            }
        }
    }


    public bool CheckSet(int idCardA, int idCardB, int idCardC)
    {
        Card cardA = GetCardById(idCardA);
        Card cardB = GetCardById(idCardB);
        Card cardC = GetCardById(idCardC);

        if (cardA.Id == 0 || cardB.Id == 0 || cardC.Id == 0)
        {
            return false;
        }

        if (!IsSet(cardA, cardB, cardC))
        {
            return false;
        }

        DeleteSet(cardA, cardB, cardC);
        return true;
    }

    public void DeleteSet(Card a, Card b, Card c)
    {
        Cards.RemoveAll(x => x.Id == a.Id || x.Id == b.Id || x.Id == c.Id);
        if (Cards.Count != 0) ReplaceCardsInField(a, b, c);
    }

    private void ReplaceCardsInField(Card a, Card b, Card c)
    {
        FieldCards.RemoveAll(x => x.Id == a.Id || x.Id == b.Id || x.Id == c.Id);
        GenerateField();
    }

    public int GetCountCards()
    {
        return Cards.Count;
    }

    public int GetCountUsers()
    {
        return Users.Count;
    }
}