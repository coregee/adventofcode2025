using System.Numerics;
using System.Text.RegularExpressions;

class Day02
{
    static BigInteger Part1()
    {
        string line = string.Concat(File.ReadAllLines("Day02.txt"));
        string[] rangeStrs = line.Split(',');

        BigInteger sum = 0;

        foreach (var rs in rangeStrs)
        {
            var parts = rs.Split('-');
            BigInteger start = BigInteger.Parse(parts[0]);
            BigInteger end = BigInteger.Parse(parts[1]);

            for (BigInteger current = start; current <= end; current++)
            {
                string s = current.ToString();
                int len = s.Length;

                if (len % 2 != 0) continue;

                string first = s[..(len / 2)];
                string second = s[(len / 2)..];

                if (first == second)
                    sum += current;
            }
        }

        return sum;
    }

    static BigInteger Part2()
    {
        Regex repeating = new Regex(@"^(\d+)\1+$");
        string line = string.Concat(File.ReadAllLines("Day02.txt"));
        string[] rangeStrs = line.Split(',');

        BigInteger sum = 0;

        foreach (var rs in rangeStrs)
        {
            var parts = rs.Split('-');
            BigInteger start = BigInteger.Parse(parts[0]);
            BigInteger end = BigInteger.Parse(parts[1]);
            for (BigInteger current = start; current <= end; current++)
            {
                string s = current.ToString();
                int len = s.Length;

                if (repeating.IsMatch(s))
                {
                    sum += current;
                }
            }
        }
        return sum;
    }

    public static void Run()
    {


        Console.WriteLine("Day 02 Solution -- Part 01:");
        Console.WriteLine(Part1());

        Console.WriteLine("Day 02 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }

}