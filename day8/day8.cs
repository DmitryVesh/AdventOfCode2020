using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;


/*
    The moment the program tries to run any instruction a second time, 
    you know it will never terminate.

    The program is supposed to terminate by attempting to execute an instruction 
    immediately after the last instruction in the file
*/

public class day8
{
    public static List<string> InstructionSet = new List<string> { "acc", "jmp", "nop" };


    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day8/day8.txt");
        List<Tuple<string, int>> originalInstructions = GetInstructions(input);

        //(int accVal, bool terminated) = RunTheInstructions(originalInstructions);
        //Console.WriteLine($"Solution 1: {accVal}");

        for (int index = 0; index < originalInstructions.Count; index++)
        {
            List<Tuple<string, int>> instructions = new List<Tuple<string, int>>(originalInstructions);
            if (instructions[index].Item1 == "jmp")
            {
                instructions[index] = new Tuple<string, int>("nop", instructions[index].Item2);
            }
            else if (instructions[index].Item1 == "nop")
            {
                instructions[index] = new Tuple<string, int>("jmp", instructions[index].Item2);
            }

            (int accVal, bool terminated) = RunTheInstructions(instructions);

            if (terminated)
            {
                Console.WriteLine($"Solution 2: {accVal}");
            }
        }
    }

    private static Tuple<int, bool> RunTheInstructions(List<Tuple<string, int>> instructions)
    {
        string op;
        int arg;

        int index = 0;
        int acc = 0;

        List<int> indexesVisited = new List<int>();

        bool terminated = false;

        while (true)
        {
            if (index == instructions.Count)
            {
                terminated = true;
                break;
            }
            (op, arg) = instructions[index];

            if (IsInfiniteLoop(index, ref indexesVisited))
                break;

            CompleteInstruction(op, arg, ref index, ref acc);
        }

        return new Tuple<int, bool> (acc, terminated);
    }
    private static bool IsInfiniteLoop(int index, ref List<int> indexesVisited)
    {
        if (indexesVisited.Contains(index))
            return true;
        else
        {
            indexesVisited.Add(index);
            return false;
        }
    }
    private static void CompleteInstruction(string op, int arg, ref int index, ref int acc)
    {
        if (op == "acc")
        {
            acc += arg;
            index++;
        }
        else if (op == "jmp")
        {
            index += arg;
        }
        else if (op == "nop")
        {
            index++;
        }
    }
    private static List<Tuple<string, int>> GetInstructions(List<string> input)
    {
        List<Tuple<string, int>> instructions = new List<Tuple<string, int>>();
        Tuple<string, int> instruction;

        foreach (string line in input)
        {
            string[] instructAndNum = line.Split(' ');
            string instruct = instructAndNum[0];           
            int num = int.Parse(instructAndNum[1]);

            instruction = new Tuple<string, int>(instruct, num);
            instructions.Add(instruction);
        }
        return instructions;
    }
}