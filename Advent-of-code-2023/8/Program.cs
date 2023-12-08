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
        var currentNodes = nodeList.Where(x => x.Name.EndsWith("A")).ToList();

        var zIndexes = new List<(string, int)>();

        while (true)
        {
            var nextStep = directions[iterator % directions.Length];
            iterator++;
            var nextNodes = new List<Node>();

            foreach (var node in currentNodes)
            {
                if (nextStep == 'L')
                    nextNodes.Add(nodeList.Single(x => x.Name == node.Left));
                else
                    nextNodes.Add(nodeList.Single(x => x.Name == node.Right));
            }
            currentNodes = nextNodes;

            if (currentNodes.Any(x => x.Name.EndsWith("Z")))
                zIndexes.AddRange(currentNodes.Where(x => x.Name.EndsWith("Z")).Select(x => (x.Name, iterator)));

            if (zIndexes.GroupBy(x => x.Item1).All(x => x.Count() > 1) && zIndexes.Count > 0)
            {
                var multiplliers = zIndexes.GroupBy(x => x.Item1).Select(x => (x.First().Item1, x.ToList()[1].Item2 / x.ToList()[0].Item2));
                var firstZIndexes = zIndexes.GroupBy(x => x.Item1).Select(x => x.First().Item2);

                // Calculate LCM on https://www.calculatorsoup.com/calculators/math/lcm.php (12083 13207 17141 18827 20513 22199) => 13,385,272,668,829
            }
        }
    }
}

public class Node
{
    public string Name { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }

    public override string ToString()
    {
        return $"{Name} => ({Left} - {Right})";
    }
}