class Day05
{
    static long Part1()
    {
        string[] lines = File.ReadAllLines("Day05.txt");
        int splitIndice = Array.IndexOf(lines, "");
        List<(long start, long end)> ranges = [.. lines.Take(splitIndice).Select(
            line => {
                string[] parts = line.Split('-');
                return (long.Parse(parts[0]), long.Parse(parts[1]));
            }
        )];
        long[] available = [.. lines.Skip(splitIndice + 1).Select(long.Parse)];
        ranges.Sort((a, b) => a.start.CompareTo(b.start));
        List<(long start, long end)> merged = [];
        foreach (var range in ranges)
        {
            if (merged.Count == 0 || merged[^1].end < range.start - 1)
            {
                merged.Add(range);
            }
            else
            {
                var (start, end) = merged[^1];
                merged[^1] = (start, Math.Max(end, range.end));
            }
        }
        ranges = merged;

        long count = 0;
        foreach (var id in available)
        {
            if (ranges.Any(r => id >= r.start && id <= r.end))
            {
                count += 1;
            }
        }
        return count;
    }
    static long Part2()
    {
        //i saw this one coming lol
        string[] lines = File.ReadAllLines("Day05.txt");
        int splitIndice = Array.IndexOf(lines, "");
        List<(long start, long end)> ranges = [.. lines.Take(splitIndice).Select(
            line => {
                string[] parts = line.Split('-');
                return (long.Parse(parts[0]), long.Parse(parts[1]));
            }
        )];
        long[] available = [.. lines.Skip(splitIndice + 1).Select(long.Parse)];
        ranges.Sort((a, b) => a.start.CompareTo(b.start));
        List<(long start, long end)> merged = [];
        foreach (var range in ranges)
        {
            if (merged.Count == 0 || merged[^1].end < range.start - 1)
            {
                merged.Add(range);
            }
            else
            {
                var (start, end) = merged[^1];
                merged[^1] = (start, Math.Max(end, range.end));
            }
        }
        ranges = merged;
        return ranges.Aggregate(0L, (acc, r) => acc + r.end - r.start + 1);
    }
    public static void Run()
    {
        Console.WriteLine("Day 05 Solution -- Part 01:");
        Console.WriteLine(Part1());
        Console.WriteLine("Day 05 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}