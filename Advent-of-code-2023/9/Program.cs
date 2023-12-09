string[] text = File.ReadAllLines(@"../../../input.txt");

int sum = 0;
foreach (string line in text)
{
    var numbers = line.Split(' ').Select(int.Parse).ToList();
    var result = GetDiffFromSequence(numbers);
    sum += result.Last();
}

Console.WriteLine(sum);

List<int> GetDiffFromSequence(List<int> numbers)
{
    if (numbers.All(x => x == 0))
        return numbers;

    List<int> seqToReturn = new();

    for (int i = 0; i < numbers.Count - 1; i++)
        seqToReturn.Add(numbers[i + 1] - numbers[i]);

    var childDiff = GetDiffFromSequence(seqToReturn);

    numbers.Add(numbers.Last() + childDiff.Last());

    return numbers;
}