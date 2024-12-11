class Program {
    static void Main() {
        string inputPath = "../data/day_11/input.txt";
        var stones = File
            .ReadAllText(inputPath)
            .Split(" ")
            .Select(s => Int64.Parse(s));
        
        long numBlinks = 75;
        long result = 0;
        var memory = new Dictionary<(long, long), long>();

        foreach (var stone in stones) {
            result += GetNumberOfStones(stone, numBlinks, memory);
        }

        Console.WriteLine($"Result: {result}.");
    }

    static long GetNumberOfStones(long stone, long numBlinks, Dictionary<(long, long), long> memory) {
        if (memory.ContainsKey((stone, numBlinks))) {
            return memory[(stone, numBlinks)];
        }
        if (numBlinks == 0) {
            memory[(stone, numBlinks)] = 1;
            return 1;
        }
        if (stone == 0) {
            var num = GetNumberOfStones(1, numBlinks - 1, memory);
            memory[(stone, numBlinks)] = num;
            return num;
        } else if (stone.ToString().Length % 2 == 0) {
            var str = stone.ToString();
            var cutIndex = str.Length / 2;
            var stone1 = Int64.Parse(str.Substring(0, cutIndex));
            var stone2 = Int64.Parse(str.Substring(cutIndex));

            var num1 = GetNumberOfStones(stone1, numBlinks - 1, memory);
            var num2 = GetNumberOfStones(stone2, numBlinks - 1, memory);
            memory[(stone, numBlinks)] = num1 + num2;
            return num1 + num2;
        } else {
            var num = GetNumberOfStones(stone * 2024, numBlinks - 1, memory);
            memory[(stone, numBlinks)] = num;
            return num;
        }
    }
}
