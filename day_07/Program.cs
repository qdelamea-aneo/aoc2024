using System.Threading.Tasks;

static class Program {
    public static void Main() {
        string inputPath = "../data/day_07/input.txt";
        long result = File
            .ReadAllLines(inputPath)
            .AsParallel()
            .Select(line => line
                .Split([':', ' '])
                .Where(n => !string.IsNullOrEmpty(n))
                .Select(Int64.Parse)
                .ToList()
            )
            .Where(numbers => OpCombinations(numbers.Count() - 1)
                    .Any(opComb => {
                        long res = numbers[1];
                        foreach (var index in Enumerable.Range(0, opComb.Count() - 1)) {
                            switch (opComb.ElementAt(index)) {
                                case '+':
                                    res += numbers[2 + index];
                                    break;
                                case '*':
                                    res *= numbers[2 + index];
                                    break;
                                case '|':
                                    res = res * (long)Math.Pow(10, numbers[2 + index].ToString().Count()) + numbers[2 + index];
                                    break;
                                default:
                                    throw new ArgumentException($"Inconsistent operation {opComb.ElementAt(index)}.");
                            }
                        }
                        return numbers[0] == res;
                    })
                )
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
            foreach (var op in new char[] { '+', '*', '|' }) {
                yield return Enumerable.Repeat(op, 1).Concat(combination);
            }
        }
    }
}
