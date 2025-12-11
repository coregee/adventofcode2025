class Network
{
    readonly Dictionary<string, string[]> cables = [];
    public Network(string filename)
    {

    }
}

class Day11
{
    static Dictionary<string, List<string>> ParseInput(string filename)
    {
        Dictionary<string, List<string>> cables = [];
        var lines = File.ReadAllLines(filename);
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var origin = parts[0][..^1];
            var destinations = parts[1..];
            cables[origin] = [.. destinations];
        }
        return cables;
    }

    static long Part1()
    {
        var cables = ParseInput("Day11.txt");

        Dictionary<string, long> counts = [];

        long GetNPaths(string node)
        {
            if (node == "out") return 1;
            if (counts.TryGetValue(node, out long value)) return value;
            if (!cables.TryGetValue(node, out List<string>? value1))
            {
                counts[node] = 0;
                return 0;
            }
            long count = 0;
            foreach (var adj in value1) count += GetNPaths(adj);
            counts[node] = count;
            return count;
        }
        return GetNPaths("you");
    }

    static long Part2()
    {
        var cables = ParseInput("Day11.txt");

        Dictionary<string, long> counts;

        long GetNPaths(string start, string end)
        {
            if (start == end) return 1;
            if (counts.TryGetValue(start, out long value)) return value;
            if (!cables.TryGetValue(start, out List<string>? value1))
            {
                counts[start] = 0;
                return 0;
            }
            long count = 0;
            foreach (var adj in value1) count += GetNPaths(adj, end);
            counts[start] = count;
            return count;
        }
        // svr-dac
        counts = [];
        var svr_dac = GetNPaths("svr", "dac");
        // svr-fft
        counts = [];
        var svr_fft = GetNPaths("svr", "fft");
        // dac-fft
        counts = [];
        var dac_fft = GetNPaths("dac", "fft");
        // fft-dac
        counts = [];
        var fft_dac = GetNPaths("fft", "dac");
        // fft-out
        counts = [];
        var fft_out = GetNPaths("fft", "out");
        // dac-out
        counts = [];
        var dac_out = GetNPaths("dac", "out");

        // svr - dac - fft - out
        var routeA = svr_dac * dac_fft * fft_out;
        // svr - fft - dac - out
        var routeB = svr_fft * fft_dac * dac_out;
        return routeA + routeB;
    }
    public static void Run()
    {
        Console.WriteLine("Day 11 Solution -- Part 01:");
        Console.WriteLine(Part1());
        Console.WriteLine("Day 11 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}