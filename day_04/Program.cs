using System.Text.RegularExpressions;


static class Program {
    static void Main() {
        string inputPath = "../data/day_04/input.txt";
        var charMatrix = ParseInput(inputPath);
        int result = 0;
        for (int i = 1; i < charMatrix.GetLength(0) - 1; i++) {
            for (int j = 1; j < charMatrix.GetLength(1) - 1; j++) {
                if (charMatrix[i,j] == 'A' && IsMas(charMatrix, new Tuple<int, int>(i-1,j-1), new Tuple<int, int>(i+1,j+1)) && IsMas(charMatrix, new Tuple<int, int>(i-1,j+1), new Tuple<int, int>(i+1,j-1))) {
                    result++;
                }
            }
        }

        Console.WriteLine($"Result: {result}.");
    }

    public static char[,] ParseInput(string inputPath) {
        var lines = File.ReadAllLines(inputPath);
        int N = lines.Length;
        var charMatrix = new char[N,N];
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) {
                charMatrix[i,j] = lines[i][j];
            }
        }
        return charMatrix;
    }

    public static bool IsMas(char[,] charMatrix, Tuple<int,int> firstIndices, Tuple<int,int> secondIndices) {
        if ((charMatrix[firstIndices.Item1, firstIndices.Item2] == 'M' && charMatrix[secondIndices.Item1, secondIndices.Item2] == 'S') || (charMatrix[firstIndices.Item1, firstIndices.Item2] == 'S' && charMatrix[secondIndices.Item1, secondIndices.Item2] == 'M')) {
            return true;
        }
        return false;
    }
}
