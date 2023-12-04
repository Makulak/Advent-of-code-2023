internal class Program
{
    private static void Main(string[] args)
    {
        string[] text = File.ReadAllLines(@"../../../input.txt");

        var scratches = new List<Scratch>();
        var idx = 1;
        foreach (string line in text)
        {
            var vals = line.Split('|');

            var winningNumbers = vals.First().Split(":").Last().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
            var scratchNumbers = vals.Last().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();

            scratches.Add(new Scratch(idx, winningNumbers, scratchNumbers));
            idx++;
        }

        for (int i = 0; i < scratches.Count; i++)
        {
            var scratchesToAdd = scratches.Skip(i+1).Take(scratches[i].MatchedNumbersCount).Select(x => { x.AddScratches(scratches[i].Count); return true; }).ToList();
        }

        var total = scratches.Select(x => x.Count).Sum();
        Console.WriteLine(total);
    }
}

class Scratch
{
    public int Index { get; set; }
    public int Count { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> ScratchNumbers { get; set; }

    public int MatchedNumbersCount { get; set; }

    public int Points => MatchedNumbersCount == 1 ? MatchedNumbersCount : (int)Math.Pow(2, MatchedNumbersCount - 1);

    public Scratch(int index, List<int> winningNumbers, List<int> scratchNumbers)
    {
        Count = 1;
        Index = index;
        WinningNumbers = winningNumbers;
        ScratchNumbers = scratchNumbers;

        MatchedNumbersCount = WinningNumbers.Aggregate(0, (total, next) => ScratchNumbers.Contains(next) ? total + 1 : total);
    }

    public void AddScratches(int count)
    {
        Count += count;
    }
}