using System.Text.RegularExpressions;

namespace AdventOfCode2025.Puzzles;

public static class Day02
{
    public static void Run()
    {
        Console.WriteLine("Day 2 Part One");
        Console.WriteLine();

        var inputs = Utility.InputReader.ReadCsv("Day02");
        
        long answer = 0;
        var regex = new Regex(@"^(.+)\1$");
        foreach (string input in inputs[0])
        {
            var parts = input.Split('-');
            long first = long.Parse(parts[0]);
            long last = long.Parse(parts[1]);
            for (long i = first; i <= last; i++)
            {
                if (regex.Count(i.ToString()) > 0)
                {
                    Console.WriteLine($"Matched {i} of range {input}");
                    answer += i;
                }
            }
        }
        Console.WriteLine();
        Console.WriteLine(answer);
        
        Console.WriteLine();
        
        Console.WriteLine("Day 2 Part Two");
        Console.WriteLine();

        answer = 0;
        regex = new Regex(@"^(.+)\1+$");
        foreach (string input in inputs[0])
        {
            var parts = input.Split('-');
            long first = long.Parse(parts[0]);
            long last = long.Parse(parts[1]);
            for (long i = first; i <= last; i++)
            {
                if (regex.Count(i.ToString()) > 0)
                {
                    Console.WriteLine($"Matched {i} of range {input}");
                    answer += i;
                }
            }
        }
        Console.WriteLine();
        Console.WriteLine(answer);
    }
}