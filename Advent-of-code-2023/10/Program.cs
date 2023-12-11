internal class Program
{
    private static void Main(string[] args)
    {
        var przesmyksStrings = new List<string>() { "||", "|L", "7|", "|F", "J|" };
        var przesmykX = new List<int>();
        string[] text = File.ReadAllLines(@"../../../input.txt");

        var map = new List<string>();

        List<(int, int)> loopPos = new();

        foreach (string line in text)
        {
            var przesmyksXForLine = new List<int>();

            foreach (var przesmyk in przesmyksStrings)
            {
                var idx = AllIndexesOf(line, przesmyk);
                if (idx.Any())
                    przesmyksXForLine.AddRange(idx);
            }

            przesmyksXForLine = przesmyksXForLine.OrderBy(x => x).ToList();

            for (int i = 0; i < przesmyksXForLine.Count; i++)
                przesmykX.Add(przesmyksXForLine[i] + i);

            map.Add(line);
        }

        przesmykX = przesmykX.Distinct().ToList();

        var map2 = new List<string>();
        foreach (string line in map)
        {
            var newLine = line;

            foreach (var x in przesmykX)
            {
                var charToInsert = "@";
                if (newLine[x] == 'S' || newLine[x] == 'L' || newLine[x] == 'F')
                    charToInsert = "-";
                else if (newLine[x + 1] == 'S' || newLine[x + 1] == '7' || newLine[x + 1] == 'J')
                    charToInsert = "-";
                newLine = newLine.Insert(x + 1, charToInsert);
            }
            map2.Add(newLine);
        }

        PrintMap(map);
        Console.WriteLine();
        PrintMap(map2);

        map = map2;

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

        Console.WriteLine(steps / 2); // End of part1

        var insideCounter = 0;

        for (int y = 0; y < text.Length; y++)
        {
            for (int x = 0; x < text[y].Length; x++)
            {
                if (IsInside(map, loopPos, x, y))
                {
                    if (map[y][x] != '@')
                    {
                        Console.WriteLine($"{x} - {y}");
                        insideCounter++;
                    }
                }
            }
        }
        Console.WriteLine(insideCounter);
    }

    public static bool IsInside(List<string> map, List<(int, int)> loopPos, int xPos, int yPos)
    {
        int currXpos = xPos;

        if (loopPos.Contains((xPos, yPos)))
            return false;

        int counterHorizontal = 0;

        while (currXpos >= 0)
        {
            if (loopPos.Contains((currXpos, yPos)) && map[yPos][currXpos] != '-' && 
                ((currXpos > 1 && map[yPos].Substring(currXpos-2, 2) != "@L" && map[yPos].Substring(currXpos-2, 2) != "@|") || currXpos <= 1))
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

    public static void PrintMap(List<string> map)
    {
        map.ForEach(x => Console.WriteLine(x));
    }
}

enum Direction
{
    Up,
    Right,
    Down,
    Left
}