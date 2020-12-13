using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;

public class day13
{
    /// <summary>
    ///  Each bus has an ID number that also indicates how often the bus leaves for the airport.
    ///  
    /// Bus schedules are defined based on a timestamp that measures the number of minutes since some fixed reference point in the past
    /// At timestamp 0, every bus simultaneously departed from the sea port
    /// 
    /// After that, each bus travels to the airport, then various other locations, and finally returns to the sea port to repeat its journey forever
    /// 
    /// the bus with ID 5 departs from the sea port at timestamps 0, 5, 10, 15
    /// The bus with ID 11 departs at 0, 11, 22, 33, and so on
    /// If you are there when the bus departs, you can ride that bus to the airport
    /// 
    /// input
    ///  The first line is your estimate of the earliest timestamp you could depart on a bus
    ///  The second line lists the bus IDs that are in service
    ///  entries that show x must be out of service, so you decide to ignore them
    ///  
    /// your goal is to figure out the earliest bus you can take to the airport
    /// </summary>
    
    private static long Target = 100000000000000;
    private static int Log10Target = 14;

    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day13/day13.txt");
        int earliestTime = int.Parse(input[0]);
        List<(int, int)> buses = GetBuses(input[1]);

        //Solution 1
        //List<Tuple<int, int>> timeStampsClosestTo = GetClosestTimeStampsTo(earliestTime, buses);
        //(int smallestBus, int smallestTime) = GetSmallest(timeStampsClosestTo);
        //Console.WriteLine($"Solution 1: {smallestBus * (smallestTime - earliestTime)}");
        Log10Target = Target.ToString().Length - 1;
        long startingTimeStampConvergence = GetBusesConverging(buses);
        Console.WriteLine($"Solution 2: {startingTimeStampConvergence}");
    }

    private static long GetBusesConverging(List<(int, int)> buses)
    {
        //(b2 * x2) - (b1 * x1) = 1
        //(b5 * x5) - (b2 * x2) = 5
        //(b7 * x7) - (b5 * x5) = 2
        //(b8 * x8) - (b7 * x7) = 1
        long[] largestCounters = new long[buses.Count];
        bool running = true;
        while (running)
        {
            List<(int, long)> timeStamps = new List<(int, long)>();

            for (int count = 0; count < buses.Count - 1; count++)
            {

                (int index0, int iD0) = buses[count];
                (int index1, int iD1) = buses[count + 1];
                int indexDif = index1 - index0;


                long counter0 = largestCounters[count];
                long counter1 = largestCounters[count + 1];

                bool multiplier0 = false;
                while (true)
                {
                    if (counter1 - counter0 == indexDif)
                        break;

                    // finding x part
                    long multiplier;
                    if (counter0 < counter1)
                    {
                        if (!multiplier0)
                        {
                            multiplier = GetMultiplier(counter0, iD0);
                            if (multiplier == 0)
                            {
                                multiplier0 = true;
                                multiplier = 1;
                            }
                        }
                        else
                            multiplier = 1;

                        counter0 += iD0 * multiplier;
                    }
                        
                    else
                    {
                        if (!multiplier0)
                        {
                            multiplier = GetMultiplier(counter1, iD1);
                            if (multiplier == 0)
                            {
                                multiplier0 = true;
                                multiplier = 1;
                            }
                        }
                        else
                            multiplier = 1;

                        counter1 += iD1 * multiplier;
                    }
                }
                largestCounters[count] = counter0;
                largestCounters[count + 1] = counter1;

                timeStamps.Add((index0, counter0));
                timeStamps.Add((index1, counter1));
            }

            long largest = 0;
            int indexLargest = -1;
            for (int count = 0; count < largestCounters.Length; count++)
            {
                long num = largestCounters[count];
                if (num > largest)
                {
                    largest = num;
                    indexLargest = TimeStampFindIndex(largest, timeStamps);
                }
            }

            foreach ((int, long) pair in timeStamps)
            {
                if (pair.Item1 == indexLargest)
                    continue;
                else if (largest - pair.Item2 != indexLargest - pair.Item1)
                {
                    running = true;
                    break;
                }
                else if (largest - pair.Item2 == indexLargest - pair.Item1)
                {
                    running = false;
                }
            }
        }
        return largestCounters[0];
    }

    private static long GetMultiplier(long counter0, int iD)
    {
        long multiplier = 0;
        //int iDNumZeros = iD.ToString().Length - 1;
        for (int count = Log10Target - 2; count > -1; count--)
        {
            long next = counter0 + (long)(iD * Math.Pow(10, count));
            if (next < Target)
            {
                multiplier = (long)Math.Pow(10, count);
                break;
            }
        }
        return multiplier;
    }

    private static int TimeStampFindIndex(long largest, List<(int, long)> timeStamps)
    {
        foreach ((int, long) pair in timeStamps)
        {
            if (pair.Item2 == largest)
                return pair.Item1;
        }
        return -1;
    }

    private static (int smallestBus, int smallest) GetSmallest(List<Tuple<int, int>> timeStampsClosestTo)
    {
        int smallest = int.MaxValue;
        int smallestBus = 0;
        foreach (Tuple<int, int> pair in timeStampsClosestTo)
        {
            if (pair.Item2 < smallest)
            {
                smallest = pair.Item2;
                smallestBus = pair.Item1;
            }
        }
        return (smallestBus, smallest);
    }
    private static List<Tuple<int, int>> GetClosestTimeStampsTo(int earliestTime, List<int> buses)
    {
        List<Tuple<int, int>> earliestTimeStamps = new List<Tuple<int, int>>();
        foreach (int bus in buses)
        {
            int counter = 0;
            while (true)
            {
                counter += bus;
                if (counter >= earliestTime)
                    break;
            }
            earliestTimeStamps.Add(new Tuple<int, int>(bus, counter));
        }
        return earliestTimeStamps;
    }
    private static List<(int, int)> GetBuses(string input)
    {
        List<(int, int)> buses = new List<(int, int)>();
        string[] busesString = input.Split(',');
        for (int count = 0; count < busesString.Length; count++)
        {
            string busString = busesString[count];
            if (busString == "x")
                continue;
            buses.Add((count, int.Parse(busString)));
        }
        return buses;
    }
}

