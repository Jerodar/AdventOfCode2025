using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day04
{
    public static void Run()
    {
        var inputs = InputReader.ReadAllLines("Day04");
        
        Console.WriteLine();
        Console.WriteLine("Day 04 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        
        Console.WriteLine();

        Console.WriteLine("Day 04 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;
        int maxY = inputs.Count - 1;
        int maxX = inputs[0].Length - 1;
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                if (inputs[y][x] != '@')
                {
                    Debug.Write('.');
                    continue;
                }
                
                int neighbours = 0;
                if (x > 0 && inputs[y][x - 1] == '@') neighbours++;
                if (y > 0 && inputs[y - 1][x] == '@') neighbours++;
                if (x < maxX && inputs[y][x + 1] == '@') neighbours++;
                if (y < maxY && inputs[y + 1][x] == '@') neighbours++;
                if (x > 0 && y > 0 && inputs[y - 1][x - 1] == '@') neighbours++;
                if (x > 0 && y < maxY && inputs[y + 1][x - 1] == '@') neighbours++;
                if (x < maxX && y > 0 && inputs[y - 1][x + 1] == '@') neighbours++;
                if (x < maxX && y < maxY && inputs[y + 1][x + 1] == '@') neighbours++;

                if (neighbours < 4)
                {
                    Debug.Write('X');
                    answer ++;
                }
                else
                {
                    Debug.Write('@');
                }
            }
            Debug.WriteLine("");
        }
        
        Debug.WriteLine("");

        return answer;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;

        Debug.WriteLine("");

        return answer;
    }
}