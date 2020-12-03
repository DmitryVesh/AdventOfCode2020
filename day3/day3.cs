using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public class day3
{
    static List<string> ActualMap;
    static List<string> RepeatingMap;

    public static void Run()
    {
        RepeatingMap = GetRepeatingMap();

        //Solution1
        //int numHitTrees = TraverseMap();
        //Console.WriteLine($"Hit: {numHitTrees} trees");

        //Solution2
        List<int> sleds = new List<int>();
        sleds.Add(TraverseMap(1, 1));
        sleds.Add(TraverseMap(3, 1));
        sleds.Add(TraverseMap(5, 1));
        sleds.Add(TraverseMap(7, 1));
        sleds.Add(TraverseMap(1, 2));

        long answer = 1;
        foreach (int sled in sleds)
        {
            //Console.WriteLine(sled);
            answer = answer * sled;
        }
        Console.WriteLine($"Multiplied Hit trees of all sleds: {answer}");
    }

    private static int TraverseMap(int travelRight, int travelDown)
    {
        ActualMap = CopyMap(RepeatingMap);

        int treeHitCount = 0;
        bool reachedBottom = false;

        int currentRow = 0;
        int currentColumn = 0;

        int bottowRow = ActualMap.Count - 1;

        while (!reachedBottom)
        {
            try
            {
                //Console.WriteLine($"At currentRow: {currentRow}");
                currentRow += travelDown;
                currentColumn += travelRight;

                char[] squaresInCurrentRow = ActualMap[currentRow].ToCharArray();
                if (squaresInCurrentRow[currentColumn] == '#')
                    treeHitCount += 1;

                if (currentRow == bottowRow)
                    reachedBottom = true;
            }
            catch (Exception exception)
            {
                if (!(exception is IndexOutOfRangeException || exception is ArgumentOutOfRangeException))
                    throw exception;

                currentRow -= travelDown;
                currentColumn -= travelRight;

                AddRepeatingMapToActualMap();
                
            }
        }
        return treeHitCount;
    }
    private static void AddRepeatingMapToActualMap()
    {
        for (int count = 0; count < RepeatingMap.Count; count++)
        {
            ActualMap[count] += RepeatingMap[count];
        }
        //PrintMap();
    }
    private static void PrintMap()
    {
        Console.WriteLine("\n\n\n\n\n\n\n\n");
        foreach(string row in ActualMap)
        {
            Console.WriteLine(row);
        }
    }
    private static List<string> CopyMap(List<string> map)
    {
        return map.GetRange(0, map.Count);
    }

    private static List<string> GetRepeatingMap()
    {
        List<string> entries = new List<string>();
        StreamReader sr = new StreamReader("D:/C# projects/AdventOfCode2020/day3/day3.txt");

        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            if (line != null)
                entries.Add(line);
            else
                break;
        }
        return entries;
    }
}
