static class Program {
    static void Main() {
        string inputPath = "../data/day_09/input.txt";
        var disk = File
            .ReadAllText(inputPath)
            .Where(c => c != '\n')
            .Select(c => Int32.Parse(c.ToString()))
            .Select((n, i) => {
                if (i % 2 == 0) {
                    return Enumerable.Repeat(i / 2, n);
                } else {
                    return Enumerable.Repeat(-1, n);
                }
            })
            .SelectMany(list => list)
            .ToArray();

        var freeBlocks = FreeBlocks(disk);
        var fileBlocks = FileBlocks((int[])disk.Clone());

        foreach (var (fileBlockStart, fileBlockLength) in fileBlocks) {
            foreach (var (freeBlockStart, freeBlockLength) in freeBlocks) {
                if (freeBlockStart > fileBlockStart) {
                    break;
                }
                if (freeBlockLength > fileBlockLength) {
                    Array.Copy(disk, fileBlockStart, disk, freeBlockStart, fileBlockLength);
                    Array.Copy(Enumerable.Repeat(-1, fileBlockLength).ToArray(), 0, disk, fileBlockStart, fileBlockLength);
                }
            }
        }

        long result = disk
            .Select((n, i) => n != -1 ? (long)(n * i) : 0)
            .Sum();

        // Console.WriteLine(string.Join(",", disk));
        Console.WriteLine($"Result: {result}.");
    }

    static IEnumerable<(int, int)> FreeBlocks(int[] disk) {
        int i = 0;
        while (i < disk.Length) {
            if (disk[i] == -1) {
                var start = i;
                while (i < disk.Length && disk[i] == -1) {
                    i++;
                }
                yield return (start, i - start + 1);
            } else {
                i++;
            }
        }
    }

    static IEnumerable<(int, int)> FileBlocks(int[] disk) {
        int i = disk.Length - 1;
        while (i >= 0) {
            if (disk[i] != -1) {
                int id = disk[i];
                var end = i;
                while (i >= 0 && disk[i] == id) {
                    i--;
                }
                yield return (i + 1, end - i);
            } else {
                i--;
            }
        }
    }
}
