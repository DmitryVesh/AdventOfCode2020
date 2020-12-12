using AdventOfCode2020;
using System;
using System.Collections.Generic;


class day10Attempt2
{
    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day10/day10.txt");
        List<Adapter> adapters = GetAdaptersSortInput(input);
        
        int[] jolts = GetAllJolts(adapters);
        Console.WriteLine($"Jolt1: {jolts[0]}, Jolt2: {jolts[1]}, Jolt3: {jolts[2]}");
        Console.WriteLine($"Solution1: {jolts[0] * jolts[2]}");

        double distinctArrangements = GetDistinctArrangements(adapters, jolts);
        Console.WriteLine($"Solution2: {distinctArrangements}");
    }

    private static double GetDistinctArrangements(List<Adapter> adapters, int[] jolts)
    {

        int[] num1sRow = new int[3];

        int current1sInRow = 0;
        foreach (Adapter adapter in adapters)
        {
            Adapter higherNeighbour = adapter.NeighbourHigher;
            if (higherNeighbour == null)
                continue;

            if (higherNeighbour.Jolts - adapter.Jolts == 1)
                current1sInRow++;
            else
            {
                int index = current1sInRow - 2;
                current1sInRow = 0;
                if (index < 0)
                    continue;
                num1sRow[index]++;
            }
        }

        double distinctArrangements = (Math.Pow(7, num1sRow[2]) * Math.Pow(4, num1sRow[1]) * Math.Pow(2, num1sRow[0]));
        
        return distinctArrangements;
    }

    private static int[] GetAllJolts(List<Adapter> adapters)
    {
        int[] jolts = new int[3];

        foreach (Adapter adapter in adapters)
        {
            int currentAdapterJolts = adapter.Jolts;
            
            Adapter neighbour = adapter.NeighbourHigher;
            if (neighbour != null)
            {
                int higherAdapterJolts = neighbour.Jolts;
                int difference = higherAdapterJolts - currentAdapterJolts;
                jolts[difference - 1]++;
            }
        }

        return jolts;
    }

    private static List<Adapter> GetAdaptersSortInput(List<string> input)
    {
        List<Adapter> adapters = new List<Adapter>();
        Adapter current;
        Adapter previous = null;

        List<int> inputInts = new List<int>();

        foreach (string line in input)
            inputInts.Add(int.Parse(line));

        inputInts.Sort();

        //Charging outlet
        inputInts.Insert(0, 0);

        //device Joltage adapter
        int maxJolts = inputInts[inputInts.Count - 1] + 3;
        inputInts.Add(maxJolts);

        foreach (int adapterInt in inputInts)
        {
            current = new Adapter(adapterInt);
            current.NeighbourLower = previous;
            adapters.Add(current);

            if (previous != null)
                previous.NeighbourHigher = current;
            previous = current;
        }

        return adapters;
    }
}

class Adapter
{
    public int Jolts { get; private set; }
    public Adapter NeighbourLower { get; set; }
    public Adapter NeighbourHigher { get; set; }


    public Adapter(int jolts)
    {
        Jolts = jolts;
    }
}

