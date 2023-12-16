internal class Program
{
    private static void Main(string[] args)
    {
        string[] text = File.ReadAllLines(@"../../../input.txt");

        var contrapation = new Contrapation(text);
        Console.WriteLine(contrapation.Start());
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
}

public class Contrapation
{
    public List<Beam> Beams { get; set; }

    public string[] Area { get; set; }

    List<string> visitedTiles;

    public Contrapation(string[] area)
    {
        visitedTiles = new();
        Area = area;

        var direction = Direction.Right;

        if (Area[0][0] == '\\' || Area[0][0] == '|')
            direction = Direction.Down;
        else if (Area[0][0] == '/')
            direction = Direction.Up;

        Beams = [new Beam(0, 0, direction)];
    }

    public int Start()
    {
        int loopIterator = 0;
        while (Beams.Any())
        {
            MakeMove();

            loopIterator++;

            Console.Clear();
            Print();
            Console.WriteLine(visitedTiles.Select(x => x.Split(";").First()).Distinct().Count());
            Console.WriteLine(loopIterator);
            Console.ReadLine();
        }

        return visitedTiles.Select(x => x.Split(";").First()).Distinct().Count();
    }
    // 7736 - too high
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