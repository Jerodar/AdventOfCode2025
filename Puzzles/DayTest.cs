using System;
using System.Collections.Generic;

namespace AdventOfCode2025.Puzzles;

public static class DayTest
{
    public static void Run()
    {
        Console.WriteLine("Test script to test InputReader");

        string rawInput = Utility.InputReader.ReadToEnd("DayTest");
        Console.WriteLine(rawInput);
        Console.WriteLine();
        
        List<string> lineInput = Utility.InputReader.ReadAllLines("DayTest");
        foreach (string line in lineInput)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine();

        List<List<string>> csvInput = Utility.InputReader.ReadCsv("DayTest",' ');
        
        foreach (List<string> line in csvInput)
        {
            switch (line.Count)
            {
                case 2:
                    Console.WriteLine($"{line[0][..3]}: {line[1]}");
                    break;
                case 5:
                    Console.WriteLine($"{line[0]} {line[1]} {line[2]} {line[3]} {line[4]}");
                    break;
            }
        }
    }
}