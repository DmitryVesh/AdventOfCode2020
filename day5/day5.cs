using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;


public class day5
{
    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day5/day5.txt");

        (List<string> Rows, List<string> Columns) = SeperateRowAndColumns(input);
        List<int> IntRows = GetIntEntries(Rows, 'F', 'B');
        List<int> IntColumns = GetIntEntries(Columns, 'L', 'R');

        //Solution 1
        int HighestID = GetHighestID(IntRows, IntColumns);
        Console.WriteLine($"Highest ID: {HighestID}");

        //Solution 2
        //Maybe get rid of all seats in the back
        //So -1 from front, add and then - 1 from back
        //Row 0 and 127 dont exist
        //not - So 8 seats per row, max is -16
        // Find the missing entries, and then minus them from your entry...
        // No dont think so Find which missing entries are below your id, if are then -1 from yours
        //      if above your id, then nothing

        (List<int> MissingEntryRows,List<int> MissingEntryColumns) = GetMissingRowsColumns(IntRows, IntColumns);
        //Console.WriteLine($"Missing num entries: {MissingEntryRows.Count}, Total num entries: {IntRows.Count}");

        //int myID = HighestID;
        for (int entryCount = 0; entryCount < MissingEntryRows.Count; entryCount++)
        {
            int id = GetID(MissingEntryRows[entryCount], MissingEntryColumns[entryCount]);
            Console.WriteLine(id);
            //if (id < HighestID || id > HighestID)
            //    myID--;
        }
        //Console.WriteLine($"My ID: {myID}");

        //Then picked the id of 633, which was the only 1 not in the front or the back...
        //I don't think the instructions were pretty clear



    }

    private static Tuple<List<int>, List<int>> GetMissingRowsColumns(List<int> intRows, List<int> intColumns)
    {
        List<int> MissingRows = new List<int>();
        List<int> MissingColumns = new List<int>();
        bool present;
        for (int rowCount = 0; rowCount < Math.Pow(2, 7); rowCount++)
        {
            for (int columnCount = 0; columnCount < Math.Pow(2, 3); columnCount++)
            {
                present = false;
                for (int entries = 0; entries < intRows.Count; entries++)
                {
                    if (intRows[entries] == rowCount && intColumns[entries] == columnCount)
                    {
                        present = true;
                        break;
                    }
                }
                if (!present)
                {
                    MissingRows.Add(rowCount);
                    MissingColumns.Add(columnCount);
                }
            }
        }
        return new Tuple<List<int>, List<int>> (MissingRows, MissingColumns);
    }

    private static int GetHighestID(List<int> intRows, List<int> intColumns)
    {
        int maxID = 0;
        int index = 0;
        for (int count = 0; count < intRows.Count; count++)
        {
            int id = GetID(intRows[count], intColumns[count]);
            if (id > maxID)
            {
                maxID = id;
                index = count;
            }
        }
        return maxID;
        //return new Tuple<int, int, int> (maxID, intRows[index], intColumns[index]);
    }
    private static int GetID(int row, int column)
    {
        return row * 8 + column;
    }

    private static List<int> GetIntEntries(List<string> entries, char lowerHalf, char upperHalf)
    {
        List<int> IntEntries = new List<int>();
        foreach (string entry in entries)
        {
            int intEntry = GetIntFromEntry(entry, lowerHalf, upperHalf);
            IntEntries.Add(intEntry);
        }
        return IntEntries;
    }
    private static int GetIntFromEntry(string entry, char lowerHalf, char upperHalf)
    {
        int lower = 0;
        int higher = (int)Math.Pow(2, entry.Length) - 1;
        int power = entry.Length - 1;
        //Console.WriteLine();
        //Console.WriteLine(entry);
        for (int count = 0; count < entry.Length; count++)
        {
            //Console.WriteLine($"Lower: {lower}, Higher: {higher}, Power: {power - count}");
            if (entry[count] == lowerHalf)
                higher = higher - (int)Math.Pow(2, power - count);
            else
                lower = lower + (int)Math.Pow(2, power - count);
        }
        //Console.WriteLine($"Final ---- Lower: {lower}, Higher: {higher}");
        return lower;
    }
    private static Tuple<List<string>, List<string>> SeperateRowAndColumns(List<string> input)
    {
        List<string> rows = new List<string>();
        List<string> columns = new List<string>();

        foreach (string entry in input)
        {
            string row = entry.Substring(0, 7);
            string column = entry.Substring(7, 3);
            rows.Add(row);
            columns.Add(column);
        }
        return new Tuple<List<string>, List<string>> (rows, columns);
    }


}