static class Program {
    static void Main() {
        string inputPath = "../data/day_08/input.txt";

        var (antennas, dimensions) = FindAntennas(inputPath);
        int result = antennas
            .Where(kvp => kvp.Value.Count() > 1)
            .Select(kvp => kvp.Value
                    .SelectMany((_, i) => kvp.Value.Skip(i + 1), (x, y) => (x, y))
                    .Select(pair => ListAntinodes(pair.x, pair.y, (pair.x.Item1 - pair.y.Item1, pair.x.Item2 - pair.y.Item2), dimensions))
                    .SelectMany(list => list)
            ).SelectMany(list => list)
            .Distinct()
            .Count();

        Console.WriteLine($"Result: {result}.");
    }

    static (Dictionary<char,List<(int,int)>>, (int, int)) FindAntennas(string inputPath) {
        var lines = File.ReadAllLines(inputPath);
        var antennas = new Dictionary<char, List<(int, int)>>();
        int N = lines.Length;
        int M = lines[0].Length;
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < M; j++) {
                var symb = lines[i][j];
                if (symb != '.') {
                    if (antennas.ContainsKey(symb)) {
                        antennas[symb].Add((i,j));
                    } else {
                        antennas.Add(symb, new List<(int, int)> {(i,j)});
                    }
                }
            }
        }
        return (antennas, (N, M));
    }

    public static List<(int, int)> ListAntinodes((int, int) anthenna1, (int, int) anthenna2, (int, int) delta, (int, int) dimensions) {
        var antinodes = new List<(int, int)>();
        var currentPosition = anthenna1;
        while (0 <= currentPosition.Item1 && currentPosition.Item1 < dimensions.Item1 && 0 <= currentPosition.Item2 && currentPosition.Item2 < dimensions.Item2) {
            antinodes.Add(currentPosition);
            currentPosition = (currentPosition.Item1 + delta.Item1, currentPosition.Item2 + delta.Item2);
        }
        currentPosition = anthenna2;
        while (0 <= currentPosition.Item1 && currentPosition.Item1 < dimensions.Item1 && 0 <= currentPosition.Item2 && currentPosition.Item2 < dimensions.Item2) {
            antinodes.Add(currentPosition);
            currentPosition = (currentPosition.Item1 - delta.Item1, currentPosition.Item2 - delta.Item2);
        }
        return antinodes;
    }
}
