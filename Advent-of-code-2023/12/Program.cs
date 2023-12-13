using System.Text.RegularExpressions;

string[] text = File.ReadAllLines(@"../../../input.txt");

var total = 0;

foreach (string line in text)
{
    var record = line.Split(' ').First();
    record = $"{record}?{record}?{record}?{record}?{record}";
    var oryginalNumbers = line.Split(' ').Last();
    var numbers = $"{oryginalNumbers},{oryginalNumbers},{oryginalNumbers},{oryginalNumbers},{oryginalNumbers}".Split(",").Select(int.Parse).ToList();

    var unknownCount = record.Where(x => x == '?').Count();

    List<string> allPossibilities = GeneratePermutations(record, numbers.Sum() - record.Where(x => x == '#').Count());

    var result = allPossibilities.Where(x =>
    {
        var regexMatch = Regex.Matches(x, @"[#]+");

        if (regexMatch.Count != numbers.Count())
            return false;

        for (int j = 0; j < regexMatch.Count; j++)
        {
            if (regexMatch[j].Value.Length != numbers[j])
                return false;
        }

        return true;
    }).Count();

    total+= result;
}

Console.WriteLine(total); //7728 too low

static List<string> GeneratePermutations(string input, int replacementCount)
{
    List<string> permutations = new List<string>();
    char[] inputArray = input.ToCharArray();

    GeneratePermutationsRecursive(inputArray, replacementCount, 0, permutations);

    return permutations;
}

static void GeneratePermutationsRecursive(char[] inputArray, int replacementCount, int index, List<string> permutations)
{
    if (replacementCount == 0)
    {
        permutations.Add(new string(inputArray));
        return;
    }

    for (int i = index; i < inputArray.Length; i++)
    {
        if (inputArray[i] == '?')
        {
            inputArray[i] = '#';
            GeneratePermutationsRecursive(inputArray, replacementCount - 1, i + 1, permutations);
            inputArray[i] = '?'; // Backtrack
        }
    }
}

