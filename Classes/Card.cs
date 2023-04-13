using System.Text.Json.Serialization;

namespace SetGame.Classes;

public class Card
{
    public int Id { get; set; }
    public int Count { get; set; }
    public int Color { get; set; }
    public int Shape { get; set; }
    public int Fill { get; set; }
    [JsonIgnore] public int[] Attributes { get; set; }

    public Card(int id, int count, int color, int shape, int fill)
    {
        Id = id;
        Count = count;
        Color = color;
        Shape = shape;
        Fill = fill;
        NormalizeCard();
    }

    public void NormalizeCard()
    {
        Attributes = new int[4] { Count, Color, Shape, Fill };
    }

    public static List<List<Card>> FindSet(List<Card> cards)
    {
        List<List<Card>> res = new List<List<Card>>();

        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = i + 1; j < cards.Count; j++)
            {
                for (int k = j + 1; k < cards.Count; k++)
                {
                    if (IsSet(cards[i], cards[j], cards[k]))
                    {
                        res.Add(new List<Card> { cards[i], cards[j], cards[k] });
                    }
                }
            }
        }

        return res;
    }

    public static bool IsSet(Card a, Card b, Card c)
    {
        {
            for (int i = 0; i < a.Attributes.Length; i++)
            {
                if (!(AllSame(a.Attributes[i], b.Attributes[i], c.Attributes[i]) ||
                      AllDifferent(a.Attributes[i], b.Attributes[i], c.Attributes[i])))
                {
                    return false;
                }
            }

            return true;
        }
    }

    private static bool AllSame(int a, int b, int c)
    {
        return a == b && b == c;
    }

    private static bool AllDifferent(int a, int b, int c)
    {
        return a != b && a != c && b != c;
    }
}