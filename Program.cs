class Program
{
    static void Main(string[] args)
    {
        var days = new Dictionary<string, Action> {
            { "1", Day01.Run },
            { "2", Day02.Run },
            { "3", Day03.Run },
            { "4", Day04.Run },
            { "5", Day05.Run },
            { "6", Day06.Run }
        };

        // Run all days if no args
        if (args.Length == 0)
        {
            foreach (var day in days)
            {
                day.Value();
            }
        }
        else
        {
            // Run each arg
            foreach (var arg in args)
            {
                if (days.TryGetValue(arg, out var runDay))
                {
                    runDay();
                }
            }
        }
    }
}
