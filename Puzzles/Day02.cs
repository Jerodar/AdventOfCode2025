using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day02
{
    public static void Run()
    {
        var inputs = InputReader.ReadCsv("Day02");
        Console.WriteLine();
        Console.WriteLine("Day 02 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs[0]);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        
        Console.WriteLine();

        Console.WriteLine("Day 02 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs[0]);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;
        var regex = new Regex(@"^(.+)\1$");
        foreach (string input in inputs)
        {
            var parts = input.Split('-');
            long first = long.Parse(parts[0]);
            long last = long.Parse(parts[1]);
            for (long i = first; i <= last; i++)
            {
                if (regex.Count(i.ToString()) > 0)
                {
                    Debug.WriteLine($"Matched {i} of range {input}");
                    answer += i;
                }
            }
        }
        Debug.WriteLine("");

        return answer;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;
        var regex = new Regex(@"^(.+)\1+$");
        foreach (string input in inputs)
        {
            var parts = input.Split('-');
            long first = long.Parse(parts[0]);
            long last = long.Parse(parts[1]);
            for (long i = first; i <= last; i++)
            {
                if (regex.Count(i.ToString()) > 0)
                {
                    Debug.WriteLine($"Matched {i} of range {input}");
                    answer += i;
                }
            }
        }
        Debug.WriteLine("");

        return answer;
    }
}