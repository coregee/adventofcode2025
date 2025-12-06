using System.Numerics;

class Problem
{
    BigInteger[] numbers;
    char op;
    public Problem(BigInteger[] numbers, char op)
    {
        this.numbers = numbers;
        this.op = op;
    }
    public BigInteger Result()
    {
        if (op == '*')
        {
            return numbers.Aggregate((a, b) => a * b);
        }
        else
        {
            return numbers.Aggregate((a, b) => a + b);
        }
    }
}

class Day06
{
    static BigInteger Part1()
    {
        string[][] lines = [.. File.ReadAllLines("Day06.txt").Select(line => line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries))];
        int cols = lines[0].Length;
        int rows = lines.Length;
        List<Problem> problems = [];
        for (int i = 0; i < cols; i++)
        {
            BigInteger[] numbers = [.. lines.Select(lines => lines[i]).Take(rows - 1).Select(BigInteger.Parse)];
            char op = lines[rows - 1][i][0];
            problems.Add(new Problem([.. numbers], op));
        }
        return problems.Aggregate(BigInteger.Zero, (acc, problem) => acc + problem.Result());
    }


    // this is so ugly i'm so sorry
    static BigInteger Part2()
    {
        char[][] lines = [.. File.ReadAllLines("Day06.txt").Select(line => line.ToCharArray())];
        List<char[]> linesRotated = [];
        int cols = lines[0].Length;
        int rows = lines.Length;
        for (int i = 0; i < cols; i++)
        {
            List<char> newLine = [];
            for (int j = 0; j < rows; j++)
            {
                newLine.Add(lines[j][i]);
            }
            linesRotated.Add([.. newLine]);
        }

        List<Problem> problems = [];
        List<BigInteger> numbers = [];
        char op = ' ';
        foreach (char[] line in linesRotated)
        {
            string numStr = string.Concat(line.Take(rows - 1));
            char opPos = line[rows - 1];
            if (opPos != ' ')
            {
                op = opPos;
            }
            if (string.IsNullOrWhiteSpace(numStr))
            {
                problems.Add(new Problem([.. numbers], op));
                numbers = [];
            }
            else
            {
                numbers.Add(BigInteger.Parse(numStr));
            }
        }
        problems.Add(new Problem([.. numbers], op));

        return problems.Aggregate(BigInteger.Zero, (acc, problem) => acc + problem.Result());
    }

    public static void Run()
    {
        Console.WriteLine("Day 06 Solution -- Part 01:");
        Console.WriteLine(Part1());
        Console.WriteLine("Day 06 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}
