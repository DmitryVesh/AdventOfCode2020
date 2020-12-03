using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class day2
{
    public static void Run()
    {
        List<string> entries = new List<string>();

        StreamReader sr = new StreamReader("D:/C# projects/AdventOfCode2020/day2/day2.txt");

        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            if (line != null)
                entries.Add(line);
            else
                break;
        }

        int validCount = 0;
        foreach (string entrie in entries)
        {
            //Console.WriteLine();
            string[] splitOnce = entrie.Split(" ");

            string[] minMax = splitOnce[0].Split("-");
            int min = int.Parse(minMax[0]);
            int max = int.Parse(minMax[1]);
            //Console.WriteLine($"{min} {max}");
            string letter = splitOnce[1].Substring(0, 1);
            //Console.WriteLine(letter);
            string password = splitOnce[2];
            //Console.WriteLine(password);
            int count = 0;
            count = password.Count(x => x == char.Parse(letter));

            //Console.WriteLine(count);

            //Solution1
            //if (min <= count && count <= max)
            //validCount++;

            //Solution2
            if (password[min - 1].ToString() == letter && !(password[max - 1].ToString() == letter))
            {
                validCount++;
            }


            if (!(password[min - 1].ToString() == letter) && (password[max - 1].ToString() == letter))
            {
                validCount++;
            }

        }
        Console.WriteLine(validCount);

    }
}
