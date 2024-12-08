static class Program {
    static void Main() {
        string inputPath = "../data/day_08/input.txt";

        var (antennas, dimensions) = FindAntennas(inputPath);
        int result = antennas
            .Where(kvp => kvp.Value.Count() > 1)
            .Select(kvp => kvp.Value
                    .SelectMany((_, i) => kvp.Value.Skip(i + 1), (x, y) => (x, y))
                    .Select(pair => {
                        var (x1, x2) = pair.x;
                        var (y1, y2) = pair.y;
                        var delta = (x1 - y1, x2 - y2);
                        return new List<(int, int)> {(x1 + delta.Item1, x2 + delta.Item2), (y1 - delta.Item1, y2 - delta.Item2)};
                    })
                    .SelectMany(list => list)
            ).SelectMany(list => list)
            .Where(coord => 0 <= coord.Item1 && coord.Item1 < dimensions.Item1 && 0 <= coord.Item2 && coord.Item2 < dimensions.Item2)
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
}