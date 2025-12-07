using System.Drawing;
using System.Numerics;

class Day07
{
    static BigInteger Part1()
    {
        char[][] grid = [.. File.ReadAllLines("Day07-test.txt").Select(line => line.ToCharArray())];
        int[] beams = [grid[0].IndexOf('S')];

        BigInteger count = 0;
        for (int i = 0; i < grid.Length; i++)
        {
            var newBeams = new List<int>();
            foreach (int j in beams)
            {
                if (grid[i][j] == '^')
                {
                    newBeams.Add(j - 1);
                    newBeams.Add(j + 1);
                    count++;
                    grid[i][j + 1] = '|';
                    grid[i][j - 1] = '|';
                }
                else
                {
                    newBeams.Add(j);
                    grid[i][j] = '|';
                }
            }
            beams = [.. newBeams.Where(val => val >= 0 && val < grid[0].Length).Distinct()];
        }
        return count;
    }

    // turns out no, this cannot be done with recursion :(
    static BigInteger Part2()
    {
        char[][] grid = [.. File.ReadAllLines("Day07.txt").Select(line => line.ToCharArray())];
        List<(int j, BigInteger count)> beams = [(grid[0].IndexOf('S'), 1)];

        for (int i = 0; i < grid.Length; i++)
        {
            var newBeams = new List<(int j, BigInteger count)>();
            foreach ((int j, BigInteger count) in beams)
            {
                if (grid[i][j] == '^')
                {
                    newBeams.Add((j - 1, count));
                    newBeams.Add((j + 1, count));
                }
                else
                {
                    newBeams.Add((j, count));
                }
            }
            // Filter invalids, merge counts for overlaps
            beams = [.. newBeams
                .Where(val => val.j >= 0 && val.j < grid[0].Length)
                .GroupBy(val => val.j)
                .Select(g => (g.Key, g.Select(v => v.count).Aggregate(BigInteger.Zero, (acc, x) => acc + x)))
            ];
        }
        return beams.Select(b => b.count).Aggregate(BigInteger.Zero, (acc, x) => acc + x);
    }

    public static void Run()
    {
        Console.WriteLine("Day 07 Solution -- Part 01:");
        Console.WriteLine(Part1());
        Console.WriteLine("Day 07 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}
