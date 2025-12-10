using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.Z3;
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
        Console.WriteLine(answer == 21111 ? $"Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<List<string>> inputs)
    {
        long answer = 0;

        foreach (var input in inputs)
        {
            uint target = ReadLightsTarget(input[0]);
            List<uint> buttons = ReadLightsButtons(input);
            var result = FindShortestLightsBFS(target, buttons);
            Debug.WriteLine(result);
            answer += result;
        }
        
        Debug.WriteLine("");

        return answer;
    }

    private static uint ReadLightsTarget(string s)
    {
        uint result = 0;
        for (int i = 1; i < s.Length - 1; i++)
        {
            if (s[i] == '#')  result += (uint)1<<(i-1);
        }
        return result;
    }
    
    private static List<uint> ReadLightsButtons(List<string> elements)
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
    
    private static int FindShortestLightsBFS(uint target, List<uint> buttons)
    {
        Queue<Node> queue = [];
        
        queue.Enqueue(new Node());
        
        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();

            for (int i = node.ButtonIndex; i < buttons.Count; i++)
            {
                Node next = new()
                {
                    Value = node.Value ^ buttons[i], 
                    Cost = node.Cost + 1, 
                    ButtonIndex = i + 1
                };

                if (next.Value == target) return next.Cost;
                
                queue.Enqueue(next);
            }
        }
        
        return 0;
    }
    
    
    private struct Node
    {
        public uint Value;
        public int Cost;
        public int ButtonIndex;
    }

    private static long RunPartTwo(List<List<string>> inputs)
    {
        long answer = 0;
        
        foreach (var input in inputs)
        {
            var target = ReadJoltageTarget(input[^1]);
            var buttons = ReadJoltageButtons(input);

            var result = FindShortestJoltageZ3(target, buttons);
            Debug.WriteLine(result);
            answer += result;
        }

        Debug.WriteLine("");

        return answer;
    }

    private static List<int> ReadJoltageTarget(string s)
    {
        return [.. s[1..^1].Split(',').Select(int.Parse)];
    }

    private static List<List<int>> ReadJoltageButtons(List<string> elements)
    {
        List<List<int>> buttons = [];

        for (int i = 1; i < elements.Count - 1; i++)
        {
            var numbers = elements[i][1..^1].Split(',').Select(int.Parse).ToList();
            buttons.Add(numbers);
        }

        return buttons;
    }

    private static long FindShortestJoltageZ3(List<int> target, List<List<int>> buttons)
    {
        // Context is used to get to all the helper functions and classes used to build an equation
        using var z3Context = new Context();
        //  The MkOptimize holds all the equations and runs an optimize on it in the end
        using var z3Optimizer = z3Context.MkOptimize();
        
        List<IntExpr> buttonVariables = [];
        for (int i = 0; i < buttons.Count; i++)
        {
            // Add the constants that represent the values the buttons increment
            buttonVariables.Add(z3Context.MkIntConst($"x{i}"));
            // And add a constraint that they are >=0
            z3Optimizer.Add(z3Context.MkGe(buttonVariables[i], z3Context.MkInt(0)));
        }

        // Create the equation that determines the value of each joltage display in target
        for (int i = 0; i < target.Count; i++)
        {
            // Collect all the buttons that affect this joltage display
            List<IntExpr> targetButtonVariables = [];
            for (int j = 0; j < buttons.Count; j++)
            {
                if (buttons[j].Contains(i)) targetButtonVariables.Add(buttonVariables[j]);
            }
            
            // Add the equation x1 + x2 + ... = target[i]
            if (targetButtonVariables.Count == 0) continue;
            var buttonSum = targetButtonVariables.Count == 1
                ? targetButtonVariables[0]
                : z3Context.MkAdd(targetButtonVariables);
            z3Optimizer.Add(z3Context.MkEq(buttonSum, z3Context.MkInt(target[i])));
        }
        
        // Set as objective to minimize the result of all button variables added together
        z3Optimizer.MkMinimize(z3Context.MkAdd(buttonVariables));
        // Calculate the results and creates a Model
        z3Optimizer.Check();
        
        // Get the resulting values for each button variable from the model and sum them together
        long sum = 0;
        foreach (var button in buttonVariables)
        {
            sum += ((IntNum)z3Optimizer.Model.Evaluate(button)).Int64;
        }
        
        return sum;
    }
}