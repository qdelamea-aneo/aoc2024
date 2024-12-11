class Program {
    static void Main() {
        string inputPath = "../data/day_11/input.txt";
        var stones = File
            .ReadAllText(inputPath)
            .Split(" ")
            .Select(s => Int64.Parse(s));
        
        int numBlinks = 25;

        for (int i = 0; i < numBlinks; i++) {
            stones = stones
                .SelectMany(n => {
                    if (n == 0) {
                        return new List<long> { 1 };
                    } else if (n.ToString().Length % 2 == 0) {
                        var str = n.ToString();
                        var cutIndex = str.Length / 2;
                        return new List<long> { Int64.Parse(str.Substring(0, cutIndex)), Int64.Parse(str.Substring(cutIndex)) };
                    } else {
                        return new List<long> { n * 2024 };
                    }
                });
        }

        Console.WriteLine($"Result: {stones.Count()}.");
    }
}
