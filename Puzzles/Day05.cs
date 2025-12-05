using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day05
{
    public static void Run()
    {
        var inputs = InputReader.ReadAllLines("Day05");
        
        Console.WriteLine();
        Console.WriteLine("Day 05 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 698 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 05 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == -1 ? $"Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;
        var ranges = new SortedDictionary<long, long>();

        int index = 0;
        for (; inputs[index] != string.Empty; index++)
        {
            string[] parts =  inputs[index].Split('-');
            long start = long.Parse(parts[0]);
            long end = long.Parse(parts[1]);
            if (!ranges.TryAdd(start, end) && end > ranges[start])
            {
                ranges[start] = end;
            }
        }

        index++;
        for (; index < inputs.Count; index++)
        {
            long current = long.Parse(inputs[index]);
            if (IsFresh(current, ranges)) answer++;
        }

        Debug.WriteLine("");

        return answer;
    }

    private static bool IsFresh(long input, SortedDictionary<long, long> ranges)
    {
        foreach (var start in ranges.Keys)
        {
            if (start > input) continue;
            if (start <= input && input <= ranges[start])
            {
                Debug.WriteLine($"{input} is in range {start} - {ranges[start]}");
                return true;
            }
        }

        return false;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;

        Debug.WriteLine("");

        return answer;
    }
}