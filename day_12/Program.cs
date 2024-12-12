class Region {

    public char plantType { get; private set; }
    public IEnumerable<(int, int)> plantPositions { get; private set; }

    public Region(char plantType, IEnumerable<(int, int)> plantPositions) {
        this.plantType = plantType;
        this.plantPositions = plantPositions;
    }

    public int Area {
        get {
            return plantPositions.Count();
        }
    }

    public int Perimeter {
        get {
            return 4 * plantPositions.Count() - 2 * plantPositions
                                                .SelectMany((_, i) => plantPositions.Skip(i + 1), (x, y) => (x, y))
                                                .Where(pair => Math.Abs(pair.x.Item1 - pair.y.Item1) == 1 && pair.x.Item2 == pair.y.Item2 || Math.Abs(pair.x.Item2 - pair.y.Item2) == 1 && pair.x.Item1 == pair.y.Item1)
                                                .Count();
        }
    }

    public int Price {
        get {
            return Area * Perimeter;
        }
    }

    public static Region Merge(Region r1, Region r2) {
        if (r1.plantType != r2.plantType) {
            throw new Exception("Can't merge two regions with different plant type.");
        }
        return new Region(r1.plantType, r1.plantPositions.Concat(r2.plantPositions).Distinct());
    }

    public static IEnumerable<Region> FromString(string s, int raw) {
        if (string.IsNullOrEmpty(s)) {
            yield break;
        }
        var currentType = s[0];
        var positions = new List<(int, int)> { (raw, 0) };
        for (int i = 1; i < s.Length; i++) {
            if (s[i] == currentType) {
                positions.Add((raw, i));
            } else {
                yield return new Region(currentType, positions);
                currentType = s[i];
                positions = new List<(int, int)> { (raw, i) };
            }
        }
        yield return new Region(currentType, positions);
    }

    public bool ShareVerticalBorder(Region r) {
        foreach (var pos1 in this.plantPositions) {
            foreach (var pos2 in r.plantPositions) {
                if (Math.Abs(pos1.Item1 - pos2.Item1) == 1 && pos1.Item2 == pos2.Item2) {
                    return true;
                }
            }
        }
        return false;
    }
}


static class Program {
    static void Main() {
        var inputPath = "../data/day_12/input.txt";
        var lines = File.ReadAllLines(inputPath);

        var regions = new List<Region>();
        var borderRegions = new List<Region>();

        for(int i = 0; i < lines.Length; i++) {
            var currentRegions = Region.FromString(lines[i], i).ToList();
            var newBorderRegion = new List<Region>();
            for (int k = 0; k < currentRegions.Count(); k++) {
                var mergingBerderRegions = borderRegions.Where(r => r.plantType == currentRegions[k].plantType && r.ShareVerticalBorder(currentRegions[k])).ToList();
                if (mergingBerderRegions.Any()) {
                    borderRegions = borderRegions.Except(mergingBerderRegions).ToList();
                    newBorderRegion = newBorderRegion.Except(mergingBerderRegions).ToList();
                    mergingBerderRegions.Add(currentRegions[k]);
                    var newRegion = mergingBerderRegions.Aggregate(Region.Merge);
                    borderRegions.Add(newRegion);
                    newBorderRegion.Add(newRegion);
                } else {
                    newBorderRegion.Add(currentRegions[k]);
                }
            }
            regions.AddRange(borderRegions.Except(newBorderRegion));
            borderRegions = newBorderRegion;
        }
        regions.AddRange(borderRegions);

        // foreach (var r in regions) {
        //     Console.WriteLine($"{r.plantType}, area = {r.Area}, perimeter = {r.Perimeter}, price = {r.Price}");//\npositions = {string.Join(",", r.plantPositions)}");
        // }

        Console.WriteLine($"Result: {regions.Sum(r => r.Price)}.");
    }
}
