using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day11
{
    public static void Run()
    {
        var inputs = InputReader.ReadCsv("Day11",' ');
        
        Console.WriteLine();
        Console.WriteLine("Day 11 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 585 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 11 Part Two");
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

        Dictionary<string,Node> nodes = [];

        foreach (var input in inputs)
        {
            var id = input[0][..^1];
            if (!nodes.TryGetValue(id, out var newNode))
            {
                newNode = new Node(id);
                nodes[id] = newNode;
            }

            for (int i = 1; i < input.Count; i++)
            {
                if (!nodes.TryGetValue(input[i], out var neighbour))
                {
                    neighbour = new Node(input[i]);
                    nodes[input[i]] = neighbour;
                }
                newNode.Neighbours.Add(neighbour);
            }
        }
        
        answer = CountAllPaths(nodes["you"],"out");
        
        Debug.WriteLine("");

        return answer;
    }

    private static long CountAllPaths(Node start, string goal)
    {
        long paths = 0;
        Queue<Node> queue = [];
    
        queue.Enqueue(start);
    
        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();

            foreach (var neighbour in node.Neighbours)
            {
                if (neighbour.Id == goal) 
                    paths++;
                else
                    queue.Enqueue(neighbour);
            }
        }

        return paths;
    }
    
    private struct Node(string id)
    {
        public readonly string Id = id;
        public readonly List<Node> Neighbours = [];
        public bool VisitedFft = false;
        public bool VisitedDac = false;
    }

    private static long RunPartTwo(List<List<string>> inputs)
    {
        long answer = 0;
        
        Dictionary<string,Node> nodes = [];

        foreach (var input in inputs)
        {
            var id = input[0][..^1];
            if (!nodes.TryGetValue(id, out var newNode))
            {
                newNode = new Node(id);
                nodes[id] = newNode;
            }

            for (int i = 1; i < input.Count; i++)
            {
                if (!nodes.TryGetValue(input[i], out var neighbour))
                {
                    neighbour = new Node(input[i]);
                    nodes[input[i]] = neighbour;
                }
                newNode.Neighbours.Add(neighbour);
            }
        }
        
        answer = CountAllPathsPassingTargets(nodes["svr"],"out");

        Debug.WriteLine("");

        return answer;
    }
    
    private static long CountAllPathsPassingTargets(Node start, string goal)
    {
        long paths = 0;
        Queue<Node> queue = [];
    
        queue.Enqueue(start);
    
        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();
            
            if (node.Id == "fft") 
                node.VisitedFft = true;
            else if (node.Id == "dac") 
                node.VisitedDac = true;

            foreach (var neighbour in node.Neighbours)
            {
                if (neighbour.Id == goal)
                {
                    if (node.VisitedDac && node.VisitedFft)
                        paths++;
                }
                else
                {
                    Node nextNode =  new Node(neighbour.Id);
                    nextNode.Neighbours.AddRange(neighbour.Neighbours);
                    nextNode.VisitedFft = node.VisitedFft;
                    nextNode.VisitedDac = node.VisitedDac;
                    
                    queue.Enqueue(nextNode);
                }
            }
        }

        return paths;
    }
}