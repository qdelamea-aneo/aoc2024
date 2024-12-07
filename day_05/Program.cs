static class Program {
    public static void Main() {
        string inputPath = "../data/day_05/input.txt";
        var rules = new Dictionary<int,List<int>>();
        var printings = new List<List<int>>();
        int result = 0;

        foreach (var line in File.ReadLines(inputPath)) {
            if (line.Contains("|")) {
                var rule = line.Split("|").Select(Int32.Parse).ToList();
                var key = rule[0];
                var value = rule[1];
                if (rules.ContainsKey(key)) {
                    rules[key].Add(value);
                } else {
                    rules.Add(key, new List<int>{value});
                }
            } else if (line.Contains(",")) {
                printings.Add(line.Split(",").Select(Int32.Parse).ToList());
            }
        }
        
        foreach (var printing in printings) {
            bool isNotCorrect = printing
                .SelectMany((p, i) => printing.Skip(i + 1), (p1, p2) => (p1, p2))
                .Any(pageCouple => {
                    try {
                        return rules[pageCouple.Item2].Contains(pageCouple.Item1);
                    } catch (KeyNotFoundException) {
                        return false;
                    }
                });
            if (isNotCorrect) {
                printing.Sort((p1, p2) => {
                    try {
                        if (rules[p1].Contains(p2)) return -1;
                        return 1;
                    } catch (KeyNotFoundException) {
                        try {
                            if (rules[p2].Contains(p1)) return 1;
                            return -1;
                        } catch (KeyNotFoundException) {
                            return 0;
                        }
                    }
                });
                result += printing[printing.Count() / 2];
            }
        }
        Console.WriteLine($"Result: {result}.");
    }
}
