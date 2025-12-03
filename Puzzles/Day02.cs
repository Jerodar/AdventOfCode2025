using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day02
{
    public static void Run()
    {
        var inputs = InputReader.ReadCsv("Day02");
        Console.WriteLine();
        Console.WriteLine("Day 02 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs[0]);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        
        Console.WriteLine();

        Console.WriteLine("Day 02 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs[0]);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;
        foreach (string input in inputs)
        {
            string[] parts = input.Split('-');
            int minDigits = parts[0].Length;
            int maxDigits = parts[1].Length;
            long first = long.Parse(parts[0]);
            long last = long.Parse(parts[1]);

            for (int i = minDigits; i <= maxDigits; i++)
            {
                if (i == 1) continue;
                if (i % 2 == 1) continue;
                
                long maxFactor = (long)Math.Pow(10.0, i);
                long halfFactor = (long)Math.Pow(10.0, i / 2);
                
                // Get starting number based on start of range
                long current = first / (maxFactor/halfFactor);
                if (current < halfFactor / 10)
                { // current would start with zero here, so increase to the first number starting with 1 (ie 080 to 100)
                    current = halfFactor / 10;
                }
                long result = current * halfFactor + current;
            
                while (current < halfFactor && result <= last && result < maxFactor)
                {
                    if (result >= first)
                    {  // number within range and not a duplicate, save it!
                        Debug.WriteLine($"Matched {result} of range {input}");
                        answer += result;
                    }

                    current++;
                    result = current * halfFactor + current;
                }
            }
        }
        Debug.WriteLine("");

        return answer;
    }
    
    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;
        foreach (string input in inputs)
        {
            string[] parts = input.Split('-');
            int minDigits = parts[0].Length;
            int maxDigits = parts[1].Length;
            long first = long.Parse(parts[0]);
            long last = long.Parse(parts[1]);
            HashSet<long> pastResults = [];

            for (int i = minDigits; i <= maxDigits; i++)
            {
                if (i == 1) continue;
                
                long maxFactor = (long)Math.Pow(10.0, i);
                long halfFactor = (long)Math.Pow(10.0, i / 2);
                long currentFactor = 10;

                while (currentFactor <= halfFactor)
                {
                    // Get starting number based on start of range
                    long current = first / (maxFactor/currentFactor);
                    if (current < currentFactor / 10)
                    { // current would start with zero here, so increase to the first number starting with 1 (ie 080 to 100)
                        current = currentFactor / 10;
                    }
                    long result = current;
                    
                    // Copy the number until the amount of digits equals i (ie from 34 to 343434)
                    while (result * currentFactor < maxFactor)
                        result = result * currentFactor + current;
                
                    while (current < currentFactor && result <= last && result < maxFactor)
                    {
                        if (result >= first && !pastResults.Contains(result))
                        {  // number within range and not a duplicate, save it!
                            Debug.WriteLine($"Matched {result} of range {input}");
                            answer += result;
                            pastResults.Add(result);
                        }

                        current++;
                        result = current;
                        while (result * 10 < maxFactor)
                            result = result * currentFactor + current;
                    }
                    
                    currentFactor *= 10;
                }
            }
        }
        Debug.WriteLine("");

        return answer;
    }
}