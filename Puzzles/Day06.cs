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

        int line0 = 0;
        int line1 = 0;
        int line2 = 0;
        int line3 = 0;
        int line4 = 0;

        while (line0 < inputs[0].Length)
        {
            long input0 = 0;
            long input1 = 0;
            long input2 = 0;
            long input3 = 0;
            
            while (line0 < inputs[0].Length && inputs[0][line0] == ' ')
            {
                line0++;
            }
            while (line0 < inputs[0].Length && inputs[0][line0] != ' ')
            {
                input0 = input0 * 10 +  inputs[0][line0] - '0';
                line0++;
            }
            
            while (line1 < inputs[1].Length && inputs[1][line1] == ' ')
            {
                line1++;
            }
            while (line1 < inputs[1].Length && inputs[1][line1] != ' ')
            {
                input1 = input1 * 10 +  inputs[1][line1] - '0';
                line1++;
            }
            
            while (line2 < inputs[2].Length && inputs[2][line2] == ' ')
            {
                line2++;
            }
            while (line2 < inputs[2].Length && inputs[2][line2] != ' ')
            {
                input2 = input2 * 10 +  inputs[2][line2] - '0';
                line2++;
            }
            
            while (line3 < inputs[3].Length && inputs[3][line3] == ' ')
            {
                line3++;
            }
            while (line3 < inputs[3].Length && inputs[3][line3] != ' ')
            {
                input3 = input3 * 10 +  inputs[3][line3] - '0';
                line3++;
            }
            
            while (line4 < inputs[4].Length && inputs[4][line4] == ' ')
            {
                line4++;
            }
            while (line4 < inputs[4].Length && inputs[4][line4] != ' ')
            {
                switch (inputs[4][line4])
                {
                    case '+':
                        Debug.WriteLine($"{input0} + {input1} + {input2} + {input3} =  {input0 + input1 + input2 + input3}");
                        answer += input0 + input1 + input2 + input3;
                        break;
                    case '*':
                        Debug.WriteLine($"{input0} * {input1} * {input2} * {input3} =  {input0 * input1 * input2 * input3}");
                        answer += input0 * input1 * input2 * input3;
                        break;
                }
                Debug.WriteLine($"{answer}");
                line4++;
            }
        }

        return answer;
    }

    private static long RunPartTwo(List<string> inputs)
    {
        long answer = 0;

        int index = inputs[4].Length - 1;

        while (index > 0)
        {
            long[] numbers = [0,0,0,0,0];
            long tempAnswer = 0;
            int numIndex = 0;
            
            while (index > 0 && inputs[4][index] == ' ')
            {
                if (inputs[0][index] != ' ')
                    numbers[numIndex] = numbers[numIndex] * 10 + inputs[0][index] - '0';
                if (inputs[1][index] != ' ')
                    numbers[numIndex] = numbers[numIndex] * 10 + inputs[1][index] - '0';
                if (inputs[2][index] != ' ')
                    numbers[numIndex] = numbers[numIndex] * 10 + inputs[2][index] - '0';
                if (inputs[3][index] != ' ')
                    numbers[numIndex] = numbers[numIndex] * 10 + inputs[3][index] - '0';
                numIndex++;
                index--;
            }
            
            if (inputs[0][index] != ' ')
                numbers[numIndex] = numbers[numIndex] * 10 + inputs[0][index] - '0';
            if (inputs[1][index] != ' ')
                numbers[numIndex] = numbers[numIndex] * 10 + inputs[1][index] - '0';
            if (inputs[2][index] != ' ')
                numbers[numIndex] = numbers[numIndex] * 10 + inputs[2][index] - '0';
            if (inputs[3][index] != ' ')
                numbers[numIndex] = numbers[numIndex] * 10 + inputs[3][index] - '0';
            
            switch (inputs[4][index])
            {
                case '+':
                    foreach (long number in numbers)
                    {
                        Debug.Write($"{number} ");
                        tempAnswer += number;
                    }
                    Debug.WriteLine($"+=  {tempAnswer}");
                    break;
                case '*':
                    foreach (long number in numbers)
                    {
                        if (number == 0) continue;
                        Debug.Write($"{number} ");
                        if (tempAnswer == 0) tempAnswer = number;
                        else tempAnswer *= number; 
                    }
                    Debug.WriteLine($"*=  {tempAnswer}");
                    break;
            }
            answer += tempAnswer;
            Debug.WriteLine($"{answer}");
            index--;
        }

        return answer;
    }
}