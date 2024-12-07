using System.Threading.Tasks;


public class Map {
    private char[,] map;
    private (int,int) position;
    private (int, int) dimensions;
    private (int, int) direction;
    private List<(int, int, int, int)> history;

    public Map(string inputPath) {
        var lines = File.ReadAllLines(inputPath);
        int N = lines.Length;
        int M = lines[0].Length;
        var charMatrix = new char[N,M];
        dimensions = (N, M);
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < M; j++) {
                charMatrix[i,j] = lines[i][j];
                if (charMatrix[i,j] == '^') {
                    position = (i,j);
                    direction = (-1,0);
                }
            }
        }
        map = charMatrix;
        history = new List<(int, int, int, int)> {(position.Item1, position.Item2, direction.Item1, direction.Item2)};
    }

    public bool WayIsClear() {
        try {
            if (map[position.Item1 + direction.Item1, position.Item2 + direction.Item2] == '#') {
                return false;
            }
            return true;
        }
        catch (IndexOutOfRangeException) {
            return true;
        }
    }

    public bool GuardLeftTheMap() {
        if (position.Item1 < 0 || position.Item1 >= dimensions.Item1 || position.Item2 < 0 || position.Item2 >= dimensions.Item2) {
            return true;
        }
        return false;
    }

    public void PrintPosition() {
        Console.WriteLine($"Position: {position.Item1},{position.Item2}");
    }

    public void MoveForward() {
        map[position.Item1, position.Item2] = 'X';
        position.Item1 += direction.Item1;
        position.Item2 += direction.Item2;
    }

    private void UpdateHistory() {
            history.Add((position.Item1,position.Item2,direction.Item1,direction.Item2));
    }

    public void TurnRight() {
        if (direction.Item1 == -1 && direction.Item2 == 0) {
            direction.Item1 = 0;
            direction.Item2 = 1;
            return;
        } else if (direction.Item1 == 0 && direction.Item2 == 1) {
            direction.Item1 = 1;
            direction.Item2 = 0;
        } else if (direction.Item1 == 1 && direction.Item2 == 0) {
            direction.Item1 = 0;
            direction.Item2 = -1;
        } else if (direction.Item1 == 0 && direction.Item2 == -1) {
            direction.Item1 = -1;
            direction.Item2 = 0;
        } else {
            throw new Exception("Incompatible value.");
        }
    }

    public int CountExploredPositions() {
        int count = 0;
        for (int i = 0; i < dimensions.Item1; i++) {
            for (int j = 0; j < dimensions.Item2; j++) {
                if (map[i,j] == 'X') {
                    count++;
                }
            }
        }
        return count;
    }

    public IEnumerable<(int, int)> GetFreePositions() {
        for (int i = 0; i < dimensions.Item1; i++) {
            for (int j = 0; j < dimensions.Item2; j++) {
                if (map[i,j] == '.') {
                    yield return (i,j);
                }
            }
        }
    }

    public void AddObstacle((int, int) position) {
        map[position.Item1, position.Item2] = '#';
    }

    private void PrintHistory() {
        Console.WriteLine($"History: {string.Join(",", history)}");
    }

    public bool IsGuardStuck() {
        while (!GuardLeftTheMap()) {
            // PrintHistory();
            if (WayIsClear()) {
                MoveForward();
                if (history.Contains((position.Item1, position.Item2, direction.Item1, direction.Item2))) {
                    return true;
                }
                UpdateHistory();
            } else {
                TurnRight();
                if (history.Contains((position.Item1, position.Item2, direction.Item1, direction.Item2))) {
                    return true;
                }
                UpdateHistory();
            }
        }
        return false;
    }
}


static class Program {
    public static void Main() {
        string inputPath = "../data/day_06/input.txt";
        var possibleObstaclePositions = new Map(inputPath).GetFreePositions();
        var tasks = possibleObstaclePositions
            .Select(possibleObstaclePosition => Task.Run(() => {
                var map = new Map(inputPath);
                map.AddObstacle(possibleObstaclePosition);
                return map.IsGuardStuck() ? 1 : 0;
            }))
            .ToArray();

        Task.WaitAll(tasks);
        int result = tasks.Sum(task => task.Result);

        Console.WriteLine($"Result: {result}.");
    }
}
