using System.Numerics;

class Day03
{
    static int Part1()
    {
        string[] bankStrs = File.ReadAllLines("Day03.txt");
        int[][] banks = [.. bankStrs.Select(line => line.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())];

        int[] joltages = new int[banks.Length];
        // For each bank, find the largest number from the first n-1 digits, and then the largest number from whatever digits remain
        for (int i = 0; i < banks.Length; i++)
        {
            int[] bank = banks[i];
            int[] firstCandidates = [.. bank.Take(bank.Length - 1)];
            int first = firstCandidates.Max();
            int firstIndice = Array.IndexOf(bank, first);
            int[] secondCandidates = [.. bank.Skip(firstIndice + 1)];
            int second = secondCandidates.Max();
            joltages[i] = int.Parse($"{first}{second}");
        }

        return joltages.Sum();
    }

    static BigInteger Part2()
    {
        string[] bankStrs = File.ReadAllLines("Day03.txt");
        int[][] banks = [.. bankStrs.Select(line => line.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())];

        BigInteger[] joltages = new BigInteger[banks.Length];
        // For each bank, do the above but for 12 digits instead
        for (int i = 0; i < banks.Length; i++)
        {
            int[] bank = banks[i];
            int[] joltageDigits = new int[12];
            for (int n = 1; n <= 12; n++)
            {
                int[] candidates = [.. bank.Take(bank.Length - (12 - n))];
                int digit = candidates.Max();
                int digitIndice = Array.IndexOf(candidates, digit);
                bank = [.. bank.Skip(digitIndice + 1)];
                joltageDigits[n - 1] = digit;
            }
            joltages[i] = BigInteger.Parse(string.Concat(joltageDigits));
        }
        return joltages.Aggregate(BigInteger.Zero, (acc, x) => acc + x);
    }

    public static void Run()
    {
        Console.WriteLine("Day 03 Solution -- Part 01:");
        Console.WriteLine(Part1());

        Console.WriteLine("Day 03 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }

}