string[] text = File.ReadAllLines(@"../../../input.txt");

//List<int> Times = text.First().Split(":").Last().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
//List<int> Distaces = text.Last().Split(":").Last().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();

List<long> Times = new List<long>() { long.Parse(text.First().Split(":").Last().Replace(" ", "")) };
List<long> Distaces = new List<long>() { long.Parse(text.Last().Split(":").Last().Replace(" ", "")) };

var result = 1;

for(int i = 0; i < Times.Count; i++)
{
    var raceTime = Times[i];
    var recordDistance = Distaces[i];

    var counter = 0;

    for(int j = 1; j < raceTime; j++)
    {
        var distance = j * (raceTime - j);

        if (distance > recordDistance)
            counter++;
    }

    result *= counter;
}

Console.WriteLine(result);