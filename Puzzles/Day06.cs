using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day06
{
    public static void Run()
    {
        var inputs = InputReader.ReadAllLines("Day06");
        
        Console.WriteLine();
        Console.WriteLine("Day 06 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 6891729672676 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 06 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 9770311947567 ? $"Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;

        int operatorLineIndex = inputs.Count - 1;
        List<long> numbers = [];
        List<int> indexes = [];
        for (int i = 0; i <= operatorLineIndex; i++)
        {
            indexes.Add(0);
        }

        while (indexes[0] < inputs[0].Length)
        {
            for (int i = 0; i < operatorLineIndex; i++)
            {
                numbers.Add(0);
                while (indexes[i] < inputs[i].Length && inputs[i][indexes[i]] == ' ')
                {
                    indexes[i]++;
                }
                while (indexes[i] < inputs[i].Length && inputs[i][indexes[i]] != ' ')
                {
                    numbers[i] = numbers[i] * 10 +  inputs[i][indexes[i]] - '0';
                    indexes[i]++;
                }
            }
            
            while (indexes[operatorLineIndex] < inputs[operatorLineIndex].Length && inputs[operatorLineIndex][indexes[operatorLineIndex]] == ' ')
            {
                indexes[operatorLineIndex]++;
            }
            while (indexes[operatorLineIndex] < inputs[operatorLineIndex].Length && inputs[operatorLineIndex][indexes[operatorLineIndex]] != ' ')
            {
                answer += CalculateAnswer(numbers, inputs[operatorLineIndex][indexes[operatorLineIndex]]);
                Debug.WriteLine($"{answer}");
                numbers.Clear();
                indexes[operatorLineIndex]++;
            }
        }

        return answer;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;

        int index = inputs[4].Length;
        int numberIndex = 0;
        int operatorLineIndex = inputs.Count - 1;
        List<long> numbers = [];

        while (index > 0)
        {
            index--;
            // check for empty column for both versions of vertical alignment
            if (inputs[0][index] == ' ' && inputs[operatorLineIndex - 1][index] == ' ' ) continue;

            numbers.Add(0);
            for (int i = 0; i < operatorLineIndex; i++)
            {
                if (inputs[i][index] != ' ')
                    numbers[numberIndex] = numbers[numberIndex] * 10 + inputs[i][index] - '0';
            }
            
            numberIndex++;
            
            if (inputs[operatorLineIndex][index] == ' ') continue;

            answer += CalculateAnswer(numbers, inputs[operatorLineIndex][index]);
            Debug.WriteLine($"{answer}");
            
            numbers.Clear();
            numberIndex = 0;
        }

        return answer;
    }

    private static long CalculateAnswer(List<long> numbers, char oper)
    {
        long answer = 0;
        foreach (long number in numbers)
        {
            if (answer == 0) 
            {  
                answer = number;
                Debug.Write($"{number}");
            }
            else if(oper == '+') 
            {
                answer += number;
                Debug.Write($" + {number}");
            }
            else
            {
                answer *= number;
                Debug.Write($" * {number}");
            } 
        }
        Debug.WriteLine($" = {answer}");
        return answer;
    }
}