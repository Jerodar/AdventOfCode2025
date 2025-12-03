using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day03
{
    public static void Run()
    {
        var inputs = InputReader.ReadAllLines("Day03");
        Console.WriteLine();
        Console.WriteLine("Day 3 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        
        Console.WriteLine();

        Console.WriteLine("Day 3 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;
        foreach (string input in inputs)
        {
            int firstIndex = FindFirstIndexOfMax(0, input.Length - 1, input);
            int secondIndex = FindFirstIndexOfMax(firstIndex + 1, input.Length, input);
            
            int result = (input[firstIndex] - '0')*10 + (input[secondIndex] - '0');
            Debug.WriteLine(result);
            answer += result;
        }
        Debug.WriteLine("");

        return answer;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;
        foreach (string input in inputs)
        {
            long result = 0;
            int startIndex = - 1;
            for (int digit = 12; digit > 0; digit--)
            {
                int endIndex = input.Length - digit + 1;
                startIndex = FindFirstIndexOfMax(startIndex + 1, endIndex, input);
                
                int number = input[startIndex] - '0';
                long factor = (long)Math.Pow(10.0, digit - 1.0); 
                result += number * factor;
            }

            Debug.WriteLine(result);
            answer += result;
        }
        Debug.WriteLine("");

        return answer;
    }

    private static int FindFirstIndexOfMax(int start, int end, string input)
    {
        int highestNumber = 0;
        int highestNumberIndex = 0;
        for (int i = start; i < end; i++)
        {
            int number = input[i] - '0';
            if (number <= highestNumber) continue;
            
            highestNumber = number;
            highestNumberIndex = i;
            
            if (number == 9) break;
        }

        return highestNumberIndex;
    }
}