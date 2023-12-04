string[] text = File.ReadAllLines(@"../../../input.txt");

int sum = 0;

var stringNumbersList = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

foreach (var line in text)
{
    var first = FindFirst(line);
    var last = FindLast(line);

    sum += first * 10 + last;
}

int FindFirst(string line)
{
    int minIdx = 0;
    for (int i = 0; i < line.Length; i++)
    {
        if (int.TryParse(line[i].ToString(), out var result))
            return result;

        var stringToCompare = new String(line.Take(i + 1).ToArray());
        var map = stringNumbersList.Select(stringToCompare.Contains).ToList();

        if (map.Any(x => x == true))
            return map.IndexOf(true) + 1;
    }

    throw new Exception("XD");
}

int FindLast(string line)
{
    int minIdx = 0;
    for (int i = line.Length - 1; i >= 0; i--)
    {
        if (int.TryParse(line[i].ToString(), out var result))
            return result;

        var stringToCompare = new String(line.Skip(i).ToArray());
        var map = stringNumbersList.Select(stringToCompare.Contains).ToList();

        if (map.Any(x => x == true))
            return map.IndexOf(true) + 1;
    }

    throw new Exception("XD");
}

Console.WriteLine(sum);