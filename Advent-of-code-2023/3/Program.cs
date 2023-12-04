using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] text = File.ReadAllLines(@"../../../input.txt");

        var sum = 0;

        var listOfAsterixIOndexes = new List<AsterixData>();

        for (int y = 0; y < text.Length; y++)
        {
            var line = text[y];

            var numbers = Regex.Matches(line, @"\d+");

            foreach (Match number in numbers)
            {
                var numberStartX = Math.Max(0, number.Index - 1);
                var numberEndX = Math.Min(line.Length - 1, numberStartX + number.Length + 1);

                var numberStartY = Math.Max(0, y - 1);
                var numberEndY = Math.Min(text.Length - 1, y + 1);

                var length = number.Index == 0 ? numberEndX - numberStartX : numberEndX - numberStartX + 1;

                var affectedLines = text.Select(x => x.Substring(numberStartX, length))
                    .Skip(numberStartY)
                    .Take(numberEndY - numberStartY + 1)
                    .ToList();

                var yPos = -1;
                for (int i = 0; i < affectedLines.Count(); i++)
                {
                    var indexes = GetAllAsterixIndexes(affectedLines[i]);

                    foreach (var index in indexes)
                    {
                        listOfAsterixIOndexes.Add(new AsterixData(numberStartX + index, numberStartY + i, int.Parse(number.Value)));
                    }
                }

                var test = new string(text.Select(x => x.Substring(numberStartX, length))
                    .Skip(numberStartY)
                    .Take(numberEndY - numberStartY + 1)
                    .SelectMany(x => x).ToArray());

                if (Regex.IsMatch(test, @"[^a-zA-Z0-9.]"))
                    sum += int.Parse(number.Value);
            }
        }

        Console.WriteLine(sum);

        var x = listOfAsterixIOndexes
            .GroupBy(x => x.Pos)
            .Where(x => x.Count() == 2)
            .Select(x => x.Select(y => y.Ratio).Aggregate(1, (total, next) => total * next))
            .Sum();

        Console.WriteLine(x);


        static List<int> GetAllAsterixIndexes(string str)
        {
            var indexes = new List<int>();
            var index = -1;

            while ((index = str.IndexOf('*', index + 1)) != -1)
            {
                indexes.Add(index);
            }

            return indexes;
        }
    }
}

public class AsterixData
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Ratio { get; set; }

    public string Pos => $"{X}_{Y}";

    public AsterixData(int x, int y, int ratio)
    {
        X = x;
        Y = y;
        Ratio = ratio;
    }
}