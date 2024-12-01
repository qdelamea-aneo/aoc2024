using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;


static class Program {
    static void Main() {
        string inputPath = "../data/day_01/input_1.txt";
        try {
            using var reader = new StreamReader(inputPath);
            string data = reader.ReadToEnd();
            List<int> numbers = data.Split(['\n', ' '])
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => Int32.Parse(s))
                .ToList();
            // Console.WriteLine(string.Join(',', numbers));
            List<int> left = numbers.Where((n, i) => i % 2 == 0).OrderBy(n => n).ToList();
            // Console.WriteLine("Left: " + string.Join(',', left));
            List<int> right = numbers.Where((n, i) => i % 2 != 0).OrderBy(n => n).ToList();
            // Console.WriteLine("Right: " + string.Join(',', right));
            int result = left.Zip(right, (x, y) => Math.Abs(x - y)).Sum();

            Console.WriteLine("Result: " + result);
            Environment.Exit(0);
        }
        catch (IOException e)
        {
            Console.WriteLine("The input file could not be read.");
            Console.WriteLine(e);
            Environment.Exit(1);
        }
        catch (FormatException e)
        {
            Console.WriteLine("An error occured when parsing numbers.");
            Console.WriteLine(e);
            Environment.Exit(2);
        }
    }
}
