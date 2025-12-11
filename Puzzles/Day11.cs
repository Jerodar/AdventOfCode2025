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
        Console.WriteLine(answer == 349322478796032 ? $"Success!" : $"Fail!");
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
        int dacIndex = sortedNodes.IndexOf("dac");
        var fftIndex = sortedNodes.IndexOf("fft");
        long paths;
        if (fftIndex < dacIndex)
        {
            paths = CountAllPathsPassingTargets(nodes["svr"], "fft", sortedNodes, fftIndex);
            answer = paths;
            Console.WriteLine($"Reached fft with {paths} different paths. Total paths {answer}");
            paths = CountAllPathsPassingTargets(nodes["fft"], "dac", sortedNodes, dacIndex);
            answer *= paths;
            Console.WriteLine($"Reached dac with {paths} different paths. Total paths {answer}");
            paths = CountAllPathsPassingTargets(nodes["dac"], "out", sortedNodes, 0);
            answer *= paths;
            Console.WriteLine($"Reach out with {paths} different paths. Total paths {answer}");
        }
        else
        {
            paths = CountAllPathsPassingTargets(nodes["svr"], "dac", sortedNodes, fftIndex);
            answer = paths;
            Console.WriteLine($"Reached dac with {paths} different paths. Total paths {answer}");
            paths = CountAllPathsPassingTargets(nodes["dac"], "fft", sortedNodes, dacIndex);
            answer *= paths;
            Console.WriteLine($"Reached fft with {paths} different paths. Total paths {answer}");
            paths = CountAllPathsPassingTargets(nodes["fft"], "out", sortedNodes, 0);
            answer *= paths;
            Console.WriteLine($"Reached out with {paths} different paths. Total paths {answer}");
        }

        Debug.WriteLine("");

        return answer;
    }
    
    private static long CountAllPathsPassingTargets(Node start, string goal, List<string> topoSorted, int limitIndex)
    {
        long paths = 0;
        Queue<Node> queue = [];
        
        queue.Enqueue(start);
    
        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();

            if (node.Id == goal)
            {
                paths++;
                Debug.WriteLine($"Reached goal, total {paths} paths found");
                continue;
            }

            if (limitIndex > 0)
            {
                // Check in the topological sorted list if the node is past the goal
                var currentIndex = topoSorted.IndexOf(node.Id);
                if (currentIndex > limitIndex) continue;
            }

            foreach (var neighbour in node.Neighbours)
                queue.Enqueue(neighbour);
        }

        return paths;
    }

    private static List<string> TopologicalSort(List<Node> nodes)
    {
        List<string> results = [];
        
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
            results.Add(node.Id);
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