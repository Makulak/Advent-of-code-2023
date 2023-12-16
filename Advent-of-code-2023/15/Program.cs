
using System.Text;
using System.Text.RegularExpressions;

string text = File.ReadAllLines(@"../../../input.txt").Single();
List<string> list = text.Split(',').ToList();
int sum = 0;
Dictionary<int, string> dictionary = new();

foreach (string step in list)
{
    var dividedStep = step.Split('-', '=');
    var lensLabel = dividedStep.First();
    var boxHash = CalculateHash(lensLabel);
    var operation = step.Contains('=') ? "=" : "-";
    var lensValue = dividedStep.Length == 2 ? dividedStep.Last() : null;

    if (operation == "=")
        AddNewLens(boxHash, lensLabel, lensValue);
    else
        TryRemoveLens(boxHash, lensLabel);

    sum += CalculateHash(step);
}

Console.WriteLine("Part 1: " + sum);
Console.WriteLine("Part 2: " + CalculatePartTwo());

int CalculatePartTwo()
{
    var sum = 0;
    foreach (var kv in dictionary.Where(x => !string.IsNullOrEmpty(x.Value)))
    {
        var multiplier = kv.Key + 1;
        var numbers = Regex.Matches(kv.Value, @"\d+").Select(x => int.Parse(x.Value)).ToList();

        sum += numbers.Select((x, idx) => multiplier * (idx + 1) * x).Sum();
    }

    return sum;
}

void TryRemoveLens(int key, string lensLabel)
{
    var hasKey = dictionary.TryGetValue(key, out var dictValue);
    if (hasKey)
    {
        dictionary.Remove(key);
        var newString = Regex.Replace(dictValue, @"\[" + lensLabel + @" [\d]+\]", "");
        dictionary.Add(key, newString);
    }
}

void AddNewLens(int key, string lensLabel, string lensValue)
{
    var hasKey = dictionary.TryGetValue(key, out var dictValue);
    if (hasKey)
    {
        dictionary.Remove(key);
        if (dictValue.Contains(lensLabel))
        {
            var newString = Regex.Replace(dictValue, @"\[" + lensLabel + @" [\d]+\]", $"[{lensLabel} {lensValue}]");
            dictionary.Add(key, newString);
        }
        else
            dictionary.Add(key, $"{dictValue} [{lensLabel} {lensValue}]");
    }
    else
        dictionary.Add(key, $"[{lensLabel} {lensValue}]");
}

int CalculateHash(string step)
{
    int current = 0;

    foreach (var c in Encoding.ASCII.GetBytes(step))
    {
        current += (int)c;
        current *= 17;
        current = current % 256;
    }

    return current;
}