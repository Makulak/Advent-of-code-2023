
using System.Text;

string text = File.ReadAllLines(@"../../../input.txt").Single();
List<string> list = text.Split(',').ToList();
int sum = 0;

foreach (string step in list)
{
    sum += CalculateHash(step);
}

Console.WriteLine(sum);

int CalculateHash(string step)
{
    int current = 0;

    foreach(var c in Encoding.ASCII.GetBytes(step))
    {
        current += (int)c;
        current *= 17;
        current = current % 256;
    }

    return current;
}