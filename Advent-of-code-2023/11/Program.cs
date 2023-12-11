string[] text = File.ReadAllLines(@"../../../input.txt");

List<string> list = new();

List<int> xWithoutGalaxy = new();
List<int> yWithoutGalaxy = new();

List<(int, int)> galaxies = new();

long sum = 0;

foreach (string line in text)
{
    if (line.IndexOf('#') == -1)
        yWithoutGalaxy.Add(list.Count());
    else
    {
        var indexes = GetAllIndexes(line, "#");
        galaxies.AddRange(indexes.Select(i => (i, list.Count())));
    }
    list.Add(line);
}
for (int i = 0; i < list.First().Count(); i++)
{
    if (new string(list.Select(x => x[i]).ToArray()).IndexOf('#') == -1)
        xWithoutGalaxy.Add(i);
}

for (int i = 0; i < galaxies.Count() - 1; i++)
{
    for (int j = i + 1; j < galaxies.Count(); j++)
    {
        var result = GetShortestPath(xWithoutGalaxy, yWithoutGalaxy, galaxies[i], galaxies[j]);
        sum += result;
    }
}

Console.WriteLine(sum);

static int GetShortestPath(List<int> xWithoutGalaxy, List<int> yWithoutGalaxy, (int, int) from, (int, int) to)
{
    var dist = Math.Abs(from.Item1 - to.Item1) + Math.Abs(from.Item2 - to.Item2);

    var doubledRows = yWithoutGalaxy.Where(x => x > Math.Min(from.Item2, to.Item2) && x < Math.Max(from.Item2, to.Item2)).Count();
    var doubledColumns = xWithoutGalaxy.Where(x => x > Math.Min(from.Item1, to.Item1) && x < Math.Max(from.Item1, to.Item1)).Count();

    return dist + doubledRows * 999999 + doubledColumns * 999999;
}
//971423302 Too low
//82000210 Too low

static IEnumerable<int> GetAllIndexes(string str, string searchstring)
{
    int minIndex = str.IndexOf(searchstring);
    while (minIndex != -1)
    {
        yield return minIndex;
        minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
    }
}