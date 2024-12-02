class Program {
    static void Main() {
        try {
            string inputPath = "../data/day_02/input.txt";
            var reader = new StreamReader(inputPath);
            var data = reader.ReadToEnd();
            int result = data
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split(' ').Select(n => Int32.Parse(n)).ToList())
                .Where(s => IsReportSafe(s))
                .Count();
            
            Console.WriteLine($"Result: {result}.");
        }
        catch (IOException e)
        {
            Console.WriteLine("The input file could not be read.");
            Console.WriteLine(e);
        }
        catch (FormatException e)
        {
            Console.WriteLine("Error while parsing string to integer.");
            Console.WriteLine(e);
        }
    }

    static bool IsReportSafe(List<int> report) {
        if (report.Count == 1) {
            return true;
        }

        var maxMagnitude = report.Zip(report.Skip(1), (x, y) => Math.Abs(x - y)).Max();
        var isAscending = report.Zip(report.Skip(1), (x, y) => x > y).All(x => x);
        var isDescending = report.Zip(report.Skip(1), (x, y) => x < y).All(x => x);
        if (maxMagnitude <= 3 && (isAscending || isDescending)) {
            return true;
        }
        return false;
    }
}
