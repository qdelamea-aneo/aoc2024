class Program {
    static void Main() {
        string inputPath = "../data/day_10/input.txt";
        var (map, trailheadList) = LoadMapFromFile(inputPath);
        
        var trailheads = trailheadList.ToDictionary(
            key => key, 
            value => new List<(int, int)>()
        );
        foreach (var trailhead in trailheads.Keys) {
            Console.WriteLine($"Trailhead: {trailhead}.");
            GetScore(map, trailhead, trailhead, trailheads);
        }

        int result = trailheads.Values
            .SelectMany(list => list)
            .Count();

        // foreach (var key in trailheads.Keys) {
        //     Console.WriteLine($"Key = {key}, Value = {string.Join(",", trailheads[key])}.");
        // }

        Console.WriteLine($"Result: {result}.");
    }

    static (int[,], List<(int, int)>) LoadMapFromFile(string inputPath) {
        var lines = File.ReadAllLines(inputPath);
        var N = lines.Length;
        var M = lines[0].Length;
        var map = new int[N,M];
        var trailheads = new List<(int, int)>();
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < M; j++) {
                map[i,j] = Int32.Parse(lines[i][j].ToString());
                if (map[i,j] == 0) {
                    trailheads.Add((i,j));
                }
            }
        }
        return (map, trailheads);
    }

    static List<(int, int)> GetPossibleNextPositions(int[,] map, (int, int) position) {
        var nextPositions = new List<(int, int)>();
        foreach (var move in new List<(int, int)> { (-1, 0), (0, 1), (1, 0), (0, -1) }) {
            var nextPosition = (position.Item1 + move.Item1, position.Item2 + move.Item2);
            if (IsOnTheMap(nextPosition, map) && map[nextPosition.Item1, nextPosition.Item2] == map[position.Item1, position.Item2] + 1) {
                nextPositions.Add(nextPosition);
            }
        }
        return nextPositions;
    }

    static bool IsOnTheMap((int, int) position, int[,] map) {
        if (position.Item1 < 0 || position.Item1 >= map.GetLength(0) || position.Item2 < 0 || position.Item2 >= map.GetLength(1)) {
            return false;
        }
        return true;
    }

    static void GetScore(int[,] map, (int, int) currentPosition, (int, int) initialPosition, Dictionary<(int, int), List<(int, int)>> peaks) {
        // Console.WriteLine($"Current position: {currentPosition}.");
        if (map[currentPosition.Item1, currentPosition.Item2] == 9) {
            peaks[initialPosition].Add(currentPosition);
            return;
        }
        var nextPositions = GetPossibleNextPositions(map, currentPosition);
        // Console.WriteLine($"Current next positions: {string.Join(",", nextPositions)}.");
        if (!nextPositions.Any()) {
            return;
        }
        foreach (var nextPosition in nextPositions) {
            GetScore(map, nextPosition, initialPosition, peaks);
        }
        return;
    }
}
