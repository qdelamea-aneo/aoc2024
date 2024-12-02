class Program {
    static void Main() {
        string inputPath = "../data/day_02/input.txt";
        int result = File
            .ReadLines(inputPath)
            .Where(line => !string.IsNullOrEmpty(line))
            .Select(line => line.Split(' ').Select(Int32.Parse))
            .Where(report => report
                .Select((_, index) => report.Where((_, i) => i != index))
                .Any(IsReportSafe))
            .Count();
        
        Console.WriteLine($"Result: {result}.");
    }

    static bool IsReportSafe(IEnumerable<int> report)
    {
        var enumerator = report.GetEnumerator();
        if (!enumerator.MoveNext())
            return false; // Ensure the list is not empty.

        int prev = enumerator.Current;

        // Determine the direction of the list: ascending or descending
        if (!enumerator.MoveNext())
            return true; // A single-element list is safe.

        int next = enumerator.Current;
        bool isAscending = next > prev;
        bool isDescending = next < prev;

        // If the first two elements are equal, the list is invalid
        if ((!isAscending && !isDescending) || Math.Abs(prev - next) > 3)
            return false;

        prev = next;

        while (enumerator.MoveNext())
        {
            next = enumerator.Current;

            if (isAscending && next <= prev)
                return false;

            if (isDescending && next >= prev)
                return false;

            if (Math.Abs(next - prev) > 3)
                return false;

            prev = next;
        }

        return true;
    }
}
