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
        Console.WriteLine(answer == 1437 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 04 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 8765 ? $"Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;
        int maxY = inputs.Count - 1;
        int maxX = inputs[0].Length - 1;
        
        bool[,] map =  CreateMap(inputs);
        
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                if (!map[y,x]) continue;
                
                int neighbours = 0;
                if (x > 0 && map[y,x - 1]) neighbours++;
                if (y > 0 && map[y - 1,x]) neighbours++;
                if (x < maxX && map[y,x + 1]) neighbours++;
                if (y < maxY && map[y + 1,x]) neighbours++;
                if (x > 0 && y > 0 && map[y - 1,x - 1]) neighbours++;
                if (x > 0 && y < maxY && map[y + 1,x - 1]) neighbours++;
                if (x < maxX && y > 0 && map[y - 1,x + 1]) neighbours++;
                if (x < maxX && y < maxY && map[y + 1,x + 1]) neighbours++;

                if (neighbours < 4)
                {
                    answer++;
                }
            }
        }

        return answer;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;
        int maxY = inputs.Count - 1;
        int maxX = inputs[0].Length - 1;
        long cleaned = 0;
        
        bool[,] map =  CreateMap(inputs);
        
        do
        {
            cleaned = CleanUpMap(map, maxX, maxY);
            answer += cleaned;
        } while (cleaned > 0);
        

        return answer;
    }

    private static bool[,] CreateMap(List<string> inputs)
    {
        bool[,] map =  new bool[inputs[0].Length, inputs.Count];
        
        for (int y = 0; y < inputs.Count; y++)
        {
            for (int x = 0; x < inputs[0].Length; x++)
            {
                if (inputs[y][x] == '@')
                    map[y, x] = true;
            }
        }
        return map;
    }

    private static int CleanUpMap(bool[,] map, int maxX, int maxY)
    {
        int papersCleaned = 0;
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                if (!map[y,x]) continue;
                
                int neighbours = 0;
                if (x > 0 && map[y,x - 1]) neighbours++;
                if (y > 0 && map[y - 1,x]) neighbours++;
                if (x < maxX && map[y,x + 1]) neighbours++;
                if (y < maxY && map[y + 1,x]) neighbours++;
                if (x > 0 && y > 0 && map[y - 1,x - 1]) neighbours++;
                if (x > 0 && y < maxY && map[y + 1,x - 1]) neighbours++;
                if (x < maxX && y > 0 && map[y - 1,x + 1]) neighbours++;
                if (x < maxX && y < maxY && map[y + 1,x + 1]) neighbours++;

                if (neighbours < 4)
                {
                    papersCleaned++;
                    map[y,x] = false;
                }
            }
        }
        return papersCleaned;
    }
}