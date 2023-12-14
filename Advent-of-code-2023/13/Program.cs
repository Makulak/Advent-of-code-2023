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
    List<int> previousIdxs = new();
    bool found = false;
    int? foundIdx = null;

    for (int i = 0; i < pattern.Count; i++)
    {
        var currentIdxs = FindAllIndexof(pattern, pattern[i]).Where(x => x != i);

        if (i == 0 && !currentIdxs.Any())
        {
            found = false;
            break;
        }
        else if (currentIdxs.Any(x => x == i - 1))
        {
            found = true;
            foundIdx = i;
            break;
        }
    }

    for (int i = pattern.Count - 1; i >= 0; i--)
    {
        var currentIdxs = FindAllIndexof(pattern, pattern[i]).Where(x => x != i);

        if (i == 0 && !currentIdxs.Any())
        {
            found = false;
            break;
        }
        else if(currentIdxs.Any(x => x == i - 1))
        {
            found = true;
            foundIdx = i;
            break;
        }
    }

    return foundIdx;
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

