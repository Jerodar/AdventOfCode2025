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
            int firstIndex = FindFirstIndexOfMax(0, input.Length - 1, input);
            int secondIndex = FindFirstIndexOfMax(firstIndex + 1, input.Length, input);
            
            int result = (input[firstIndex] - '0')*10 + (input[secondIndex] - '0');
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
                startIndex = FindFirstIndexOfMax(startIndex + 1, endIndex, input);
                
                int number = input[startIndex] - '0';
                long factor = (long)Math.Pow(10.0, digit - 1.0); 
                result += number * factor;
            }

            Console.WriteLine(result);
            answer += result;
        }
        Console.WriteLine();
        Console.WriteLine(answer);
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