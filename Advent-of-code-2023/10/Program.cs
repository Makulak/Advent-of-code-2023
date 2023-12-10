using System.Reflection.PortableExecutable;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] text = File.ReadAllLines(@"../../../input.txt");

        var map = new List<string>();

        List<(int, int)> loopPos = new();
        List<(int, int)> przesmykPos = new();

        foreach (string line in text)
        {
            map.Add(line);
            var przesmyksStrings = new List<string>() { "||", "|L", "7|", "|F", "J|" };

            przesmyksStrings.ForEach(x =>
            {
                przesmykPos.AddRange(AllIndexesOf(line, x).Select(x => (x, map.Count)));
            });
        }

        var map2 = new List<string>();
        foreach (var przesmyk in przesmykPos.GroupBy(x => x.Item1))
        {
            foreach (string line in map)
            {
                var charToReplace = map[map2.Count][przesmyk.First().Item1 + 1] == '|' ? "." : "-";
                map2.Add(line.Insert(przesmyk.First().Item1 + 1, charToReplace));
            }
            map.Clear();
            map.AddRange(map2);
            map2.Clear();
        }

        var xPos = map.Single(x => x.Contains("S")).IndexOf("S");
        var yPos = map.IndexOf(map.Single(x => x.Contains("S")));

        int prevX = xPos;
        int prevY = yPos;
        var currVal = map[yPos][xPos];

        int steps = 0;

        while (currVal != 'S' || steps == 0)
        {
            var direction = GetNextStep(map, xPos, yPos, prevX, prevY);
            prevX = xPos;
            prevY = yPos;
            if (direction == Direction.Up)
                yPos--;
            else if (direction == Direction.Down)
                yPos++;
            else if (direction == Direction.Left)
                xPos--;
            else
                xPos++;

            currVal = map[yPos][xPos];
            steps++;
            loopPos.Add((xPos, yPos));
        }

        //Console.WriteLine(steps / 2); // End of part1

        var insideCounter = 0;
        var przesmyksX = przesmykPos.Select(x => x.Item1).GroupBy(x => x).Select(x => x.First()).ToList();

        for (int y = 0; y < text.Length; y++)
        {
            for (int x = 0; x < text[y].Length; x++)
            {
                if (IsInside(map, loopPos, przesmyksX, x, y))
                {
                    Console.WriteLine($"{x} - {y}");
                    insideCounter++;
                }
            }
        }
        Console.WriteLine(insideCounter);
    }

    public static bool IsInside(List<string> map, List<(int, int)> loopPos, List<int> przesmyksX, int xPos, int yPos)
    {
        int currXpos = xPos;

        if (loopPos.Contains((xPos, yPos)) || przesmyksX.Any(x => x + 1 == xPos))
            return false;

        int counterHorizontal = 0;

        while (currXpos >= 0)
        {
            if (loopPos.Contains((currXpos, yPos)) && !(new char[] { '-' }.Contains(map[yPos][currXpos])))
                counterHorizontal++;

            currXpos--;
        }
        return counterHorizontal % 2 == 1;
    }

    public static Direction GetNextStep(List<string> map, int currX, int currY, int prevX, int prevY)
    {
        var currVal = map[currY][currX];

        if (currVal == 'S')
        {
            if (new char[] { '|', 'J', 'L', }.Contains(map[currY + 1][currX]))
                return Direction.Down;
            if (new char[] { '|', '7', 'F', }.Contains(map[currY - 1][currX]))
                return Direction.Up;
            if (new char[] { '-', 'L', 'F', }.Contains(map[currY][currX - 1]))
                return Direction.Left;
            else
                return Direction.Right;
        }
        if (currVal == 'F')
        {
            if (prevX == currX)
                return Direction.Right;
            else
                return Direction.Down;
        }
        if (currVal == '7')
        {
            if (prevX == currX)
                return Direction.Left;
            else
                return Direction.Down;
        }
        if (currVal == 'J')
        {
            if (prevX == currX)
                return Direction.Left;
            else
                return Direction.Up;
        }
        if (currVal == 'L')
        {
            if (prevX == currX)
                return Direction.Right;
            else
                return Direction.Up;
        }
        if (currVal == '-')
        {
            if (prevX < currX)
                return Direction.Right;
            else
                return Direction.Left;
        }
        if (currVal == '|')
        {
            if (prevY < currY)
                return Direction.Down;
            else
                return Direction.Up;
        }
        else
            throw new Exception("Unknown char!");
    }

    public static IEnumerable<int> AllIndexesOf(string str, string searchstring)
    {
        int minIndex = str.IndexOf(searchstring);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
        }
    }
}

enum Direction
{
    Up,
    Right,
    Down,
    Left
}