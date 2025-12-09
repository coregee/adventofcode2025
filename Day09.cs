class Tile(int x, int y)
{
    public int x = x;
    public int y = y;
}

class Day09
{
    static long Part1()
    {
        var lines = File.ReadAllLines("Day09.txt");
        var tiles = new List<Tile>();
        lines.Select(line => line.Split(',')).ToList().ForEach(coords =>
        {
            tiles.Add(new Tile(int.Parse(coords[0]), int.Parse(coords[1])));
        });

        var areas = new List<(long area, Tile a, Tile b)>();
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = i + 1; j < tiles.Count; j++)
            {
                var tileA = tiles[i];
                var tileB = tiles[j];
                long width = Math.Abs(tileA.x - tileB.x) + 1;
                long height = Math.Abs(tileA.y - tileB.y) + 1;
                long area = width * height;
                areas.Add((area, tileA, tileB));
            }
        }
        areas.Sort((a, b) => b.area.CompareTo(a.area));
        return areas[0].area;
    }

    // "oh wow guys advent of code is so easy this year lol" oh, hello day 9 part 2.
    static long Part2()
    {
        var lines = File.ReadAllLines("Day09.txt");
        var bigTiles = new List<Tile>();
        lines.Select(line => line.Split(',')).ToList().ForEach(coords =>
        {
            bigTiles.Add(new Tile(int.Parse(coords[0]), int.Parse(coords[1])));
        });

        var tiles = bigTiles.Select(t => new Tile(t.x, t.y)).ToList();

        // Collapse tile indices; 1-index for flood-fill later
        var xMap = tiles.Select(t => t.x).Distinct().Order().ToList().Select((x, i) => (x, i)).ToDictionary(p => p.x, p => p.i + 1);
        var yMap = tiles.Select(t => t.y).Distinct().Order().ToList().Select((y, i) => (y, i)).ToDictionary(p => p.y, p => p.i + 1);
        tiles.ForEach(t =>
        {
            t.x = xMap[t.x];
            t.y = yMap[t.y];
        });

        // Make an empty grid; +1 flood fill
        var xMax = tiles.Max(t => t.x);
        var yMax = tiles.Max(t => t.y);
        var grid = new List<List<char>>();
        for (int y = 0; y <= yMax + 1; y++)
        {
            var row = new List<char>();
            for (int x = 0; x <= xMax + 1; x++)
            {
                row.Add('.');
            }
            grid.Add(row);
        }

        // Fill the grid with a perimeter bound by subsequent tile pairs
        for (int i = 0; i < tiles.Count; i++)
        {
            var start = tiles[i];
            var end = tiles[(i + 1) % tiles.Count];
            if (start.x == end.x)
            {
                for (int y = Math.Min(start.y, end.y); y <= Math.Max(start.y, end.y); y++)
                {
                    grid[y][start.x] = 'X';
                }
            }
            else if (start.y == end.y)
            {
                for (int x = Math.Min(start.x, end.x); x <= Math.Max(start.x, end.x); x++)
                {
                    grid[start.y][x] = 'X';
                }
            }
        }
        tiles.ForEach(t => grid[t.y][t.x] = '#');

        // Fill the outside
        var stack = new Stack<(int x, int y)>();
        stack.Push((0, 0));
        while (stack.Count > 0)
        {
            var (x, y) = stack.Pop();
            if (x < 0 || x >= grid[0].Count || y < 0 || y >= grid.Count)
                continue;
            if (grid[y][x] != '.')
                continue;
            grid[y][x] = ':';
            stack.Push((x + 1, y));
            stack.Push((x - 1, y));
            stack.Push((x, y + 1));
            stack.Push((x, y - 1));
        }

        var areas = new List<(long area, Tile a, Tile b)>();
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = i + 1; j < tiles.Count; j++)
            {
                var tileA = tiles[i];
                var tileB = tiles[j];
                // Fetch area from original tile coordinates
                var oA = bigTiles[i];
                var oB = bigTiles[j];
                long width = Math.Abs(oA.x - oB.x) + 1;
                long height = Math.Abs(oA.y - oB.y) + 1;
                long area = width * height;
                areas.Add((area, tileA, tileB));
            }
        }
        areas.Sort((a, b) => b.area.CompareTo(a.area));

        for (int i = 0; i < areas.Count; i++)
        {
            var (area, a, b) = areas[i];
            if (ValidRectangle(grid, a, b))
                return area;
        }
        return 0;
    }

    static bool ValidRectangle(List<List<char>> grid, Tile a, Tile b)
    {
        var topLeft = (x: Math.Min(a.x, b.x), y: Math.Min(a.y, b.y));
        var bottomRight = (x: Math.Max(a.x, b.x), y: Math.Max(a.y, b.y));

        for (int y = topLeft.y; y <= bottomRight.y; y++)
        {
            for (int x = topLeft.x; x <= bottomRight.x; x++)
            {
                if (grid[y][x] == ':')
                    return false;
            }
        }
        return true;
    }

    public static void Run()
    {
        Console.WriteLine("Day 09 Solution -- Part 01:");
        Console.WriteLine(Part1());
        Console.WriteLine("Day 09 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}