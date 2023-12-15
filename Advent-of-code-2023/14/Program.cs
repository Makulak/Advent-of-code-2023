using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

List<string> text = File.ReadAllLines(@"../../../input.txt").ToList();

static void MoveToNorth(List<string> text)
{
    bool moved = false;

    do
    {
        moved = false;
        for (int y = 1; y < text.Count; y++)
        {
            var lineCurrent = text[y];
            var lineToNorth = text[y - 1];

            for (int x = 0; x < lineCurrent.Length; x++)
            {
                if (lineCurrent[x] == 'O' && lineToNorth[x] == '.')
                {
                    moved = true;
                    lineToNorth = lineToNorth.Remove(x, 1).Insert(x, "O");
                    lineCurrent = lineCurrent.Remove(x, 1).Insert(x, ".");
                }
            }
            text[y] = lineCurrent;
            text[y - 1] = lineToNorth;
        }
    } while (moved);
}

static void MoveToSouth(List<string> text)
{
    bool moved = false;

    do
    {
        moved = false;
        for (int y = text.Count - 2; y >= 0; y--)
        {
            var lineCurrent = text[y];
            var lineToSouth = text[y + 1];

            for (int x = 0; x < lineCurrent.Length; x++)
            {
                if (lineCurrent[x] == 'O' && lineToSouth[x] == '.')
                {
                    moved = true;
                    lineToSouth = lineToSouth.Remove(x, 1).Insert(x, "O");
                    lineCurrent = lineCurrent.Remove(x, 1).Insert(x, ".");
                }
            }
            text[y] = lineCurrent;
            text[y + 1] = lineToSouth;
        }
    } while (moved);
}

static void MoveToRight(List<string> text)
{
    for (int y = 0; y < text.Count; ++y)
    {
        if (!text[y].Contains("O."))
            continue;

        text[y] = string.Join('#', text[y]
            .Split('#')
            .Select(x =>
            {
                var roundedRocksCount = x.Count(c => c == 'O');
                return new string('.', x.Length - roundedRocksCount) + new string('O', roundedRocksCount);
            }));
    }
}

static void MoveToLeft(List<string> text)
{
    for (int y = text.Count - 1; y >= 0; --y)
    {
        if (!text[y].Contains(".O"))
            continue;

        text[y] = string.Join('#', text[y]
            .Split('#')
            .Select(x =>
            {
                var roundedRocksCount = x.Count(c => c == 'O');
                return new string('O', roundedRocksCount) + new string('.', x.Length - roundedRocksCount);
            }));
    }
}

static int CalculateWeight(List<string> text)
{
    int sum = 0;
    for (int i = 0; i < text.Count; i++)
        sum += text[i].Where(x => x == 'O').Count() * (text.Count - i);

    return sum;
}

static void DoCycles(List<string> text)
{
    List<string> hashes = new();

    var s = Stopwatch.StartNew();
    for (int i = 0; i < 1000000000; i++)
    {
        MoveToNorth(text);
        MoveToLeft(text);
        MoveToSouth(text);
        MoveToRight(text);

        var hash = ComputeHash(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(text)));

        if(hashes.Any(x => x == hash))
        {
            Console.WriteLine(hashes.Count());
        }
        hashes.Add(hash);
    }
    s.Stop();
}

static string ComputeHash(byte[] b)
{
    return ByteArrayToString(MD5.HashData(b));
}

static string ByteArrayToString(byte[] arrInput)
{
    int i;
    StringBuilder sOutput = new StringBuilder(arrInput.Length);
    for (i = 0; i < arrInput.Length; i++)
    {
        sOutput.Append(arrInput[i].ToString("X2"));
    }
    return sOutput.ToString();
}

static void Print(List<string> text)
{
    Console.WriteLine();
    text.ForEach(Console.WriteLine);
}

Print(text);
DoCycles(text);
Print(text);
var result = CalculateWeight(text);
Console.WriteLine(result);
