using System.Runtime.InteropServices;

class Region(long w, long h, int[] shapeCounts)
{
    public long width = w;
    public long height = h;
    public int[] shapeCounts = shapeCounts;
    public long Area => width * height;
}


class Day12
{
    static char[,] ToCharGrid(List<string> lines)
    {
        int h = lines.Count;
        int w = lines[0].Length;
        var grid = new char[h, w];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                grid[y, x] = lines[y][x];

        return grid;
    }
    static (List<Region> regions, List<char[,]> shapes) ParseInput(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var shapes = new List<char[,]>();
        var regions = new List<Region>();

        int i = 0;
        while (i < lines.Length && !lines[i].Contains('x'))
        {
            if (string.IsNullOrWhiteSpace(lines[i]) || lines[i].Contains(':'))
            {
                i++;
                continue;
            }

            var shapeLines = new List<string>();
            while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
            {
                shapeLines.Add(lines[i]);
                i++;
            }

            shapes.Add(ToCharGrid(shapeLines));
        }

        for (; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var dims = parts[0]
                .TrimEnd(':')
                .Split('x')
                .Select(long.Parse)
                .ToArray();

            var counts = parts
                .Skip(1)
                .Select(int.Parse)
                .ToArray();

            regions.Add(new Region(dims[0], dims[1], counts));
        }

        return (regions, shapes);
    }

    static long[] GetFactors(long n)
    {
        long[] factors = [];
        for (long i = 1; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
            {
                factors = [.. factors, i];
                if (i != n / i)
                {
                    factors = [.. factors, (n / i)];
                }
            }
        }
        return factors;
    }

    static long Part1(List<Region> regions, List<char[,]> shapes)
    {
        long count = 0;

        int[] shapeSizes = [.. shapes.Select(s => s.Cast<char>().Count(c => c == '#'))];

        foreach (var region in regions)
        {
            var shapeArea = shapeSizes.Select((size, index) => size * region.shapeCounts[index]).Sum();
            var boxes = (region.width / 3) * (region.height / 3);
            if (region.shapeCounts.Sum() <= boxes) count++;
            else if (shapeArea > region.Area) continue;
            else
            {
                // uhhhhh, this never gets called? lol
            }

        }

        return count;
    }

    static long Part2(List<Region> regions)
    {
        return 0;
    }

    public static void Run()
    {
        var (regions, shapes) = ParseInput("Day12.txt");
        Console.WriteLine("Day 12 Solution -- Part 01:");
        Console.WriteLine(Part1(regions, shapes));
        Console.WriteLine("Day 12 Solution -- Part 02:");
        Console.WriteLine(Part2(regions));
    }
}