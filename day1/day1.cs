using System;
using System.Collections.Generic;
using System.IO;

public class day1
{
    public static void Run()
    {
        List<int> numbers = new List<int>();

        StreamReader sr = new StreamReader("D:/C# projects/AdventOfCode2020/day1/day1.txt");

        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            if (line != null)
            {
                //Console.WriteLine(line);
                int num = int.Parse(line);
                numbers.Add(num);
            }
            else
            {
                break;
            }
        }

        int result1, result2, result3;
        foreach (int num1 in numbers)
        {
            foreach (int num2 in numbers)
            {
                foreach (int num3 in numbers)
                {
                    if (num1 + num2 + num3 == 2020)
                    {
                        result1 = num1;
                        result2 = num2;
                        result3 = num3;
                        Console.WriteLine($"\nResult1: {result1}, Result2: {result2}, Result3: {result3}" +
                        $"\nAnswer: {result1 * result2 * result3}");
                    }
                }

            }
        }
    }
}
