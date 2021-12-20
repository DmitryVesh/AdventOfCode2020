using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

public class day14
{
    //Read/write to memory
    // Values and memory addresses are both 36-bit unsigned integers
    //The bitmask is always given as a string of 36 bits, 
    //written with the most significant bit (representing 2^35

    //The current bitmask is applied to values immediately before they are written to memory
    //0 or 1 overwrites the corresponding bit in the value
    //X leaves the bit in the value unchanged

    //need the sum of all values left in memory after the initialization program completes
    //Test1 sol1 ans: 165
    //Test1 sol2 ans: 208
    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day14/day14Test2.txt");
        List<Instruction> instructions = Instruction.ConvertInput(input);
        long sumVals = GetSumVals(instructions);
        Console.WriteLine($"Solution 1: {sumVals}");

        sumVals = GetSumValsVersion2(instructions);
        Console.WriteLine($"Solution 2: {sumVals}");
    }

    private static long GetSumValsVersion2(List<Instruction> instructions)
    {
        Mask currentMask = null;
        Dictionary<long, Assignment> memory = new Dictionary<long, Assignment>();

        foreach (Instruction instruction in instructions)
        {
            if (instruction.Mask != null)
            {
                currentMask = instruction.Mask;
                continue;
            }

            Assignment mem = instruction.Assignment;
            long[] memsAfterMask = Instruction.MaskAddress(currentMask, mem.Address);

            foreach (long address in memsAfterMask)
            {
                if (memory.ContainsKey(address))
                    memory[address].Value = mem.Value;
                else
                {
                    Assignment newMem = new Assignment(address, mem.Value);
                    memory.Add(address, newMem);
                }
            }
        }

        long sum = 0;
        foreach (Assignment mem in memory.Values)
            sum += mem.Value;

        return sum;
    }

    private static long GetSumVals(List<Instruction> instructions)
    {
        Mask currentMask = null;
        Dictionary<long, Assignment> memory = new Dictionary<long, Assignment>();

        foreach (Instruction instruction in instructions)
        {
            if (instruction.Mask != null)
            {
                currentMask = instruction.Mask;
                continue;
            }

            Assignment mem = instruction.Assignment;
            long valAfterMask = Instruction.MaskValue(currentMask, mem.Value);
            mem.Value = valAfterMask;

            long add = mem.Address;

            if (memory.ContainsKey(add))
                memory[add] = mem;
            else
                memory.Add(add, mem);
        }

        long sum = 0;
        foreach (Assignment mem in memory.Values)
            sum += mem.Value;

        return sum;
    }
}

internal class Instruction
{
    public Mask Mask { get; set; }
    public Assignment Assignment { get; set; }

    internal static List<Instruction> ConvertInput(List<string> input)
    {
        List<Instruction> instructions = new List<Instruction>();
        char[] memRemoveStart = new char[] { 'm', 'e', '[' };
        foreach (string line in input)
        {
            Instruction instruction = new Instruction();
            if (line.StartsWith("mask"))
            {
                string[] splitString = line.Split(" = ");
                instruction.Mask = new Mask(splitString[1]);
            }
            else
            {
                string[] memAndVal = line.Split(" = ");
                long val = long.Parse(memAndVal[1]);

                memAndVal[0] = memAndVal[0].TrimStart(memRemoveStart);
                memAndVal[0] = memAndVal[0].TrimEnd(']');
                long add = long.Parse(memAndVal[0]);

                instruction.Assignment = new Assignment(add, val);
            }
            instructions.Add(instruction);
        }
        return instructions;
    }

    internal static long[] MaskAddress(Mask currentMask, long address)
    {
        string binVal = Convert.ToString(address, 2);
        if (binVal.Length < 36)
        {
            string bitsToAdd = "";
            for (int count = 0; count < 36 - binVal.Length; count++)
                bitsToAdd += '0';
            binVal = binVal.Insert(0, bitsToAdd);
        }

        string bitMask = currentMask.BitMask;
        string newBinVal = "";

        List<Tuple<int, int>> indexesChanged = new List<Tuple<int, int>>();
        for (int count = 0; count < binVal.Length; count++)
        {
            char charMask = bitMask[count];
            if (charMask == '0')
            {
                newBinVal += binVal[count];
                continue;
            }
            else if (charMask == 'X')
                indexesChanged.Add(new Tuple<int, int>(count, 0));
            newBinVal += charMask;
        }

        int numXsInCurrentMask = currentMask.CountXs();
        long combinations = (long)Math.Pow(2, numXsInCurrentMask);

        //Use indexesChanged to make unique string chars
        List<List<Tuple<int, char>>> UniqueIndexes = GetUniqueIndexes(indexesChanged, combinations);

        List<string> addresses = new List<string>();
        foreach (List<Tuple<int, char>> uniqueIndex in UniqueIndexes)
        {
            string currentBinVal = newBinVal;
            foreach (Tuple<int, char> charToChangeAtIndex in uniqueIndex)
            {
                int index = charToChangeAtIndex.Item1;
                char changeTo = charToChangeAtIndex.Item2;
                currentBinVal = currentBinVal.Insert(index, changeTo.ToString());
                currentBinVal = currentBinVal.Remove(index + 1, 1);
            }
            addresses.Add(currentBinVal);
        }

        long[] adds = new long[combinations];
        for (int count = 0; count < combinations; count++)
        {
            adds[count] = long.Parse(Convert.ToString(Convert.ToInt64(addresses[count], 2), 10));
        }
        return adds;
    }

    private static List<List<Tuple<int, char>>> GetUniqueIndexes(List<Tuple<int, int>> indexesChanged, long combinations)
    {
        List<List<Tuple<int, char>>> uniqueIndexes = new List<List<Tuple<int, char>>>();
        for (int pairCount = 0; pairCount < indexesChanged.Count; pairCount++)
        {
            List<Tuple<int, char>> AddressToAdd = new List<Tuple<int, char>>();
            for (int count = 0; count < combinations; count++)
            {
                Tuple<int, int> pair = indexesChanged[pairCount];
                int index = pair.Item1;
                char bit;
                int iterationsRan = pair.Item2;
                if (iterationsRan < combinations / 2)
                    bit = '1';
                else
                    bit = '0';
                
                indexesChanged[pairCount] = new Tuple<int, int>(pair.Item1, iterationsRan + 1);
                AddressToAdd.Add(new Tuple<int, char>(index, bit));
            }
            uniqueIndexes.Add(AddressToAdd);
        }
        return uniqueIndexes;
    }

    internal static long MaskValue(Mask currentMask, long val)
    {
        string binVal = Convert.ToString(val, 2);
        if (binVal.Length < 36)
        {
            string bitsToAdd = "";
            for (int count = 0; count < 36 - binVal.Length; count++)
                bitsToAdd += '0';
            binVal = binVal.Insert(0, bitsToAdd);
        }

        string bitMask = currentMask.BitMask;
        string newBinVal = "";
        for (int count = 0; count < binVal.Length; count++)
        {
            char charMask = bitMask[count];
            if (charMask == 'X')
            {
                newBinVal += binVal[count];
                continue;
            }
            newBinVal += charMask;
        }

        long maskedVal = long.Parse(Convert.ToString(Convert.ToInt64(newBinVal, 2), 10));
        return maskedVal;
    }
}

internal class Assignment
{
    public long Address { get; set; }
    public long Value { get; set; }

    public Assignment(long address, long value)
    {
        Address = address;
        Value = value;
    }
}

internal class Mask
{
    public string BitMask { get; private set; }

    public Mask(string bitMask)
    {
        BitMask = bitMask;
    }

    internal int CountXs()
    {
        int count = 0;
        foreach (char bit in BitMask)
        {
            if (bit == 'X')
                count++;
        }
        return count;
    }
}