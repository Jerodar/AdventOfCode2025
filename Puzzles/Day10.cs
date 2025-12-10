using System;
using System.Collections;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day10
{
    public static void Run()
    {
        var inputs = InputReader.ReadCsv("Day10",' ');
        
        Console.WriteLine();
        Console.WriteLine("Day 10 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 547 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 10 Part Two");
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

        foreach (var input in inputs)
        {
            uint target = ReadTarget(input[0]);
            List<uint> buttons = ReadButtons(input);
            var result = FindShortestSolve(target, buttons);
            Debug.WriteLine(result);
            answer += result;
        }
        
        Debug.WriteLine("");

        return answer;
    }

    private static uint ReadTarget(string s)
    {
        uint result = 0;
        for (int i = 1; i < s.Length - 1; i++)
        {
            if (s[i] == '#')  result += (uint)1<<(i-1);
        }
        return result;
    }
    
    private static List<uint> ReadButtons(List<string> elements)
    {
        List<uint> buttons = [];

        for (int i = 1; i < elements.Count - 1; i++)
        {
            var numbers = elements[i][1..^1].Split(',').Select(int.Parse).ToList();
            uint button = 0;
            foreach (var number in numbers)
                button += (uint)1 << (number);
            buttons.Add(button);
        }

        return buttons;
    }
    
    private static int FindShortestSolve(uint target, List<uint> buttons)
    {
        return PushButtons(target, buttons, 0, 0, 1);;
    }

    private static int PushButtons(uint target, List<uint> buttons, int start, uint state, int depth)
    {
        int smallest = int.MaxValue;
        for (int i = start; i < buttons.Count; i++)
        {
            uint newState = state ^ buttons[i];
            if (newState == target)
            {
                return depth;
            }

            int result = PushButtons(target, buttons, i + 1, newState, depth + 1);
            if (result < smallest) smallest = result;
        }

        return smallest;
    }

    private static long RunPartTwo(List<List<string>> inputs)
    {
        long answer = 0;

        Debug.WriteLine("");

        return answer;
    }
}