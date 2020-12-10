using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;

public class day10
{
    //Any given adapter can take an input 1, 2, or 3 jolts lower than its rating and still produce its rated output joltage

    //built-in joltage adapter rated for 3 jolts higher than the highest-rated
    //adapter list were 3, 9, and 6, your device's built-in adapter would be rated for 12 

    //Treat the charging outlet near your seat as having an effective joltage rating of 0
    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day10/day10Test1.txt");
        List<int> adapters = GetAdapters(input);

        (int jolt1, int jolt2, int jolt3) = GetAllJoltsSolution1(adapters);
        Console.WriteLine($"Jolt1: {jolt1}, Jolt2: {jolt2}, Jolt3: {jolt3}");
        Console.WriteLine($"Solution1: {jolt1 * jolt3}");

        int distinctArrangements = GetDistinctArrangements(adapters);
        Console.WriteLine($"Solution2: {distinctArrangements}");
    }

    private static int GetDistinctArrangements(List<int> adapters)
    {
        int distinctArrangements = 0;

        
        List<int> sortedAdapters = new List<int>(adapters);
        sortedAdapters.Sort();

        sortedAdapters.Insert(0, 0);
        sortedAdapters.Add(sortedAdapters[sortedAdapters.Count - 1] + 3);

        List<int> testingAdapter = new List<int>(sortedAdapters);
        List<int> previousAdapter;

        for (int count2 = 0; count2 < sortedAdapters.Count; count2++)
        {
            testingAdapter = new List<int>();
            for (int count1 = 0; count1 < sortedAdapters.Count - 2; count1++)
            {
                int adapter1, adapter2, adapter3, difference1And2, difference2And3;

                adapter1 = sortedAdapters[count1];
                adapter2 = sortedAdapters[count1 + 1];
                adapter3 = sortedAdapters[count1 + 2];

                /*
                if (count1 == sortedAdapters.Count - 1)
                    adapter2 = sortedAdapters[sortedAdapters.Count - 1] + 3;
                else
                    adapter2 = sortedAdapters[count1 + 1];
                */

                difference1And2 = adapter2 - adapter1;
                difference2And3 = adapter3 - adapter2;

                Console.WriteLine($"Adapter1: {adapter1}, Adapter2: {adapter2}, Adapter3: {adapter3}");

                if (difference1And2 == 1 && difference2And3 < 3)
                {
                    distinctArrangements++;
                    continue;
                }
                else if (difference1And2 == 2 && difference2And3 < 3)
                {
                    distinctArrangements++;
                    continue;
                }
                else if (difference1And2 == 3)
                {
                    distinctArrangements++;
                    continue;
                }

            }
            previousAdapter = testingAdapter;
        }
        
        //Console.WriteLine($"Jolt1: {jolt1}, Jolt2: {jolt2}, Jolt3: {jolt3}, Solution1: {jolt1 * jolt3}");
        return distinctArrangements;

    }
    private static (int jolt1, int jolt2, int jolt3) GetAllJoltsSolution1(List<int> adapters)
    {
        int jolt1 = 0, jolt2 = 0, jolt3 = 0;
        List<int> addedAdapters = new List<int>();
        List<int> sortedAdapters = new List<int>(adapters);
        sortedAdapters.Sort();

        sortedAdapters.Insert(0, 0);
        sortedAdapters.Add(sortedAdapters[sortedAdapters.Count - 1] + 3);
        
        for (int count1 = 0; count1 < sortedAdapters.Count; count1++)
        {
            for (int count2 = count1 + 1; count2 < sortedAdapters.Count; count2++)
            {
                int adapter1 = sortedAdapters[count1];
                int adapter2 = sortedAdapters[count2];
                int difference = adapter2 - adapter1;

                //Console.WriteLine($"Adapter1: {adapter1}, Adapter2: {adapter2}, Jolt: {adapter2 - adapter1}");
                if (difference == 0) 
                {
                    //Console.WriteLine("Something wrong, shouldnt be equal");
                    continue; 
                }
                else if (difference == 1)
                {
                    jolt1++;
                }
                else if (difference == 2)
                {
                    jolt2++;
                }
                else if (difference == 3)
                {
                    jolt3++;
                }
                break;
            }
        }
        return (jolt1, jolt2, jolt3);
        
    }

    private static List<int> GetAdapters(List<string> input)
    {
        List<int> adapters = new List<int>();
        foreach (string line in input)
        {
            adapters.Add(int.Parse(line));
        }
        return adapters;
    }
}