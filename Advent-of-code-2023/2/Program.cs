using System.Text.RegularExpressions;

string[] text = File.ReadAllLines(@"../../../input.txt");

var maxGreen = 13;
var maxBlue = 14;
var maxRed = 12;

var possibleGamesIdsCount = 0;
var power = 0;

foreach (string line in text)
{
    var isPossible = true;
    var lineMatch = Regex.Match(line, @"Game ([0-9]+): (.+)");

    var gameId = lineMatch.Groups[1].Value;
    var listOfResults = lineMatch.Groups[2].Value.Split(";");

    var minRed = 0; 
    var minGreen = 0;
    var minBlue = 0; 

    foreach (var result in listOfResults)
    {
        var cubes = result.Split(",");

        foreach (var cube in cubes)
        {
            var cubeMatch = Regex.Match(cube, @"([0-9]+) (red|green|blue)");

            var cubesCount = int.Parse(cubeMatch.Groups[1].Value);
            var cubeColor = cubeMatch.Groups[2].Value;

            if (cubeColor == "red" && minRed < cubesCount)
                minRed = cubesCount;
            if (cubeColor == "green" && minGreen < cubesCount)
                minGreen = cubesCount;
            if (cubeColor == "blue" && minBlue < cubesCount)
                minBlue = cubesCount;

            //if (!IsPossible(int.Parse(cubesCount), cubeColor))
            //{
            //    isPossible = false;
            //    break;
            //}
        }
        //if (!isPossible)
            //break;
    }
    power += minRed * minGreen * minBlue;

    //if (isPossible)
    //    possibleGamesIdsCount += int.Parse(gameId);
}

//Console.WriteLine(possibleGamesIdsCount);
Console.WriteLine(power);

bool IsPossible(int cubesCount, string cubeColor)
{
    if (cubeColor == "red")
        return maxRed >= cubesCount;
    if (cubeColor == "green")
        return maxGreen >= cubesCount;
    if (cubeColor == "blue")
        return maxBlue >= cubesCount;
    else
        throw new Exception("Impossible color");
}