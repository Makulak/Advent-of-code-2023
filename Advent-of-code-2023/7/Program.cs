internal class Program
{
    private static void Main(string[] args)
    {
        var game = new CamelGame();
        string[] text = File.ReadAllLines(@"../../../input.txt");

        foreach (string line in text)
        {
            var cards = line.Split(" ").First();
            var bidValue = int.Parse(line.Split(" ").Last());

            game.Hands.Add(new Hand(cards, bidValue));
        }

        Console.WriteLine(game.GetResult());
    }
}

public enum HandType
{
    HighCard,
    OnePair,
    TwoPairs,
    Three,
    Full,
    Four,
    Five
}

public class CamelGame
{
    public List<Hand> Hands { get; set; }
    public CamelGame()
    {
        Hands = new();
    }

    public int GetResult()
    {
        var test = Hands
            .Order()
            .Select((x, idx) => new { Rank = idx + 1, Hand = x })
            .ToList();
        return Hands
            .Order()
            .Select((x, idx) => new { Rank = idx + 1, Hand = x })
            .Aggregate(0, (sum, element) => sum += element.Rank * element.Hand.BetValue);
    }
}

public class Hand : IComparable<Hand>
{
    public string Cards { get; set; }
    public int BetValue { get; set; }

    public Hand(string cards, int betValue)
    {
        Cards = cards;
        BetValue = betValue;
    }

    private List<int> GetCardValues()
    {
        return Cards.Select(GetCardValue).ToList();
    }

    private int GetCardValue(char card)
    {
        var map = new Dictionary<string, int>()
            {
                { "T", 10 },
                { "J", 1 },
                { "Q", 12 },
                { "K", 13 },
                { "A", 14 },
            };
        if (int.TryParse(card.ToString(), out var num))
            return num;

        return map[card.ToString()];
    }

    public string GetBestHandWithJockers()
    {
        if (!Cards.Contains("J") || Cards.All(x => x == 'J'))
            return Cards;

        var temp = new String(Cards
            .Where(x => x != 'J')
            .GroupBy(x => x)
            .OrderByDescending(x => x.Count())
            .ThenBy(x => GetCardValue(x.First()))
            .First()
            .Select(x => x)
            .Take(1)
            .ToArray());

        var strongestPossibleHand = Cards.Replace("J", temp);

        return strongestPossibleHand;
    }

    public HandType GetHandType()
    {
        //var groupped = Cards.GroupBy(x => x);
        var groupped = GetBestHandWithJockers().GroupBy(x => x);

        if (groupped.Count() == 1)
            return HandType.Five;
        else if (groupped.Any(x => x.Count() == 4))
            return HandType.Four;
        else if (groupped.Any(x => x.Count() == 3))
        {
            if (groupped.Any(x => x.Count() == 2))
                return HandType.Full;

            return HandType.Three;
        }
        else if (groupped.Where(x => x.Count() == 2).Count() == 2)
            return HandType.TwoPairs;
        else if (groupped.Where(x => x.Count() == 2).Count() == 1)
            return HandType.OnePair;

        return HandType.HighCard;
    }

    public int CompareTo(Hand? other)
    {
        var currentHandType = GetHandType();
        var otherHandType = other.GetHandType();

        if (currentHandType == otherHandType)
        {
            var currentCardValues = GetCardValues();
            var otherCardValues = other.GetCardValues();

            for (int i = 0; i < currentCardValues.Count(); i++)
            {
                var compareResult = currentCardValues[i].CompareTo(otherCardValues[i]);

                if(compareResult != 0)
                    return compareResult;
            }
            return 0;
        }

        return currentHandType.CompareTo(otherHandType);
    }

    public override string ToString()
    {
        return Cards;
    }
}