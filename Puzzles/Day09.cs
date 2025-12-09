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
        var horizontalLines = new Dictionary<int, Dictionary<int, int>>();
        var verticalLines = new Dictionary<int, Dictionary<int, int>>();
        
        (int x, int y) lastCoord = (0, 0);
        (int x, int y) newCoord;
        foreach (var line in inputs)
        {
            newCoord = (int.Parse(line[0]), int.Parse(line[1]));
            redCoords.Add(newCoord);

            if (lastCoord != (0, 0))
            {
                if (newCoord.x == lastCoord.x)
                    StoreLine(lastCoord.y, newCoord.y, newCoord.x, verticalLines);
                else
                    StoreLine(lastCoord.x, newCoord.x, newCoord.y, horizontalLines);
            }
            
            lastCoord = newCoord;
        }
        
        // Also store the line between the last and first point
        newCoord = redCoords.First();
        if (newCoord.x == lastCoord.x)
            StoreLine(lastCoord.y, newCoord.y, newCoord.x, verticalLines);
        else
            StoreLine(lastCoord.x, newCoord.x, newCoord.y, horizontalLines);

        answer = GetMaxRectangleSizeInsidePolygon(redCoords, verticalLines, horizontalLines);

        Debug.WriteLine("");

        return answer;
    }

    private static void StoreLine(int p1, int p2, int key, Dictionary<int, Dictionary<int, int>> lineDict)
    {
        int start = p1 < p2 ? p1 : p2;
        int end = p1 > p2 ? p1 : p2;
        if (lineDict.ContainsKey(key))
            lineDict[key].TryAdd(start, end);
        else
            lineDict[key] = new Dictionary<int, int> { { start, end } };
    }

    private static long GetMaxRectangleSizeInsidePolygon(List<(int x, int y)> coords, Dictionary<int, Dictionary<int, int>> verticalLines, Dictionary<int, Dictionary<int, int>> horizontalLines)
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
                    
                if (!IsLineInPoly(coords[i].y, corner1.y, corner1.x, horizontalLines, verticalLines)) continue;
                if (!IsLineInPoly(coords[i].x, corner2.x, corner2.y, verticalLines, horizontalLines)) continue;
                if (!IsLineInPoly(coords[j].y, corner2.y, corner2.x, horizontalLines, verticalLines)) continue;
                if (!IsLineInPoly(coords[j].x, corner1.x, corner1.y, verticalLines, horizontalLines)) continue;

                Debug.WriteLine($"{i},{j} - {coords[i]} - {coords[j]} - {size}");
                maxSize = size;
            }
        }
        return maxSize;
    }

    private static bool IsLineInPoly(int p1, int p2, int key, Dictionary<int, Dictionary<int, int>> crossLineDict, Dictionary<int, Dictionary<int, int>> parallelLineDict)
    {
        bool inside = false;
        
        int start = p1 < p2 ? p1 : p2;
        int end = p1 > p2 ? p1 : p2;
        
        if (!parallelLineDict.TryGetValue(key, out var parallelLines)) parallelLines = [];
 
        for (int i = 0; i < end; i++)
        {
            // Check for any crossing lines at this point on the line
            if (crossLineDict.TryGetValue(i, out Dictionary<int, int>? crossLine) && crossLine.Any(pair => pair.Key <= key && pair.Value >= key))
            {
                if (parallelLines.TryGetValue(i, out var lineEnd))
                {
                    // found the start of a parallel line, check the crosslines connecting to it:
                    //   __  this shape results in the inside flag flipping     __  this shape results keeps inside flag the same
                    //   I   after leaving the parallel line                    I   after leaving the parallel line
                    // __I                                                      I_  
                    // since the left end of the line is the Key in the dict, check if only one has the key equal to the start of the parallel line
   
                    if (crossLineDict[i].ContainsKey(key) != crossLineDict[lineEnd].ContainsKey(key))
                        inside = !inside;
 
                    i = lineEnd + 1;
                }
                else
                {
                    // Crossing an intersecting line in the middle of it always flip inside flag
                    inside = !inside;
                    i++;
                }
            }
            
            // if moving outside of polygon at any point after the checked line starts return false
            if (i > start && !inside) 
                return false;
        }

        return inside;
    }
}