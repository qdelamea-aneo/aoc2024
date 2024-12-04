using System.Text.RegularExpressions;


static class Program {
    static void Main() {
        const string inputPath = "../data/day_03/input.txt";
        var sr = File.OpenText(inputPath);
        var data = sr.ReadToEnd();
        var pattern = @"mul\((\d+),(\d+)\)";
        var regex = new Regex(pattern);
        int result = 0;
        foreach (Match match in regex.Matches(data)) {
            result += Int32.Parse(match.Groups[1].Value) * Int32.Parse(match.Groups[2].Value);
        }
        Console.WriteLine($"Result: {result}.");
    }
}
