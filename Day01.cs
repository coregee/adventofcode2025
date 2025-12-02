class Day01
{
    static int Part1()
    {
        string[] lines = File.ReadAllLines("Day01.txt");
        int dial = 50;
        int count = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            int polarity = char.ToLower(line[0]) == 'r' ? 1 : -1;
            int value = int.Parse(line[1..]);
            dial += polarity * value;
            dial %= 100;
            if (dial == 0) count++;
        }
        return count;
        // Correct answer for supplied input: 1195
    }

    static int Part2()
    {
        string[] lines = File.ReadAllLines("Day01.txt");
        int dial = 50;
        int count = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            int polarity = char.ToLower(line[0]) == 'r' ? 1 : -1;
            int value = int.Parse(line[1..]);

            int fullRotations = value / 100;
            count += fullRotations;

            int remainder = value % 100;
            int next = dial + polarity * remainder;
            // If dial is 0, remainder cannot loop
            if (dial != 0 && (next <= 0 || next >= 100)) count++;
            dial = (next + 100) % 100; // Ensure dial in [0,99]
        }
        return count;
        // Correct answer for supplied input: 6770
    }

    public static void Run()
    {
        Console.WriteLine("Day 01 Solution -- Part 01:");
        Console.WriteLine(Part1());
        Console.WriteLine("Day 01 Solution -- Part 02:");
        Console.WriteLine(Part2());
    }
}