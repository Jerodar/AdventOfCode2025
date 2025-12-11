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
        
        var sortedNodes = TopologicalSort(nodes.Values.ToList());
        answer = CountAllPathsPassingTargets(nodes["svr"],"out", sortedNodes);

        Debug.WriteLine("");

        return answer;
    }
    
    private static long CountAllPathsPassingTargets(Node start, string goal, List<Node> topoSorted)
    {
        long paths = 0;
        Queue<Node> queue = [];
        
        var dacIndex = topoSorted.FindIndex(n => n.Id == "dac");
        var fftIndex = topoSorted.FindIndex(n => n.Id == "fft");
        
        queue.Enqueue(start);
    
        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();
            
            if (node.Id == "dac") 
                node.VisitedDac = true;
            else if (node.Id == "fft") 
                node.VisitedFft = true;
            
            // Check in the topological sorted list if the node is past any of the intermediate goals
            var currentIndex = topoSorted.FindIndex(n => n.Id == node.Id);
            if (!node.VisitedDac && currentIndex > dacIndex) continue;
            if (!node.VisitedFft && currentIndex > fftIndex) continue;

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

    private static List<Node> TopologicalSort(List<Node> nodes)
    {
        List<Node> results = [];
        
        Dictionary<string,int> inDegree = new();
        Queue<Node> queue = [];

        foreach (var node in nodes)
            foreach (var neighbour in node.Neighbours)
                if (!inDegree.TryAdd(neighbour.Id, 1))
                    inDegree[neighbour.Id]++;
        
        foreach (var node in nodes)
            if (!inDegree.ContainsKey(node.Id))
                queue.Enqueue(node);

        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();
            results.Add(node);
            foreach (var neighbour in node.Neighbours)
            {
                inDegree[neighbour.Id]--;
                if (inDegree[neighbour.Id] == 0)
                    queue.Enqueue(neighbour);
            }
        }
        
        return results;
    }
}