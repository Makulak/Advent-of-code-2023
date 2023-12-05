internal class Program
{
    private static void Main(string[] args)
    {
        string[] text = File.ReadAllLines(@"../../../input.txt");

        var almanac = new Almanac();

        AlmanacNode? nodeToAdd = null;

        foreach (string line in text)
        {
            if (line.Contains("seeds"))
            {
                almanac.Seeds = line.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Skip(1).Select(long.Parse).ToList();
            }
            else if (line.Contains("map"))
            {
                nodeToAdd = new AlmanacNode(line.Split(" ").First());
            }
            else if (string.IsNullOrEmpty(line) && nodeToAdd != null)
            {
                almanac.PushNewNode(nodeToAdd);
            }
            else if(!string.IsNullOrEmpty(line)) // numbers
            {
                var numbers = line.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse).ToList();
                nodeToAdd.Maps.Add(new AlmanacMap(numbers[1], numbers[0], numbers[2]));
            }
        }
        almanac.PushNewNode(nodeToAdd);

        Console.WriteLine(almanac.GetLowestLocation());
    }
}

public class Almanac
{
    public List<long> Seeds { get; set; }
    public AlmanacNode? Root { get; set; }

    public void PushNewNode(AlmanacNode node)
    {
        if (Root == null)
            Root = node;
        else
        {
            var current = Root;
            while (current.Next != null)
                current = current.Next;

            current.Next = node;
        }
    }

    public long GetLowestLocation()
    {
        return Seeds.Select(Root.CalculateNext).OrderBy(x => x).First();
    }
}

public class AlmanacNode
{
    public string Name { get; set; }
    public List<AlmanacMap> Maps { get; set; }

    public AlmanacNode? Next { get; set; }

    public AlmanacNode(string name)
    {
        Maps = new List<AlmanacMap>();
        Name = name;
    }

    public long CalculateNext(long value)
    {
        var valueToReturn = value;

        foreach (var map in Maps)
        {
            if (map.IsInScope(value))
            {
                valueToReturn = map.GetDestinationIndex(value);
                break;
            }
        }

        if (Next != null)
            return Next.CalculateNext(valueToReturn);

        return valueToReturn;
    }
}

public class AlmanacMap
{
    public long StartSourceIndex { get; set; }
    public long StartDestinationIndex { get; set; }
    public long Length { get; set; }

    public AlmanacMap(long startSourceIndex, long startDestinationIndex, long length)
    {
        StartSourceIndex = startSourceIndex;
        StartDestinationIndex = startDestinationIndex;
        Length = length;
    }

    public bool IsInScope(long sourceIndex)
    {
        return StartSourceIndex <= sourceIndex && sourceIndex <= StartSourceIndex + Length - 1;
    }

    public long GetDestinationIndex(long sourceIndex)
    {
        return StartDestinationIndex + sourceIndex - StartSourceIndex;
    }
}