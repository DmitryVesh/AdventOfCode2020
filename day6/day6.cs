using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;

public class day6
{
    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day6/day6.txt");
        List<List<string>> groups = SeperateGroups(input);
        //List<int> answeredYesGroups = GetYesInGroupsSolution1(groups);
        List<int> answeredYesGroups = GetYesInGroupsSolution2(groups);
        int sumYesInGroups = SumInList(answeredYesGroups);
        Console.WriteLine($"\nSolution 1: {sumYesInGroups}");
    }

    private static int SumInList(List<int> answeredYesGroups)
    {
        int sum = 0;
        foreach (int yesGroup in answeredYesGroups)
        {
            sum += yesGroup;
        }
        return sum;
    }
    //Solution1
    private static List<int> GetYesInGroupsSolution1(List<List<string>> groups)
    {
        List<int> yesInGroups = new List<int>();
        foreach (List<string> group in groups)
        {
            int yesCount = 0;
            List<char> alreadyAnsweredYesTo = new List<char>();

            foreach (string person in group)
            {
                foreach (char answer in person)
                {
                    if (!alreadyAnsweredYesTo.Contains(answer))
                    {
                        yesCount++;
                        alreadyAnsweredYesTo.Add(answer);
                    }
                }
            }
            PrintAlreadyAnsweredYesTo(alreadyAnsweredYesTo, yesCount);
            yesInGroups.Add(yesCount);
        }
        return yesInGroups;
    }
    private static List<int> GetYesInGroupsSolution2(List<List<string>> groups)
    {
        List<int> yesInGroups = new List<int>();
        foreach (List<string> group in groups)
        {
            int yesCount = 0;
            List<char> answeredYesTo = new List<char>();
            int personCount = 0;
            foreach (string person in group)
            {
                foreach (char answer in person)
                {
                    answeredYesTo.Add(answer);
                }
                personCount++;
            }

            yesCount = GetYesCountThatAllPeopleHave(answeredYesTo, personCount);
            PrintAlreadyAnsweredYesTo(answeredYesTo, yesCount);
            yesInGroups.Add(yesCount);
        }
        return yesInGroups;
    }

    private static int GetYesCountThatAllPeopleHave(List<char> answeredYesTo, int personInGroup)
    {
        int allAnsweredYesTo = 0;
        Dictionary<char, int> eachCharDict = new Dictionary<char, int>();

        foreach (char yes in answeredYesTo)
        {
            if (!eachCharDict.ContainsKey(yes))
            {
                eachCharDict.Add(yes, 1);
            }
            else
            {
                eachCharDict[yes]++;
            }
        }

        foreach (var item in eachCharDict)
        {
            if (item.Value == personInGroup)
            {
                allAnsweredYesTo++;
            }
        }

        return allAnsweredYesTo;
    }

    private static void PrintAlreadyAnsweredYesTo(List<char> alreadyAnsweredYesTo, int yesCount)
    {
        foreach (char yes in alreadyAnsweredYesTo)
        {
            Console.Write($"{yes}, ");
        }
        Console.Write(yesCount);
        Console.WriteLine();
    }

    private static List<List<string>> SeperateGroups(List<string> input)
    {
        List<List<string>> groups = new List<List<string>>();
        List<string> group = new List<string>();

        int lineCount = 0;
        int finalLine = 2147;

        int linesInPerson = 0;
        string lastEntry = "";
        foreach (string person in input)
        {
            linesInPerson++;
            lineCount++;
            //God damit, this was the problem.
            //When a group is only 1 line, it doesn't get registered...
            // Turns out that doesn't occur ever, so honestly no idea what the problem is
            if (person == "" || lineCount == finalLine)
            {
                if (linesInPerson == 1 && lineCount == finalLine)
                {
                    groups.Add(new List<string>() { person });
                }
                else if (linesInPerson == 1)
                {
                    groups.Add(new List<string> { lastEntry });
                }
                else if (lineCount == finalLine)
                {
                    //Can't ****ing this was the condition not met, so the answer i got like 40 mins ago was just +1...
                    group.Add(person);
                    groups.Add(group);
                }
                else
                {
                    groups.Add(group);
                }
                group = new List<string>();
                linesInPerson = 0;
            }
            else
            {
                group.Add(person);
            }

            if (lineCount == finalLine)
                break;

            lastEntry = person;
        }
        return groups;
    }
}