internal class Program
{
    private static void Main(string[] args)
    {
        string[] text = File.ReadAllLines(@"../../../input.txt");

        var contrapation = new Contrapation(text);
        Console.WriteLine(contrapation.StartPartOne());
        Console.ReadLine();
        Console.WriteLine(contrapation.StartPartTwo());
    }
}

public class Beam
{
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Direction { get; set; }

    public Beam(int x, int y, Direction direction)
    {
        X = x;
        Y = y;
        Direction = direction;
    }

    public override string ToString()
    {
        return $"{X}, {Y}; Direction: {Direction}";
    }
}// 8056 too low

public class Contrapation
{
    public List<Beam> Beams { get; set; }

    public string[] Area { get; set; }

    List<string> visitedTiles;

    public Contrapation(string[] area)
    {
        visitedTiles = new();
        Area = area;
        Beams = new();
    }

    public int StartPartTwo()
    {
        int max = -1;
        for (int x = 0; x < Area.First().Length; x++)
        {
            // Top
            visitedTiles.Clear();
            Beams.Clear();

            SetStartPos(Direction.Down, x, 0);
            var resultTop = Start();
            if (resultTop > max)
                max = resultTop;

            // Bottom
            visitedTiles.Clear();
            Beams.Clear();

            SetStartPos(Direction.Up, x, Area.Length - 1);
            var resultBottom = Start();
            if (resultBottom > max)
                max = resultBottom;

            Console.WriteLine($"x: {x}");
        }

        for (int y = 0; y < Area.Length; y++)
        {
            // Left
            visitedTiles.Clear();
            Beams.Clear();

            SetStartPos(Direction.Right, 0, y);
            var resultLeft = Start();
            if (resultLeft > max)
                max = resultLeft;

            // Right
            visitedTiles.Clear();
            Beams.Clear();

            SetStartPos(Direction.Left, Area.First().Length - 1, y);
            var resultRight = Start();
            if (resultRight > max)
                max = resultRight;

            Console.WriteLine($"y: {y}");
        }

        return max;
    }

    private void SetStartPos(Direction direction, int x, int y)
    {
        if (direction == Direction.Down)
        {
            if (Area[y][x] == '\\')
                Beams.Add(new Beam(x, y, Direction.Right));
            else if (Area[y][x] == '/')
                Beams.Add(new Beam(x, y, Direction.Left));
            else if (Area[y][x] == '-')
            {
                Beams.Add(new Beam(x, y, Direction.Right));
                Beams.Add(new Beam(x, y, Direction.Left));
            }
            else
                Beams.Add(new Beam(x, y, direction));
        }
        else if (direction == Direction.Up)
        {
            if (Area[y][x] == '\\')
                Beams.Add(new Beam(x, y, Direction.Left));
            else if (Area[y][x] == '/')
                Beams.Add(new Beam(x, y, Direction.Right));
            else if (Area[y][x] == '-')
            {
                Beams.Add(new Beam(x, y, Direction.Right));
                Beams.Add(new Beam(x, y, Direction.Left));
            }
            else
                Beams.Add(new Beam(x, y, direction));
        }
        else if (direction == Direction.Right)
        {
            if (Area[y][x] == '\\')
                Beams.Add(new Beam(x, y, Direction.Down));
            else if (Area[y][x] == '/')
                Beams.Add(new Beam(x, y, Direction.Up));
            else if (Area[y][x] == '|')
            {
                Beams.Add(new Beam(x, y, Direction.Down));
                Beams.Add(new Beam(x, y, Direction.Up));
            }
            else
                Beams.Add(new Beam(x, y, direction));
        }
        else if (direction == Direction.Left)
        {
            if (Area[y][x] == '\\')
                Beams.Add(new Beam(x, y, Direction.Up));
            else if (Area[y][x] == '/')
                Beams.Add(new Beam(x, y, Direction.Down));
            else if (Area[y][x] == '|')
            {
                Beams.Add(new Beam(x, y, Direction.Down));
                Beams.Add(new Beam(x, y, Direction.Up));
            }
            else
                Beams.Add(new Beam(x, y, direction));
        }
    }

    public int StartPartOne()
    {
        SetStartPos(Direction.Down, 0, 0);
        return Start();
    }

    public int Start()
    {
        int loopIterator = 0;
        while (Beams.Any())
        {
            MakeMove();

            loopIterator++;

            //Console.Clear();
            //Print();
            //Console.WriteLine(visitedTiles.Select(x => x.Split(";").First()).Distinct().Count());
            //Console.WriteLine(loopIterator);
            //Console.ReadLine();
        }

        return visitedTiles.Select(x => x.Split(";").First()).Distinct().Count();
    }

    public void MakeMove()
    {
        var newBeams = new List<Beam>();
        var beamsToRemove = new List<Beam>();

        foreach (var beam in Beams)
        {
            var nextPos = GetNextBeamPos(beam);
            if (!nextPos.HasValue || visitedTiles.Any(x => x == beam.ToString()))
            {
                visitedTiles.Add(beam.ToString());
                beamsToRemove.Add(beam);
                continue;
            }

            visitedTiles.Add(beam.ToString());

            var areaChar = GetAreaChar(nextPos.Value);

            beam.X = nextPos.Value.Item1;
            beam.Y = nextPos.Value.Item2;

            if (areaChar == '/' || areaChar == '\\')
            {
                beam.Direction = GetProperDirection(beam.Direction, areaChar.Value);
            }
            else if (areaChar == '-' && (beam.Direction == Direction.Down || beam.Direction == Direction.Up))
            {
                beam.Direction = Direction.Left;

                newBeams.Add(new Beam(beam.X, beam.Y, Direction.Right));
            }
            else if (areaChar == '|' && (beam.Direction == Direction.Left || beam.Direction == Direction.Right))
            {
                beam.Direction = Direction.Up;

                newBeams.Add(new Beam(beam.X, beam.Y, Direction.Down));
            }
        }
        beamsToRemove.ForEach(b => Beams.Remove(b));
        Beams.AddRange(newBeams);

        newBeams.Clear();
        beamsToRemove.Clear();

        visitedTiles = visitedTiles.Distinct().ToList();
    }

    public void Print()
    {
        for (int i = 0; i < Area.Count(); i++)
        {
            var str = Area[i];

            foreach (var beam in Beams.Where(x => x.Y == i))
            {
                str = str.Remove(beam.X, 1).Insert(beam.X, GetDirectionArrow(beam.Direction).ToString());
            }
            Console.WriteLine(str);
        }
    }

    private char GetDirectionArrow(Direction direction)
    {
        return direction switch
        {
            Direction.Up => '^',
            Direction.Right => '>',
            Direction.Left => '<',
            Direction.Down => 'v',
        };
    }

    private Direction GetProperDirection(Direction currentBeamDirection, char areaChar)
    {
        if (areaChar == '/')
        {
            return currentBeamDirection switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Up,
                Direction.Left => Direction.Down,
                Direction.Down => Direction.Left,
            };
        }
        if (areaChar == '\\')
        {
            return currentBeamDirection switch
            {
                Direction.Up => Direction.Left,
                Direction.Right => Direction.Down,
                Direction.Left => Direction.Up,
                Direction.Down => Direction.Right,
            };
        }
        return currentBeamDirection;
    }

    private (int, int)? GetNextBeamPos(Beam beam)
    {
        var x = beam.X;
        var y = beam.Y;

        if (beam.Direction == Direction.Right)
            x++;
        else if (beam.Direction == Direction.Left)
            x--;
        else if (beam.Direction == Direction.Up)
            y--;
        else if (beam.Direction == Direction.Down)
            y++;

        if (x < 0 || y < 0 || x >= Area.First().Length || y >= Area.Count())
            return null;

        return (x, y);
    }

    private char? GetAreaChar((int, int) pos) => Area[pos.Item2][pos.Item1];
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}