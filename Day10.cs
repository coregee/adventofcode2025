
class Machine(bool[] lights, List<int[]> buttons, List<int> joltages)
{
    public bool[] lights = lights;
    public List<int[]> buttons = buttons;
    public List<int> joltages = joltages;
}

class Day10
{
    static List<Machine> ParseInput(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var machines = new List<Machine>();
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var lights = parts[0].Replace("[", "").Replace("]", "").ToCharArray().Select(c => c == '#').ToArray();
            var joltages = parts[^1].Replace("{", "").Replace("}", "").Split(",").Select(int.Parse).ToList();
            var buttons = new List<int[]>();
            for (int i = 1; i < parts.Length - 1; i++)
            {
                var button = parts[i].Replace("(", "").Replace(")", "").Split(",").Select(int.Parse).ToArray();
                buttons.Add(button);
            }
            machines.Add(new Machine(lights, buttons, joltages));
        }
        return machines;
    }

    static long SolveMachine2(Machine machine)
    {
        var queue = new Queue<(int[] joltages, int presses)>();
        queue.Enqueue((new int[machine.joltages.Count], 0));
        var visited = new HashSet<string>();
        while (queue.Count > 0)
        {
            var (joltages, presses) = queue.Dequeue();
            if (joltages.SequenceEqual(machine.joltages)) return presses;
            foreach (var button in machine.buttons)
            {
                var nextJoltages = joltages.Select(j => j).ToArray();
                foreach (var jIdx in button) nextJoltages[jIdx]++;
                bool invalid = nextJoltages.Where((j, i) => j > machine.joltages[i]).Any();
                if (!invalid)
                {
                    // I HATE THIS LANGUAGE SO MUCH
                    string hash = string.Join(",", nextJoltages);
                    if (!visited.Contains(hash))
                    {
                        visited.Add(hash);
                        queue.Enqueue((nextJoltages, presses + 1));
                    }
                }
            }
        }
        return long.MaxValue; //please do not get to here :(
    }


    static long Part2()
    {
        var machines = ParseInput("Day10.txt");
        long count = 0;
        foreach (var machine in machines)
        {
            count += SolveMachine2(machine);
        }
        return count;
    }

    public static void Run()
    {
        Console.WriteLine("Day 10 Solution -- Part 01:");
        // Console.WriteLine(Part1());
        Console.WriteLine("Day 10 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}