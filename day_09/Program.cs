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
            .ToList();

        var freeBlocks = FreeBlockPositions(disk);
        var fileBlocks = FinalFileBlockPositions(disk);

        foreach (var (freeBlock, fileBlock) in freeBlocks.Zip(fileBlocks, (freeBlock, fileBlock) => (freeBlock, fileBlock))) {
            if (freeBlock > fileBlock) {
                break;
            }
            disk[freeBlock] = disk[fileBlock];
            disk[fileBlock] = -1;
        }

        long result = disk
            .TakeWhile(n => n >= 0)
            .Select((n, i) => (long)(n * i))
            .Sum();

        // Console.WriteLine(string.Join(",", disk));
        Console.WriteLine($"Result: {result}.");
    }

    static IEnumerable<int> FreeBlockPositions(List<int> disk) {
        for (int i = 0; i < disk.Count(); i++) {
            if (disk[i] == -1) {
                yield return i;
            }
        }
    }

    static IEnumerable<int> FinalFileBlockPositions(List<int> disk) {
        for (int i = disk.Count() - 1; i >= 0; i--) {
            if (disk[i] != -1) {
                yield return i;
            }
        }
    }
}
