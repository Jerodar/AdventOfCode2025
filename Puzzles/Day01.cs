using System.Net.Mail;

namespace AdventOfCode2025.Puzzles;

public static class Day01
{
    public static void Run()
    {
        Console.WriteLine("Day 1 Part One");
        Console.WriteLine();

        List<string> inputs = Utility.InputReader.ReadAllLines("Day01");

        int current = 50;
        int answer = 0;
        foreach (string input in inputs)
        {
            int number = int.Parse(input[1..]);

            if (input[0] == 'L')
            {
                number *= -1;
            }

            number %= 100;
            current += number;

            if (current < 0)
            {
                current += 100;
            }
            else if (current > 99)
            {
                current -= 100;
            }
            
            if (current == 0)
            {
                answer += 1;
            }
        }
        Console.WriteLine(answer);
        Console.WriteLine();
        
        Console.WriteLine("Day 1 Part Two");
        Console.WriteLine();
        
        current = 50;
        answer = 0;
        int index = 0;
        foreach (string input in inputs)
        {
            index++;
            int tempAnswer = 0;
            int number = int.Parse(input[1..]);

            int loops = number / 100;
            tempAnswer += loops;
            
            int remainder = number % 100;
            if (input[0] == 'L')
            {
                current -= remainder;
                
                if (current < 0)
                {
                    if (current != -remainder)
                    { // started at zero so did not pass zero the first cycle
                        tempAnswer++;
                    }
                    current += 100;
                }
                if (current == 0)
                {
                    tempAnswer++;
                }
            }
            else
            {
                current += remainder;

                if (current > 99)
                {
                    current -= 100;
                    tempAnswer++;
                } 
            }
            
            answer += tempAnswer;
            Console.WriteLine($"{index,4}: {input,-4} {current,2} +{tempAnswer}");
        }
        Console.WriteLine();
        Console.WriteLine(answer);
    }
}