using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day07
{
    public static void Run()
    {
        var inputs = InputReader.ReadAllLines("Day07");
        
        Console.WriteLine();
        Console.WriteLine("Day 07 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 1633 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 07 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 34339203133559 ? $"Success!" : $"Fail!");
    }

    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;

        int maxX = inputs[0].Length;
        bool[] beams = new bool[maxX];

        for (int x = 0; x < maxX; x++)
        {
            if (inputs[0][x] != 'S') continue;
            beams[x] = true;
            break;
        }

        for (int y = 2; y < inputs.Count; y+=2)
        {
            for (int x = 0; x < maxX; x++)
            {
                if (!beams[x]) continue;
                if (inputs[y][x] != '^') continue;

                answer++;

                if (x-1 >= 0) beams[x-1] = true;
                if (x+1 < maxX) beams[x+1] = true;
                beams[x] = false;
            }
        }

        return answer;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;

        int maxX = inputs[0].Length;
        long[] timelines = new long[maxX];

        for (int x = 0; x < maxX; x++)
        {
            if (inputs[0][x] != 'S') continue;
            timelines[x] = 1;
            break;
        }

        for (int y = 2; y < inputs.Count; y+=2)
        {
            for (int x = 0; x < maxX; x++)
            {
                if (timelines[x] == 0) continue;
                if (inputs[y][x] != '^') continue;
                
                if (x-1 >= 0) timelines[x-1] += timelines[x];
                if (x+1 < maxX) timelines[x+1] += timelines[x];
                timelines[x] = 0;
            }
        }

        answer = timelines.Sum();
        return answer;
    }
}