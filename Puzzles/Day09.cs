using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day09
{
    public static void Run()
    {
        var inputs = InputReader.ReadCsv("Day09");
        
        Console.WriteLine();
        Console.WriteLine("Day 09 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 4777824480 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 09 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == -1 ? $"Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<List<string>> inputs)
    {
        long answer = 0;
        var coords = new List<(long x, long y)>();
        foreach (var line in inputs)
        {
            coords.Add((long.Parse(line[0]), long.Parse(line[1])));
        }

        List<(int a, int b, long d)> sizes = GetAllRectangleSizes(coords);
        answer = sizes.Max(x => x.d);
        
        Debug.WriteLine("");

        return answer;
    }
    
    private static List<(int a, int b, long d)> GetAllRectangleSizes(List<(long x, long y)> coords)
    {
        var distances = new List<(int a, int b, long d)>();
        for (int i = 0; i < coords.Count; i++)
        {
            for (int j = i + 1; j < coords.Count; j++)
            {
                distances.Add((i,j,GetRectangleSize(coords[i], coords[j])));
            }
        }
        return distances;
    }

    private static long GetRectangleSize((long x, long y) a, (long x, long y) b)
    {
        long lengthX = Math.Abs(b.x - a.x) + 1;
        long lengthY = Math.Abs(b.y - a.y) + 1;

        return lengthX * lengthY;
    }

    private static long RunPartTwo(List<List<string>> inputs)
    {
        long answer = 0;

        Debug.WriteLine("");

        return answer;
    }
}