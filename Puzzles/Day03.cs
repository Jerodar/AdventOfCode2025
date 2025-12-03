using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2025.Puzzles;

public static class Day03
{
    public static void Run()
    {
        Console.WriteLine("Day 3 Part One");
        Console.WriteLine();

        var inputs = Utility.InputReader.ReadAllLines("Day03");

        long answer = 0;
        foreach (string input in inputs)
        {
            int firstIndex = FindMax(0, input.Length - 1, input);
            int secondIndex = FindMax(firstIndex + 1, input.Length, input);
            
            int result = (input[firstIndex] - 48)*10 + (input[secondIndex] - 48);
            Console.WriteLine(result);
            answer += result;
        }
        Console.WriteLine();
        Console.WriteLine(answer);
        
        Console.WriteLine();
        
        Console.WriteLine("Day 3 Part Two");
        Console.WriteLine();

        answer = 0;
        foreach (string input in inputs)
        {
            long result = 0;
            int startIndex = - 1;
            for (int digit = 12; digit > 0; digit--)
            {
                int endIndex = input.Length - digit + 1;
                startIndex = FindMax(startIndex + 1, endIndex, input);
                result += (long)((input[startIndex] - 48) * Math.Pow(10.0,digit - 1.0));
            }

            Console.WriteLine(result);
            answer += result;
        }
        Console.WriteLine();
        Console.WriteLine(answer);
    }

    private static int FindMax(int start, int end, string input)
    {
        int[] firstIndex = Enumerable.Repeat(-1, 10).ToArray();
        for (int i = start; i < end; i++)
        {
            if (firstIndex[input[i] - 48] < 0)
            {
                firstIndex[input[i] - 48] = i;
                if (input[i] - 48 == 9)
                {
                    break;
                }
            }
        }

        int firstMax = 9;
        for (; firstMax >= 0; firstMax--)
        {
            if (firstIndex[firstMax] >= 0)
            {
                break;
            }
        }

        return firstIndex[firstMax];
    }
}