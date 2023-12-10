internal class Program
{
    private static void Main(string[] args)
    {
        string[] text = File.ReadAllLines(@"../../../input.txt");

        var map = new List<string>();

        var xPos = 0;
        var yPos = 0;

        foreach (string line in text)
        {
            map.Add(line);

            if (line.Contains('S'))
            {
                xPos = line.IndexOf('S');
                yPos = text.ToList().IndexOf(line);
            }
        }

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
        }

        Console.WriteLine(steps/2);
    }

    public static Direction GetNextStep(List<string> map, int currX, int currY, int prevX, int prevY)
    {
        var currVal = map[currY][currX];

        if(currVal == 'S')
        {
            if (new char[] { '|', 'J', 'L', }.Contains(map[currY + 1][currX]))
                return Direction.Down;
            if (new char[] { '|', '7', 'F', }.Contains(map[currY - 1][currX]))
                return Direction.Up;
            if (new char[] { '-', 'L', 'F', }.Contains(map[currY][currX-1]))
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
        if(currVal == '-')
        {
            if(prevX < currX)
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
}

enum Direction
{
    Up,
    Right,
    Down,
    Left
}