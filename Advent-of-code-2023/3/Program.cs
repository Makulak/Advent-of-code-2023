using System.Text.RegularExpressions;

string[] text = File.ReadAllLines(@"../../../input.txt");

var sum = 0;

for (int y = 0; y < text.Length; y++)
{
    var line = text[y];

    var numbers = Regex.Matches(line, @"\d+");

    foreach (Match number in numbers)
    {
        var numberStartX = Math.Max(0, number.Index - 1);
        var numberEndX = Math.Min(line.Length - 1, numberStartX + number.Length + 1);

        var numberStartY = Math.Max(0, y - 1);
        var numberEndY = Math.Min(text.Length - 1, y + 1);

        var length = number.Index == 0 ? numberEndX - numberStartX : numberEndX - numberStartX + 1;

        var x = text.Select(x => x.Substring(numberStartX, length))
            .ToList();

        var test = new String(text.Select(x => x.Substring(numberStartX, length))
            .Skip(numberStartY)
            .Take(numberEndY - numberStartY + 1)
            .SelectMany(x => x).ToArray());

        if (Regex.IsMatch(test, @"[^a-zA-Z0-9.]"))
            sum += int.Parse(number.Value);
    }
}

Console.WriteLine(sum);