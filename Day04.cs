class Grid
{
    public char[][] cells;
    public int width = 0;
    public int height = 0;

    public Grid(string[] inputLines)
    {
        cells = new char[inputLines.Length][];
        for (int i = 0; i < inputLines.Length; i++)
        {
            cells[i] = inputLines[i].ToCharArray();
        }
        width = cells[0].Length;
        height = cells.Length;
    }
    public List<(int x, int y)> AdjacentCoords(int x, int y)
    {
        List<(int x, int y)> adjacent = [];
        var n = NCoords(x, y);
        var s = SCoords(x, y);
        var w = WCoords(x, y);
        var e = ECoords(x, y);
        var ne = NECoords(x, y);
        var nw = NWCoords(x, y);
        var se = SECoords(x, y);
        var sw = SWCoords(x, y);
        if (n != null) adjacent.Add(n.Value);
        if (s != null) adjacent.Add(s.Value);
        if (w != null) adjacent.Add(w.Value);
        if (e != null) adjacent.Add(e.Value);
        if (ne != null) adjacent.Add(ne.Value);
        if (nw != null) adjacent.Add(nw.Value);
        if (se != null) adjacent.Add(se.Value);
        if (sw != null) adjacent.Add(sw.Value);
        return adjacent;
    }
    public (int x, int y)? NCoords(int x, int y)
    {
        if (y - 1 < 0)
            return null;
        return (x, y - 1);
    }
    public (int x, int y)? SCoords(int x, int y)
    {
        if (y + 1 >= height)
            return null;
        return (x, y + 1);
    }
    public (int x, int y)? WCoords(int x, int y)
    {
        if (x - 1 < 0)
            return null;
        return (x - 1, y);
    }
    public (int x, int y)? ECoords(int x, int y)
    {
        if (x + 1 >= width)
            return null;
        return (x + 1, y);
    }
    public (int x, int y)? NECoords(int x, int y)
    {
        if (y - 1 < 0 || x + 1 >= width)
            return null;
        return (x + 1, y - 1);
    }
    public (int x, int y)? NWCoords(int x, int y)
    {
        if (y - 1 < 0 || x - 1 < 0)
            return null;
        return (x - 1, y - 1);
    }
    public (int x, int y)? SECoords(int x, int y)
    {
        if (y + 1 >= height || x + 1 >= width)
            return null;
        return (x + 1, y + 1);
    }
    public (int x, int y)? SWCoords(int x, int y)
    {
        if (y + 1 >= height || x - 1 < 0)
            return null;
        return (x - 1, y + 1);
    }

    public char[] Adjacent(int x, int y)
    {
        List<char> adjacent = [];
        char? n = N(x, y);
        char? s = S(x, y);
        char? w = W(x, y);
        char? e = E(x, y);
        char? ne = NE(x, y);
        char? nw = NW(x, y);
        char? se = SE(x, y);
        char? sw = SW(x, y);
        if (n != null) adjacent.Add((char)n);
        if (s != null) adjacent.Add((char)s);
        if (w != null) adjacent.Add((char)w);
        if (e != null) adjacent.Add((char)e);
        if (ne != null) adjacent.Add((char)ne);
        if (nw != null) adjacent.Add((char)nw);
        if (se != null) adjacent.Add((char)se);
        if (sw != null) adjacent.Add((char)sw);
        return [.. adjacent];
    }

    public char? N(int x, int y)
    {
        if (y - 1 < 0)
            return null;
        return cells[y - 1][x];
    }
    public char? S(int x, int y)
    {
        if (y + 1 >= height)
            return null;
        return cells[y + 1][x];
    }
    public char? W(int x, int y)
    {
        if (x - 1 < 0)
            return null;
        return cells[y][x - 1];
    }

    public char? E(int x, int y)
    {
        if (x + 1 >= width)
            return null;
        return cells[y][x + 1];
    }
    public char? NE(int x, int y)
    {
        if (y - 1 < 0 || x + 1 >= width)
            return null;
        return cells[y - 1][x + 1];
    }
    public char? NW(int x, int y)
    {
        if (y - 1 < 0 || x - 1 < 0)
            return null;
        return cells[y - 1][x - 1];
    }
    public char? SE(int x, int y)
    {
        if (y + 1 >= height || x + 1 >= width)
            return null;
        return cells[y + 1][x + 1];
    }
    public char? SW(int x, int y)
    {
        if (y + 1 >= height || x - 1 < 0)
            return null;
        return cells[y + 1][x - 1];
    }
}

class Day04
{
    public static long Part1()
    {
        string[] inputLines = File.ReadAllLines("Day04.txt");
        Grid grid = new(inputLines);
        long count = 0;
        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                char c = grid.cells[y][x];
                if (c != '@') continue;
                char[] adjacent = grid.Adjacent(x, y);
                if (adjacent.Where(c => c == '@').Count() < 4)
                {
                    count += 1;
                }
            }
        }
        return count;
    }

    public static List<(int x, int y)> CheckAdjacent(Grid grid, int x, int y)
    {
        List<(int x, int y)> toUpdate = [];
        List<(int x, int y)> adjacentCoords = grid.AdjacentCoords(x, y);
        foreach (var coords in adjacentCoords)
        {
            int ax = coords.x;
            int ay = coords.y;
            char c = grid.cells[ay][ax];
            char[] adjacent = grid.Adjacent(ax, ay);
            if (c == '@' && adjacent.Where(a => a == '@').Count() < 4)
            {
                toUpdate.Add((ax, ay));
            }
        }
        return toUpdate;
    }

    // This is pretty inelegant, but if it works...
    public static long Part2()
    {
        string[] inputLines = File.ReadAllLines("Day04.txt");
        Grid grid = new(inputLines);
        long count = 0;
        var toUpdate = new List<(int x, int y)>();
        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                char c = grid.cells[y][x];
                if (c != '@') continue;
                char[] adjacent = grid.Adjacent(x, y);
                if (adjacent.Where(c => c == '@').Count() < 4)
                {
                    count += 1;
                    toUpdate.Add((x, y));
                }
            }
        }
        bool done = false;
        while (!done)
        {
            foreach (var entry in toUpdate)
            {
                int x = entry.x;
                int y = entry.y;
                grid.cells[y][x] = '.';
            }
            var checkAdjacent = toUpdate;
            toUpdate = [];
            foreach (var entry in checkAdjacent)
            {
                int x = entry.x;
                int y = entry.y;
                var toChange = CheckAdjacent(grid, x, y);
                toUpdate.AddRange(toChange);
            }
            toUpdate = [.. toUpdate.Distinct()];
            count += toUpdate.Count;
            if (toUpdate.Count == 0)
            {
                done = true;
            }
        }
        return count;
    }

    public static void Run()
    {
        Console.WriteLine("Day 04 Solution -- Part 01:");
        Console.WriteLine(Part1());
        Console.WriteLine("Day 04 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}