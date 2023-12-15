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
    var horizontal = GetHorizontalReflection(pattern) ?? 0;
    var vertical = GetVerticalReflection(pattern) ?? 0;

    return horizontal * 100 + vertical;
}
// 27300 too low
// 44392 too high
// 41179 too high

// 37859

static int? GetHorizontalReflection(List<string> source, int prevMatch = -1)
{
    static bool CheckMatch(string A, string B, bool doSmudgeCheck)
    {
        int matches = Enumerable.Range(0, A.Length).Count(c => A[c] == B[c]);
        if (matches == A.Length) return true;
        if (doSmudgeCheck && matches == A.Length - 1) return true;

        return false;
    }

    bool doCleaning = prevMatch != -1;

    for (int y = 0; y < source.Count - 1; y++)
    {
        if (CheckMatch(source[y], source[y + 1], doCleaning))
        {
            if (doCleaning && prevMatch == y + 1) continue; // skip prev seen.

            bool isMatch = true;
            int numToEdge = int.Min(y, source.Count - (y + 2));
            for (int j = 1; j <= numToEdge; j++)
            {
                isMatch = CheckMatch(source[y - j], source[y + j + 1], doCleaning);
            }
            if (isMatch) return y + 1;
        }
    }

    return null;
}

static int? GetVerticalReflection(List<string> pattern)
{
    List<string> columns = new();
    for (int i = 0; i < pattern.First().Count(); i++)
        columns.Add(new string(pattern.Select(x => x[i]).ToArray()));

    return GetHorizontalReflection(columns);
}
