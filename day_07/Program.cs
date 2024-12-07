static class Program {
    public static void Main() {
        string inputPath = "../data/day_07/input.txt";
        long result = File
            .ReadLines(inputPath)
            .Select(line => line
                .Split([':', ' '])
                .Where(n => !string.IsNullOrEmpty(n))
                .Select(Int64.Parse)
                .ToList()
            )
            .Where(numbers => {
                foreach (var opComb in OpCombinations(numbers.Count() - 1)) {
                    long res = numbers[1];
                    foreach (var index in Enumerable.Range(0, opComb.Count() - 1)) {
                        res = (opComb.ElementAt(index) == '+') ? res + numbers[2 + index] : res * numbers[2 + index];
                    }
                    if (numbers[0] == res) {
                        return true;
                    }
                }
                return false;
            })
            .Select(numbers => numbers.First())
            .Sum();
        Console.WriteLine($"Result: {result}.");
    }

    public static IEnumerable<IEnumerable<char>> OpCombinations(int repeat = 1) {
        if (repeat == 0) {
            yield return Enumerable.Empty<char>();
            yield break;
        }

        foreach (var combination in OpCombinations(repeat - 1)) {
            foreach (var op in new char[] { '+', '*' }) {
                yield return Enumerable.Repeat(op, 1).Concat(combination);
            }
        }
    }
}
