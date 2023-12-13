string[] text = File.ReadAllLines(@"../../../input.txt");

List<string> pattern = new();
var result = 0;

foreach (string line in text)
{
    if (string.IsNullOrEmpty(line))
    {
        result += Calculate(pattern);
        pattern.Clear();
    }
    else
        pattern.Add(line);
}

Console.WriteLine(result);

static int Calculate(List<string> pattern)
{
    var vert = GetHorizontalReflection(pattern);
    if (vert != null)
        return vert.Value * 100;
    else
        return GetVerticalReflection(pattern).Value;

}
// 27300 too low
// 44392 too high
// 41179 too high

static int? GetHorizontalReflection(List<string> pattern)
{
    var test = pattern.Select((x, i) => FindAllIndexof(pattern, x).Where(y => y != i).ToList()).ToList();
    var matchingIndexes = pattern.Select((x, i) => FindAllIndexof(pattern, x).Where(y => y != i).ToList())
            .Select(x => x.Count() == 0 ? new List<int> { -100 } : x)
            .ToList();

    var longestStrike = 0;
    var longestStrikeStartIdx = 0;

    var currentStrike = 0;
    var currentStrikeStartIdx = 0;
    for (int i = 0; i < matchingIndexes.Count - 1; i++)
    {
        if (matchingIndexes[i].Any(x => matchingIndexes[i + 1].Contains(x - 1)))
            currentStrike++;
        else if (currentStrike > longestStrike && (currentStrikeStartIdx == 0 || currentStrikeStartIdx + currentStrike + 1 == matchingIndexes.Count()))
        {
            longestStrike = currentStrike + 1;
            longestStrikeStartIdx = currentStrikeStartIdx;
            currentStrikeStartIdx = i + 1;
        }
        else
        {
            currentStrike = 0;
            currentStrikeStartIdx = i + 1;
        }
    }

    if (currentStrike > longestStrike)
    {
        longestStrike = currentStrike;
        longestStrikeStartIdx = currentStrikeStartIdx;
    }

    var endLongestStrikeIdx = longestStrike + longestStrikeStartIdx;

    if (matchingIndexes.First().All(x => x == -100) && matchingIndexes.Last().All(x => x == -100))
        return null;
    if (longestStrike == matchingIndexes.Count())
        return longestStrike / 2 + longestStrikeStartIdx;
    if (longestStrikeStartIdx == 0)
        return longestStrike / 2;
    if (longestStrikeStartIdx != 0 && endLongestStrikeIdx + 1 == matchingIndexes.Count())
        return longestStrike / 2 + longestStrikeStartIdx;

    return null;
}

static int? GetVerticalReflection(List<string> pattern)
{
    List<string> columns = new();
    for (int i = 0; i < pattern.First().Count(); i++)
        columns.Add(new string(pattern.Select(x => x[i]).ToArray()));

    return GetHorizontalReflection(columns);
}

static List<int> FindAllIndexof<T>(IEnumerable<T> values, T val)
{
    var x = values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToList();

    return x;
}