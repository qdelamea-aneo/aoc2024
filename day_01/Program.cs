static class Program {
    static void Main() {
        string inputPath = "../data/day_01/input.txt";
        try {
            using var reader = new StreamReader(inputPath);
            string data = reader.ReadToEnd();
            var numbers = data.Split(['\n', ' '])
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => Int32.Parse(s))
                .ToList();
            var left = numbers
                .Where((n, i) => i % 2 == 0)
                .OrderBy(n => n)
                .ToList();
            var right = numbers
                .Where((n, i) => i % 2 != 0)
                .GroupBy(n => n)
                .ToDictionary(g => g.Key, g => g.Count());
            int result = left
                .Where(n => right.ContainsKey(n))
                .Select(n => n * right[n])
                .Sum();

            Console.WriteLine($"Result: {result}.");
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
