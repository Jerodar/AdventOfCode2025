using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2025.Utility;

namespace AdventOfCode2025.Puzzles;

public static class Day09
{
    public static void Run()
    {
        var inputs = InputReader.ReadCsv("Day09");
        
        Console.WriteLine();
        Console.WriteLine("Day 09 Part One");
        long answer = 0;
        TimeSpan time = Benchmark.Time(() =>
        {
            answer = RunPartOne(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 4777824480 ? $"Success!" : $"Fail!");
        
        Console.WriteLine();

        Console.WriteLine("Day 09 Part Two");
        time = Benchmark.Time(() =>
        {
            answer = RunPartTwo(inputs);
        });
        Console.WriteLine($"Answer: {answer} in {time.TotalMilliseconds} ms");
        Console.WriteLine(answer == 1542119040 ? "Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<List<string>> inputs)
    {
        long answer = 0;
        
        var coords = new List<(int x, int y)>();
        foreach (var line in inputs)
        {
            coords.Add((int.Parse(line[0]), int.Parse(line[1])));
        }
        
        answer = GetMaxRectangleSize(coords);

        return answer;
    }
    
    private static long GetMaxRectangleSize(List<(int x, int y)> coords)
    {
        long maxSize = 0; 
        for (int i = 0; i < coords.Count; i++)
        {
            for (int j = i + 1; j < coords.Count; j++)
            {
                long size = GetRectangleSize(coords[i], coords[j]);
                if (size > maxSize) maxSize = size;
            }
        }
        return maxSize;
    }

    private static long GetRectangleSize((int x, int y) a, (int x, int y) b)
    {
        long lengthX = Math.Abs(b.x - a.x) + 1;
        long lengthY = Math.Abs(b.y - a.y) + 1;

        return lengthX * lengthY;
    }

    private static long RunPartTwo(List<List<string>> inputs)
    {
        long answer = 0;
        
        var redCoords = new List<(int x, int y)>();
        var edgeCoords = new Dictionary<int, HashSet<int>>();
        
        (int x, int y) lastCoord = (0, 0);
        foreach (var line in inputs)
        {
            (int x, int y) newCoord = (int.Parse(line[0]), int.Parse(line[1]));
            redCoords.Add(newCoord);
            
            if (lastCoord != (0, 0))
                StorePointLine(newCoord, lastCoord, edgeCoords);
            
            lastCoord = newCoord;
        }
        
        // Also store the line between the last and first point
        StorePointLine(redCoords.First(), lastCoord, edgeCoords);

        answer = GetMaxRectangleSizeInsidePolygon(redCoords, edgeCoords);

        Debug.WriteLine("");

        return answer;
    }

    private static void StorePointLine((int x, int y) newCoord, (int x, int y) lastCoord, Dictionary<int, HashSet<int>> edgeCoords)
    {
        if (newCoord.x == lastCoord.x)
        {
            int startY = lastCoord.y < newCoord.y ? lastCoord.y : newCoord.y;
            int endY = lastCoord.y > newCoord.y ? lastCoord.y : newCoord.y;
            for (int y = startY;  y <= endY; y++)
                if (edgeCoords.ContainsKey(newCoord.x))
                    edgeCoords[newCoord.x].Add(y);
                else
                    edgeCoords[newCoord.x] = [y];
        }
        else
        {
            int startX = lastCoord.x < newCoord.x ? lastCoord.x : newCoord.x;
            int endX = lastCoord.x > newCoord.x ? lastCoord.x : newCoord.x;
            for (int x = startX;  x <= endX; x++)
                if (edgeCoords.ContainsKey(x))
                    edgeCoords[x].Add(newCoord.y);
                else
                    edgeCoords[x] = [newCoord.y];
        }
    }

    private static long GetMaxRectangleSizeInsidePolygon(List<(int x, int y)> coords, Dictionary<int, HashSet<int>> edges)
    {
        long maxSize = 0; 
        for (int i = 0; i < coords.Count; i++)
        {
            Debug.WriteLine($"Checking {i+1}/{coords.Count}");
            for (int j = i + 1; j < coords.Count; j++)
            {
                long size = GetRectangleSize(coords[i], coords[j]);
                if (size <= maxSize) continue;
                
                (int x, int y) corner1 = (coords[i].x, coords[j].y);
                (int x, int y) corner2 = (coords[j].x, coords[i].y);
                    
                if (!IsEdgeInPoly(coords[i], corner1, edges)) continue;
                if (!IsEdgeInPoly(coords[i], corner2, edges)) continue;
                if (!IsEdgeInPoly(coords[j], corner1, edges)) continue;
                if (!IsEdgeInPoly(coords[j], corner2, edges)) continue;

                Debug.WriteLine($"{i},{j} - {coords[i]} - {coords[j]} - {size}");
                maxSize = size;
            }
        }
        return maxSize;
    }

    private static bool IsEdgeInPoly((int x, int y) start, (int x, int y) end,
        Dictionary<int, HashSet<int>> edges)
    {
        // start a sort of raycast from the start of the grid
        // keep track if it crosses an edge of the polygon
        // if it goes outside the polygon between the start and end of coords, the edge is not completely in the polygon
        
        bool inside = false;
        if (start.x == end.x)
        {
            if (!edges.ContainsKey(start.x))  return false;

            int startY = start.y < end.y ? start.y : end.y;
            int endY = start.y > end.y ? start.y : end.y;
            
            bool lastY = false;
            bool alongEdge = false;
            bool insideBeforeEdge = false;
            int delta = 0;
            for (int y = 0; y <= endY; y++)
            {
                if (edges[start.x].Contains(y))
                {
                    if (!alongEdge && edges[start.x].Contains(y + 1))
                    {   // edge is parallel to raycast, store if the edge originated from left or right of the ray
                        if (edges.TryGetValue(start.x - 1, out HashSet<int>? value1) && value1.Contains(y))
                            delta = 1;
                        else if (edges.TryGetValue(start.x + 1, out HashSet<int>? value2) && value2.Contains(y))
                            delta = -1;

                        alongEdge = true;
                        insideBeforeEdge = inside;
                        inside = true;
                    }
                    
                    if (!lastY && !alongEdge)
                        inside = !inside;
                    
                    lastY = true;
                }
                else
                {
                    if (lastY && alongEdge)
                    {
                        // look in previous y to see which direction the parallel edge turned to
                        if (edges.TryGetValue(start.x + delta, out HashSet<int>? value) && value.Contains(y - 1))
                            inside = !insideBeforeEdge;
                        else
                            inside = insideBeforeEdge;
                        alongEdge = false;
                        delta = 0;
                    }
                        
                    lastY = false;
                    if (y > startY && !inside)
                        return false;
                }
            }
        }
        else if (start.y == end.y)
        {
            long startX = start.x < end.x ? start.x : end.x;
            long endX = start.x > end.x ? start.x : end.x;
            
            bool lastX = false;
            bool alongEdge = false;
            bool insideBeforeEdge = false;
            int delta = 0;
            for (int x = 0; x <= endX; x++)
            {
                if (edges.TryGetValue(x, out HashSet<int>? value) && value.Contains(start.y))
                {
                    if (!alongEdge && edges.TryGetValue(x + 1, out HashSet<int>? nextValue) && nextValue.Contains(start.y))
                    {   // edge is parallel to raycast, store if the edge originated from left or right of the ray
                        if (value.Contains(start.y - 1))
                            delta = 1;
                        else if (value.Contains(start.y + 1))
                            delta = -1;

                        alongEdge = true;
                        insideBeforeEdge = inside;
                        inside = true;
                    }
                    
                    if (!lastX && !alongEdge)
                        inside = !inside;
                    
                    lastX = true;
                }
                else
                {
                    if (lastX && alongEdge)
                    {
                        // look in previous y to see if edge went back in the same direction it came from
                        // if edge turns in the other direction, flip the inside flag
                        if (edges.TryGetValue(x - 1, out HashSet<int>? nextValue) && nextValue.Contains(start.y + delta))
                            inside = !insideBeforeEdge;
                        else
                            inside = insideBeforeEdge;
                        alongEdge = false;
                        delta = 0;
                    }

                    lastX = false;
                    if (x > startX && !inside)
                        return false;
                }
            }
        }

        return inside;
    }
}