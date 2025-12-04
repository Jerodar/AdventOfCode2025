using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day01
{
    public static void Run()
    {
        var inputs = InputReader.ReadAllLines("Day01");
        
        Console.WriteLine();
        Console.WriteLine("Day 01 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 1182 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 01 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 6907 ? $"Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;
        int current = 50;
        int index = 0;
        
        foreach (string input in inputs)
        {
            index++;
            int number = int.Parse(input[1..]);

            if (input[0] == 'L')
            {
                number *= -1;
            }

            number %= 100;
            current += number;

            if (current < 0)
            {
                current += 100;
            }
            else if (current > 99)
            {
                current -= 100;
            }
            
            if (current == 0)
            {
                answer += 1;
            }
            Debug.WriteLine($"{index,4}: {input,-4} {current,2} +{answer}");
        }
        Debug.WriteLine("");

        return answer;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;
        int current = 50;
        int index = 0;
        
        foreach (string input in inputs)
        {
            index++;
            int tempAnswer = 0;
            int number = int.Parse(input[1..]);

            int loops = number / 100;
            tempAnswer += loops;
            
            int remainder = number % 100;
            if (input[0] == 'L')
            {
                current -= remainder;
                
                if (current < 0)
                {
                    if (current != -remainder)
                    { // started at zero so did not pass zero the first cycle
                        tempAnswer++;
                    }
                    current += 100;
                }
                if (current == 0)
                {
                    tempAnswer++;
                }
            }
            else
            {
                current += remainder;

                if (current > 99)
                {
                    current -= 100;
                    tempAnswer++;
                } 
            }
            
            answer += tempAnswer;
            Debug.WriteLine($"{index,4}: {input,-4} {current,2} +{tempAnswer}");
        }
        Debug.WriteLine("");

        return answer;
    }
}