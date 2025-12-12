using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day12
{
    public static void Run()
    {
        var inputs = InputReader.ReadAllLines("Day12");
        
        Console.WriteLine();
        Console.WriteLine("Day 12 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 499 ? $"Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<string> inputs)
    {
        long answer = 0;

        List<bool[,]> blocks = [];
        List<int> blockSizes = [];
        
        for (int i = 0; i < inputs.Count; i++)
        {
            if (inputs[i][1] == ':')
            {
                blocks.Add(ParseBlock(inputs,i + 1));
                blockSizes.Add(ParseBlockSize(inputs,i + 1));
                i += 4;
            }
            else
            {
                var elements = inputs[i].Split(' ');
                (int x, int y) size = ParseSize(elements[0]);
                int[] pieces = elements[1..].Select(int.Parse).ToArray();
                
                long totalSize = 0;
                for (int j = 0; j < pieces.Length; j++)
                    totalSize += pieces[j] * blockSizes[j];

                if (totalSize > size.x * size.y) continue;

                // figure out if shapes fit together somehow
                Debug.WriteLine($"{size}  {pieces[0]} {pieces[1]} {pieces[2]} {pieces[3]} {pieces[4]} {pieces[5]}");
                answer++;
            }
        }
        
        Debug.WriteLine("");

        return answer;
    }

    private static bool[,] ParseBlock(List<string> inputs, int i)
    {
        bool [,] block = new bool[inputs[i].Length, inputs[i].Length];
        for (int x = 0; x < inputs[i].Length; x++)
            for (int y = 0; y < inputs[i].Length; y++)
                block[x, y] = inputs[y+i][x] == '#';
        return block;
    }
    
    private static int ParseBlockSize(List<string> inputs, int i)
    {
        int size = 0;
        for (int x = 0; x < inputs[i].Length; x++)
            for (int y = 0; y < inputs[i].Length; y++)
                if (inputs[y + i][x] == '#')
                    size++;
        return size;
    }
    
    private static (int x, int y) ParseSize(string s)
    {
        int cross = s.IndexOf('x');
        return (int.Parse(s[..cross]), int.Parse(s[(cross+1)..^1]));
    }
}