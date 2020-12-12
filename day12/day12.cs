using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;
using System.Drawing;

public class day12
{
    /*
    Action N means to move north by the given value.
    Action S means to move south by the given value.
    Action E means to move east by the given value.
    Action W means to move west by the given value.
    Action L means to turn left the given number of degrees.
    Action R means to turn right the given number of degrees.
    Action F means to move forward by the given value in the direction the ship is currently facing.
    */

    //The ship starts by facing east

    //Only the L and R actions change the direction the ship is facing

    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day12/day12.txt");
        List<Entry> entries = SeperateEntries(input);
        int distance = GetDistanceTravelledFromEntries(entries);
        //Console.WriteLine($"Solution 1: {distance}");
        Console.WriteLine($"Solution 2: {distance}");
    }

    private static int GetDistanceTravelledFromEntries(List<Entry> entries)
    {
        char lastDir = 'E';

        foreach (Entry entry in entries)
        {
            //(int north, int east) = Entry.EvaluateSolution1(entry, ref lastDir);
            (int north, int east) = Entry.EvaluateSolution2(entry, ref lastDir);

            Ship.TravelledNorth += north;
            Ship.TravelledEast += east;
        }

        Console.WriteLine($"Travelled north: {Ship.TravelledNorth}, travelled east: {Ship.TravelledEast}");
        int distance = Math.Abs(Ship.TravelledNorth) + Math.Abs(Ship.TravelledEast);
        return distance;
    }

    private static List<Entry> SeperateEntries(List<string> input)
    {
        List<Entry> entries = new List<Entry>();
        foreach (string line in input)
        {
            char action = line[0];
            int value = int.Parse(line.Substring(1));
            entries.Add(new Entry(action, value));
        }
        return entries;
    }
}
public static class Waypoint
{
    public static int RelativeNorth { get; set; } = 1;
    public static int RelativeEast { get; set; } = 10;

    public static void Rotate(bool clockwise, int currentDir)
    {
        if (clockwise)
            (RelativeEast, RelativeNorth) = (RelativeNorth, -RelativeEast); 
        else
            (RelativeEast, RelativeNorth) = (-RelativeNorth, RelativeEast);
    }
    
}
public static class Ship
{
    public static int TravelledNorth { get; set; } = 0;
    public static int TravelledEast { get; set; } = 0;
}
public class Entry
{
    private static List<char> dirs = new List<char>() { 'N', 'E', 'S', 'W' };

    public char Action { get; set; }
    public int Value { get; set; }

    public Entry(char action, int value)
    {
        Action = action;
        Value = value;
    }

    public static (int travelledNorth, int travelledEast) EvaluateSolution2(Entry entry, ref char lastDir)
    {
        char action = entry.Action;
        int value = entry.Value;

        int north = 0;
        int east = 0;

        switch (action)
        {
            case 'N':
                Waypoint.RelativeNorth += value;
                break;
            case 'S':
                Waypoint.RelativeNorth -= value;
                break;
            case 'E':
                Waypoint.RelativeEast += value;
                break;
            case 'W':
                Waypoint.RelativeEast -= value;
                break;

            case 'L':
            case 'R':
                int direction = GetDirectionSolution2(action, value, lastDir);               
                lastDir = dirs[direction];
                break;

            case 'F':
                north = value * Waypoint.RelativeNorth;
                east = value * Waypoint.RelativeEast;
                break;
                
        }

        return (north, east);
    }
    private static int GetDirectionSolution2(char action, int value, char lastDir)
    {
        int numTurns = value / 90;

        int currentFacingDir = dirs.IndexOf(lastDir);

        if (action == 'R')
            currentFacingDir = GetDirectionAfterTurnsSolution2(numTurns, currentFacingDir, true);
        else
            currentFacingDir = GetDirectionAfterTurnsSolution2(numTurns, currentFacingDir, false);

        return currentFacingDir;
    }
    private static int GetDirectionAfterTurnsSolution2(int numTurns, int currentFacingDir, bool clockwise)
    {
        int turningDirection;
        if (clockwise)
            turningDirection = 1;
        else
            turningDirection = -1;

        for (int turn = 0; turn < numTurns; turn++)
        {
            Waypoint.Rotate(clockwise, currentFacingDir);
            if (currentFacingDir + turningDirection <= 3 && currentFacingDir + turningDirection >= 0)
            {
                currentFacingDir += turningDirection;
            }
            else if (currentFacingDir + turningDirection == 4)
            {
                currentFacingDir = 0;
            }
            else if (currentFacingDir + turningDirection == -1)
            {
                currentFacingDir = 3;
            }
        }
        return currentFacingDir;
    }


    public static (int travelledNorth, int travelledEast) EvaluateSolution1(Entry entry, ref char lastDir)
    {
        char action = entry.Action;
        int value = entry.Value;

        int north = 0;
        int east = 0;

        switch (action)
        {
            case 'N':
                north = value;
                break;
            case 'S':
                north = -value;
                break;
            case 'E':
                east = value;
                break;
            case 'W':
                east = -value;
                break;

            case 'L':
            case 'R':
                int direction = GetDirectionSolution1(action, value, lastDir);
                lastDir = dirs[direction];
                break;

            case 'F':
                switch (lastDir)
                {
                    case 'N':
                        north = value;
                        break;
                    case 'S':
                        north = -value;
                        break;
                    case 'E':
                        east = value;
                        break;
                    case 'W':
                        east = -value;
                        break;
                }
                break;
        }

        return (north, east);
    }
    private static int GetDirectionSolution1(char action, int value, char lastDir)
    {
        int numTurns = value / 90;

        int currentFacingDir = dirs.IndexOf(lastDir);

        if (action == 'R')
            currentFacingDir = GetDirectionAfterTurnsSolution1(numTurns, currentFacingDir, true);
        else
            currentFacingDir = GetDirectionAfterTurnsSolution1(numTurns, currentFacingDir, false);

        return currentFacingDir;
    }
    private static int GetDirectionAfterTurnsSolution1(int numTurns, int currentFacingDir, bool clockwise)
    {
        int turningDirection;
        if (clockwise)
            turningDirection = 1;
        else
            turningDirection = -1;

        for (int turn = 0; turn < numTurns; turn++)
        {
            if (currentFacingDir + turningDirection <= 3 && currentFacingDir + turningDirection >= 0)
                currentFacingDir += turningDirection;
            else if (currentFacingDir + turningDirection == 4)
                currentFacingDir = 0;
            else if (currentFacingDir + turningDirection == -1)
                currentFacingDir = 3;
        }
        return currentFacingDir;
    }
}