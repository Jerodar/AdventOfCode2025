namespace AdventOfCode2025.Puzzles;

public static class Day01
{
    public static void Run()
    {
        Console.WriteLine("Test script to test InputReader");
        Console.WriteLine();

        List<string> inputs = Utility.InputReader.ReadAllLines("Day01");

        int current = 50;
        int answer = 0;
        int index = 0;
        foreach (string input in inputs)
        {
            index++;
            int tempAnswer = 0;
            int number = int.Parse(input[1..]);

            if (input[0] == 'L')
            {
                current -= number;
                while (current < 0)
                {
                    if (current != -number)
                    { // started at zero so did not pass zero the first cycle
                        tempAnswer += 1;
                    }
                    current += 100;
                }
                if (current == 0)
                {
                    tempAnswer += 1;
                }
            }
            else
            {
                current += number;
                while (current > 99)
                {
                    current -= 100;
                    tempAnswer += 1;
                }
            }
            
            answer += tempAnswer;
            Console.WriteLine($"{index,4}: {input,-4} {current,2} +{tempAnswer}");
        }
        Console.WriteLine();
        Console.WriteLine(answer);
    }
}