using System.Numerics;
using System.Security.Cryptography.X509Certificates;

class Coord(long x, long y, long z)
{
    private static long _counter = 0;
    public long id = _counter++;
    public long x = x;
    public long y = y;
    public long z = z;

    public decimal DistanceTo(Coord other)
    {
        return (decimal)Math.Sqrt(
            Math.Pow(x - other.x, 2) +
            Math.Pow(y - other.y, 2) +
            Math.Pow(z - other.z, 2)
        );
    }
}

class Day08
{
    static BigInteger Part1()
    {
        List<Coord> boxes = [.. File.ReadAllLines("Day08.txt")
            .Select(line =>
            {
                long[] vals = [.. line.Split(',').Select(long.Parse)];
                return new Coord(vals[0], vals[1], vals[2]);
            })
        ];

        List<(decimal dist, Coord a, Coord b)> distanceMap = [];
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = i + 1; j < boxes.Count; j++)
            {
                decimal dist = boxes[i].DistanceTo(boxes[j]);
                distanceMap.Add((dist, boxes[i], boxes[j]));
            }
        }
        distanceMap = [.. distanceMap.OrderBy(t => t.dist)];

        List<(long a, long b)> links = [.. distanceMap.Take(1000).Select(t => (t.a.id, t.b.id))];

        // DFS
        var neighbours = new Dictionary<long, List<long>>();

        foreach (var (a, b) in links)
        {
            if (!neighbours.ContainsKey(a)) neighbours[a] = [];
            if (!neighbours.ContainsKey(b)) neighbours[b] = [];
            neighbours[a].Add(b);
            neighbours[b].Add(a);
        }

        List<List<long>> components = [];
        var visited = new HashSet<long>();

        foreach (var start in neighbours.Keys)
        {
            if (visited.Contains(start)) continue;

            var stack = new Stack<long>();
            stack.Push(start);
            visited.Add(start);

            var component = new List<long>();
            while (stack.Count > 0)
            {
                var curr = stack.Pop();
                component.Add(curr);
                foreach (var neighbor in neighbours[curr])
                {
                    if (visited.Add(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                }
            }
            components.Add(component);
        }
        components.Sort((a, b) => b.Count - a.Count);
        return components.Take(3).Aggregate((BigInteger)1, (acc, comp) => acc * comp.Count);
    }

    static BigInteger Part2()
    {
        List<Coord> boxes = [.. File.ReadAllLines("Day08.txt")
            .Select(line =>
            {
                long[] vals = [.. line.Split(',').Select(long.Parse)];
                return new Coord(vals[0], vals[1], vals[2]);
            })
        ];

        List<(decimal dist, Coord a, Coord b)> distanceMap = [];
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = i + 1; j < boxes.Count; j++)
            {
                decimal dist = boxes[i].DistanceTo(boxes[j]);
                distanceMap.Add((dist, boxes[i], boxes[j]));
            }
        }
        distanceMap = [.. distanceMap.OrderBy(t => t.dist)];

        var links = new Queue<(decimal dist, Coord a, Coord b)>(distanceMap);

        var components = new List<List<long>>();
        Coord lastA = new(0, 0, 0);
        Coord lastB = new(0, 0, 0);
        while ((components.Count > 0 ? components[0].Count : 0) < boxes.Count)
        {
            var (dist, a, b) = links.Dequeue();
            lastA = a;
            lastB = b;
            var a_com = components.FindIndex(c => c.Contains(a.id));
            var b_com = components.FindIndex(c => c.Contains(b.id));
            if (a_com == -1 && b_com == -1) components.Add([a.id, b.id]);
            else if (a_com != -1 && b_com == -1) components[a_com].Add(b.id);
            else if (a_com == -1 && b_com != -1) components[b_com].Add(a.id);
            else if (a_com != b_com)
            {
                components[a_com] = [.. components[a_com].Concat(components[b_com]).Distinct()];
                components.RemoveAt(b_com);
            }
        }

        return lastA.x * lastB.x;
    }

    public static void Run()
    {
        Console.WriteLine("Day 08 Solution -- Part 01:");
        Console.WriteLine(Part1());
        Console.WriteLine("Day 08 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}