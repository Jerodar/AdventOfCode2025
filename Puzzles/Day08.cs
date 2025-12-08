using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day08
{
    public static void Run()
    {
        var inputs = InputReader.ReadAllLines("Day08");
        var coords = new List<(int x, int y, int z)>();
        foreach (var line in inputs)
        {
            var split = line.Split(',');
            coords.Add((int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
        }
        
        Console.WriteLine();
        Console.WriteLine("Day 08 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(coords);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 32103 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 08 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(coords);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 8133642976 ? $"Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<(int x, int y, int z)> inputs)
    {
        const int iterations = 1000;
        long answer = 0;

        var distances = GetAllDistances(inputs);
        distances.Sort((item1, item2) => item1.d.CompareTo(item2.d));
        
        var connected = new Dictionary<int, int>();
        var groups = new Dictionary<int, List<int>>();
        int newGroupId = 1;
        for (int i = 0; i < iterations; i++)
        {
            connected.TryGetValue(distances[i].a, out var groupIdA);
            connected.TryGetValue(distances[i].b, out var groupIdB);
            
            Debug.WriteLine($"Connecting {distances[i].a} - {distances[i].b} groups: {groupIdA} - {groupIdB}");
            if (groupIdA > 0 && groupIdB > 0 && groupIdA == groupIdB)
            {
                Debug.WriteLine($"Boxes in same group");
            }
            else if (groupIdA > 0 && groupIdB > 0)
            {
                foreach (var item in groups[groupIdB])
                {
                    connected[item] = groupIdA;
                    Debug.WriteLine($"Moving item {item} from {groupIdB} to group {groupIdA}");
                }
                groups[groupIdA].AddRange(groups[groupIdB]);
                groups.Remove(groupIdB);
            }
            else if (groupIdA > 0)
            {
                groups[groupIdA].Add(distances[i].b);
                connected.Add(distances[i].b, groupIdA);
                Debug.WriteLine($"Adding item {distances[i].b} to group {groupIdA}");
            }
            else if (groupIdB > 0)
            {
                groups[groupIdB].Add(distances[i].a);
                connected.Add(distances[i].a, groupIdB);
                Debug.WriteLine($"Adding item {distances[i].a} to group {groupIdB}");
            }
            else
            {
                connected.Add(distances[i].a, newGroupId);
                connected.Add(distances[i].b, newGroupId);
                groups[newGroupId] =
                [
                    distances[i].a,
                    distances[i].b
                ];
                Debug.WriteLine($"Created new group {newGroupId}");
                newGroupId++;
            }
        }

        var largestGroups = groups.OrderByDescending(x => x.Value.Count).Take(3).ToList();
        answer = largestGroups[0].Value.Count *  largestGroups[1].Value.Count * largestGroups[2].Value.Count;
        
        Debug.WriteLine("");

        return answer;
    }

    private static List<(int a, int b, double d)> GetAllDistances(List<(int x, int y, int z)> coords)
    {
        var distances = new List<(int a, int b, double d)>();
        for (int i = 0; i < coords.Count; i++)
        {
            for (int j = i + 1; j < coords.Count; j++)
            {
                distances.Add((i,j,GetDistance(coords[i], coords[j])));
            }
        }
        return distances;
    }

    private static double GetDistance((int x, int y, int z) a, (int x, int y, int z) b)
    {
        long deltaX = b.x- a.x;
        long deltaY = b.y - a.y;
        long deltaZ = b.z - a.z;

        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    }

    private static long RunPartTwo(List<(int x, int y, int z)> inputs)
    {
        long answer = 0;

        var distances = GetAllDistances(inputs);
        distances.Sort((item1, item2) => item1.d.CompareTo(item2.d));
        
        var connected = new Dictionary<int, int>();
        var groups = new Dictionary<int, List<int>>();
        int newGroupId = 1;
        for (int i = 0; i < distances.Count; i++)
        {
            connected.TryGetValue(distances[i].a, out var groupIdA);
            connected.TryGetValue(distances[i].b, out var groupIdB);
            
            Debug.WriteLine($"Connecting {distances[i].a} - {distances[i].b} groups: {groupIdA} - {groupIdB}");
            long groupSize;
            if (groupIdA > 0 && groupIdB > 0 && groupIdA == groupIdB)
            {
                groupSize = groups[groupIdA].Count;
                Debug.WriteLine($"Boxes in same group with size {groupSize}/{inputs.Count}");
            }
            else if (groupIdA > 0 && groupIdB > 0)
            {
                foreach (var item in groups[groupIdB])
                {
                    connected[item] = groupIdA;
                    Debug.WriteLine($"Moving item {item} from {groupIdB} to group {groupIdA}");
                }
                groups[groupIdA].AddRange(groups[groupIdB]);
                groups.Remove(groupIdB);
                groupSize = groups[groupIdA].Count;
                Debug.WriteLine($"Group {groupIdA} is now size {groupSize}/{inputs.Count}");
            }
            else if (groupIdA > 0)
            {
                groups[groupIdA].Add(distances[i].b);
                connected.Add(distances[i].b, groupIdA);
                groupSize = groups[groupIdA].Count;
                Debug.WriteLine($"Adding item {distances[i].b} to group {groupIdA} with size {groupSize}/{inputs.Count}");
            }
            else if (groupIdB > 0)
            {
                groups[groupIdB].Add(distances[i].a);
                connected.Add(distances[i].a, groupIdB);
                groupSize = groups[groupIdB].Count;
                Debug.WriteLine($"Adding item {distances[i].a} to group {groupIdB} with size {groupSize}/{inputs.Count}");
            }
            else
            {
                connected.Add(distances[i].a, newGroupId);
                connected.Add(distances[i].b, newGroupId);
                groups[newGroupId] =
                [
                    distances[i].a,
                    distances[i].b
                ];
                Debug.WriteLine($"Created new group {newGroupId}");
                groupSize = groups[newGroupId].Count;
                newGroupId++;
            }

            if (groupSize == inputs.Count)
            {
                answer = (long)inputs[distances[i].a].x * inputs[distances[i].b].x;
                break;
            }
        }
        
        Debug.WriteLine("");

        return answer;
    }
}