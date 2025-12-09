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
        Console.Write(answer > 158460858 ? "" : $"Too low! ");
        Console.Write(answer < 2912110226 ? "" : $"Too High! ");
        Console.WriteLine(answer is > 158460858 and < 2912110226 ? "Success!" : $"Fail!");
    }
    
    private static long RunPartOne(List<List<string>> inputs)
    {
        long answer = 0;
        
        var coords = new List<(long x, long y)>();
        foreach (var line in inputs)
        {
            coords.Add((long.Parse(line[0]), long.Parse(line[1])));
        }

        List<(int a, int b, long d)> sizes = GetAllRectangleSizes(coords);
        answer = sizes.Max(x => x.d);
        
        Debug.WriteLine("");

        return answer;
    }
    
    private static List<(int a, int b, long d)> GetAllRectangleSizes(List<(long x, long y)> coords)
    {
        var distances = new List<(int a, int b, long d)>();
        for (int i = 0; i < coords.Count; i++)
        {
            for (int j = i + 1; j < coords.Count; j++)
            {
                distances.Add((i,j,GetRectangleSize(coords[i], coords[j])));
            }
        }
        return distances;
    }

    private static long GetRectangleSize((long x, long y) a, (long x, long y) b)
    {
        long lengthX = Math.Abs(b.x - a.x) + 1;
        long lengthY = Math.Abs(b.y - a.y) + 1;

        return lengthX * lengthY;
    }

    private static long RunPartTwo(List<List<string>> inputs)
    {
        long answer = 0;
        
        var redCoords = new List<(long x, long y)>();
        var redCoordsSet = new HashSet<(long x, long y)>();
        var edgeCoords = new Dictionary<long, HashSet<long>>();
        (long x, long y) lastCoord = (0, 0);
        foreach (var line in inputs)
        {
            (long x, long y) newCoord = (long.Parse(line[0]), long.Parse(line[1]));
            redCoords.Add(newCoord);
            redCoordsSet.Add(newCoord);
            
            if (lastCoord != (0, 0))
            {
                if (newCoord.x == lastCoord.x)
                {
                    if (newCoord.y > lastCoord.y)
                        for (long y = lastCoord.y;  y <= newCoord.y; y++)
                            if (edgeCoords.ContainsKey(newCoord.x))
                                edgeCoords[newCoord.x].Add(y);
                            else
                                edgeCoords[newCoord.x] = [y];
                    else
                        for (long y = newCoord.y;  y <= lastCoord.y; y++)
                            if (edgeCoords.ContainsKey(newCoord.x))
                                edgeCoords[newCoord.x].Add(y);
                            else
                                edgeCoords[newCoord.x] = [y];
                }
                else
                {
                    if (newCoord.x > lastCoord.x)
                        for (long x = lastCoord.x;  x <= newCoord.x; x++)
                            if (edgeCoords.ContainsKey(x))
                                edgeCoords[x].Add(newCoord.y);
                            else
                                edgeCoords[x] = [newCoord.y];
                    else
                        for (long x = newCoord.x;  x <= lastCoord.x; x++)
                            if (edgeCoords.ContainsKey(x))
                                edgeCoords[x].Add(newCoord.y);
                            else
                                edgeCoords[x] = [newCoord.y];
                }
            }
            
            lastCoord = newCoord;
        }
        
        answer = GetRectangleSizeInsidePolygon(redCoords, redCoordsSet, edgeCoords);

        Debug.WriteLine("");

        return answer;
    }
    
    private static long GetRectangleSizeInsidePolygon(List<(long x, long y)> coords, HashSet<(long x, long y)> coordsSet, Dictionary<long, HashSet<long>> edges)
    {
        long maxDistance = 0;
        for (int i = 0; i < coords.Count; i++)
        {
            for (int j = i + 1; j < coords.Count; j++)
            {
                (long x, long y) corner1 = (coords[i].x, coords[j].y);
                (long x, long y) corner2 = (coords[j].x, coords[i].y);
                if (!IsEdgeInPoly(coords[i], corner1, edges)) continue;
                if (!IsEdgeInPoly(coords[i], corner2, edges)) continue;
                long distance = GetRectangleSize(coords[i], coords[j]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
        }
        return maxDistance;
    }

    private static bool IsEdgeInPoly((long x, long y) start, (long x, long y) end,
        Dictionary<long, HashSet<long>> edges)
    {
        // start a sort of raycast from the start of the grid
        // keep track if it crosses an edge of the polygon
        // if it goes outside the polygon between the start and end of coords, the edge is not completely in the polygon
        
        bool inside = false;
        if (start.x == end.x)
        {
            if (!edges.ContainsKey(start.x))  return false;

            long startY = start.y < end.y ? start.y : end.y;
            long endY = start.y > end.y ? start.y : end.y;
            
            bool lastY = false;
            bool alongEdge = false;
            bool insideBeforeEdge = false;
            for (long y = 0; y <= endY; y++)
            {
                if (edges[start.x].Contains(y))
                {
                    if (edges[start.x].Contains(y + 1))
                    {
                        alongEdge = true;
                        insideBeforeEdge = inside;
                    }
                    
                    if (!lastY && !alongEdge)
                        inside = !inside;
                    
                    lastY = true;
                }
                else
                {
                    if (lastY && alongEdge)
                        inside = insideBeforeEdge;
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
            for (long x = 0; x <= endX; x++)
            {
                if (edges.TryGetValue(x, out HashSet<long>? value) && value.Contains(start.y))
                {
                    if (edges.TryGetValue(x + 1, out HashSet<long>? nextValue) && nextValue.Contains(start.y))
                    {
                        alongEdge = true;
                        insideBeforeEdge = inside;
                    }
                    
                    if (!lastX && !alongEdge)
                        inside = !inside;
                    
                    lastX = true;
                }
                else
                {
                    if (lastX && alongEdge)
                        inside = insideBeforeEdge;
                    lastX = false;
                    if (x > startX && !inside)
                        return false;
                }
            }
        }

        return inside;
    }
}