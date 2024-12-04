using System.Text.RegularExpressions;


static class Program {
    static void Main() {
        string inputPath = "../data/day_04/input.txt";
        string word = "XMAS";
        string reversedWord = new string(word.ToCharArray().Reverse().ToArray());
        int result = ParseInput(inputPath)
            .IterRawsColumnsAndDiagonals()
            .Where(s => s.Length >= 4)
            .Select(s => Regex.Matches(s, word).Count() + Regex.Matches(s, reversedWord).Count())
            .Sum();
        
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
}


public static class CharMatrixExtensions {
    public static IEnumerable<string> IterRawsColumnsAndDiagonals(this char[,] charMatrix) {
        int n = charMatrix.GetLength(0);
        for (int i = 0; i < n; i++) {
            yield return charMatrix.GetRaw(i);
            yield return charMatrix.GetCol(i);
            yield return charMatrix.GetDiag(i);
            yield return charMatrix.GetDiag(i, true);
            if (i > 0) {
                yield return charMatrix.GetDiag(-1 * i);
                yield return charMatrix.GetDiag(-1 * i, true);
            }
        }
    }

    public static string GetRaw(this char[,] charMatrix, int rawIndex) {
        int n = charMatrix.GetLength(0);
        char[] raw = new char[n];
        for (int j = 0; j < n; j++) {
            raw[j] = charMatrix[rawIndex, j];
        }
        return new string(raw);
    }

    public static string GetCol(this char[,] charMatrix, int colIndex) {
        int n = charMatrix.GetLength(0);
        char[] col = new char[n];
        for (int j = 0; j < n; j++) {
            col[j] = charMatrix[j, colIndex];
        }
        return new string(col);
    }

    public static string GetDiag(this char[,] charMatrix, int diagIndex, bool transpose = false) {
        int n = charMatrix.GetLength(0);
        int m = Math.Abs(diagIndex);
        char[] diag = new char[n - m];
        for (int j = 0; j < n - m; j++) {
            if (diagIndex >= 0) {
                if (transpose) {
                    diag[j] = charMatrix[j + diagIndex, n - 1 - j];
                } else {
                    diag[j] = charMatrix[j, j + diagIndex];
                }
            } else {
                if (transpose) {
                    diag[j] = charMatrix[j, n - 1 - j + diagIndex];
                } else {
                    diag[j] = charMatrix[j - diagIndex, j];
                }
            }
        }
        return new string(diag);
    }
}
