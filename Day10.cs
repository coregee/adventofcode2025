using LpSolveDotNet;

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

    static long SolveMachine1(Machine machine)
    {
        var queue = new Queue<(bool[] lights, int presses)>();
        queue.Enqueue((new bool[machine.lights.Length], 0));
        var visited = new HashSet<bool[]>();
        while (queue.Count > 0)
        {
            var (lights, presses) = queue.Dequeue();
            if (lights.SequenceEqual(machine.lights)) return presses;
            foreach (var button in machine.buttons)
            {
                var nextLights = lights.Select(l => l).ToArray();
                foreach (var i in button) nextLights[i] = !nextLights[i];
                if (!visited.Contains(nextLights))
                {
                    visited.Add(nextLights);
                    queue.Enqueue((nextLights, presses + 1));
                }
            }
        }
        return long.MaxValue; //please do not get to here :(
    }

    static long Part1()
    {
        var machines = ParseInput("Day10.txt");
        long count = 0;
        foreach (var machine in machines)
        {
            count += SolveMachine1(machine);
        }
        return count;
    }

    // all credit to reddit hints and this library
    static long SolveMachine2(Machine machine)
    {
        LpSolve.Init();
        // nJoltage constraints, nButton variables
        int nCol = machine.buttons.Count;
        int nRow = machine.joltages.Count;
        using var lp = LpSolve.make_lp(nRow, nCol);

        const double ignored = 0;
        lp.set_minim();
        lp.set_obj_fn([ignored, .. Enumerable.Repeat(1.0, nCol)]);

        lp.set_add_rowmode(true);
        for (int i = 0; i < nRow; i++)
        {
            var joltage = machine.joltages[i];
            lp.add_constraint([ignored, .. machine.buttons.Select(b => b.Contains(i) ? 1.0 : 0.0)], lpsolve_constr_types.EQ, joltage);
        }
        lp.set_add_rowmode(false);

        lp.set_verbose(lpsolve_verbosity.IMPORTANT);
        lpsolve_return solve = lp.solve();
        if (solve == lpsolve_return.OPTIMAL)
        {
            var results = new double[nCol];
            lp.get_variables(results);
            return (long)results.Sum();
        }
        Console.WriteLine("please do not ever print this line");
        return long.MaxValue;
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
        //Console.WriteLine(Part1());
        Console.WriteLine("Day 10 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}