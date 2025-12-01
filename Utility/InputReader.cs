using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2025.Utility;

public static class InputReader
{
    public static string ReadToEnd(string name)
    {
        string result = string.Empty;
        try
        {
            result =  File.ReadAllText(InputDir(name));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }
    
    public static List<string> ReadAllLines(string name)
    {
        List<string> result = [];
        try
        {
            result =  File.ReadAllLines(InputDir(name)).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    public static List<List<string>> ReadCsv(string name, char  separator = ',')
    {
        List<List<string>> result = [];
        try
        {
            var lines = File.ReadAllLines(InputDir(name));
            for (var i = 0; i < lines.Length; i += 1) {
                var line = lines[i];
                result.Add(line.Split(separator).ToList());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return result;
    }

    private static string InputDir(string name)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "Inputs", name);
    }
}