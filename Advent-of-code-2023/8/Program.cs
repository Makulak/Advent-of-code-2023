internal class Program
{
    private static void Main(string[] args)
    {
        var nodeList = new List<Node>();

        string[] text = File.ReadAllLines(@"../../../input.txt");

        var directions = text.First();

        var textNodes = text.Skip(2).ToList();

        foreach (var node in textNodes)
        {
            var x = node.Replace("=", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", "")
                .Split(" ")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            nodeList.Add(new Node { Name = x[0], Left = x[1], Right = x[2] });
        }

        var iterator = 0;
        var currentNode = nodeList.Single(x => x.Name == "AAA");

        while (true)
        {
            var nextStep = directions[iterator % directions.Length];

            if (nextStep == 'L')
                currentNode = nodeList.Single(x => x.Name == currentNode.Left);
            else
                currentNode = nodeList.Single(x => x.Name == currentNode.Right);

            iterator++;

            if (currentNode.Name == "ZZZ")
                break;
        }

        Console.WriteLine(iterator);
    }
}

public class Node
{
    public string Name { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }
}