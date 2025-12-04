using System;
using System.Diagnostics;

namespace AdventOfCode2025.Utility;

public static class Benchmark
{
    public static TimeSpan Time(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
    
    public static TimeSpan RepeatTime(Action action, int iterations)
    {
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            action();
        stopwatch.Stop();
        return stopwatch.Elapsed / iterations;
    }
}