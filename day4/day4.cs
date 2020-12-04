using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;

public class day4
{
    private static List<string> MandatoryFields = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
    private static List<string> ValidEyeColor = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
    private static List<char> HexDigits = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
    
    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day4/day4.txt");

        //Solution 1
        List<List<string>> entries = SeperateEntries(input);
        int validEntryCount = GetValidEntriesCount(entries);
        Console.WriteLine($"Valid Entries count: {validEntryCount}");

        //Solution 2
        //Just Changed the GetValidEntriesCount
    }

    private static int GetValidEntriesCount(List<List<string>> entries)
    {
        int validCount = 0;
        foreach (List<string> individual in entries)
        {
            //Console.WriteLine();
            bool[] tempValidFields = new bool[MandatoryFields.Count];
            foreach (string item in individual)
            {
                //Console.WriteLine(item);
                string[] pair = item.Split(':');
                string attributeName = pair[0];
                //Console.WriteLine(attributeName);
                for (int count = 0; count < MandatoryFields.Count; count++)
                {
                    if (attributeName == MandatoryFields[count])
                    {
                        //Solution 1
                        //tempValidFields[count] = true;
                        //break;


                        //Solution 2
                        bool validAttribute = ValidateAttribute(pair[1], attributeName);
                        //Console.WriteLine($"{attributeName} = {pair[1]} is: {validAttribute}");
                        tempValidFields[count] = validAttribute;
                        break;
                    }
                }
            }

            int attributesValidCount = 0;
            for (int count = 0; count < MandatoryFields.Count; count++)
            {
                if (tempValidFields[count])
                    attributesValidCount++;
            }
            if (attributesValidCount == MandatoryFields.Count)
                validCount++;
        }
        return validCount;
    }
    private static bool ValidateAttribute(string attribute, string field)
    {
        bool valid = false;
        int num;
        string text;
        switch (field)
        {
            case "byr":
                num = ConvertToInt(attribute);
                if (num == -1)
                    return false;
                if (1920 <= num && num <= 2002)
                    valid = true;
                break;
            case "iyr":
                num = ConvertToInt(attribute);
                if (num == -1)
                    return false;
                if (2010 <= num && num <= 2020)
                    valid = true;
                break;
            case "eyr":
                num = ConvertToInt(attribute);
                if (num == -1)
                    return false;
                if (2020 <= num && num <= 2030)
                    valid = true;
                break;

            case "hgt":
                (num, text) = GetNumFollowedByText(attribute);
                if (text == "cm" && 150 <= num && num <= 193)
                    valid = true;
                else if (text == "in" && 59 <= num && num <= 76)
                    valid = true;
                break;

            case "hcl":
                if (attribute[0] == '#' && attribute.Length == 7)
                {
                    foreach (char character in attribute.Substring(1))
                    {
                        if (!IsHexDigit(character))
                            return false;
                    }
                    valid = true;
                }
                break;

            case "ecl":
                foreach (string item in ValidEyeColor)
                {
                    if (item == attribute)
                    {
                        valid = true;
                        break;
                    }
                }
                break;

            case "pid":
                if (attribute.Length == 9 && ConvertToInt(attribute) != -1)
                    valid = true;
                break;

            case "cid":
                valid = true;
                break;
        }
        return valid;
    }
    private static bool IsHexDigit(char character)
    {
        foreach (char item in HexDigits)
        {
            if (character == item)
                return true;
        }
        return false;
    }
    private static Tuple<int, string> GetNumFollowedByText(string field)
    {
        Tuple<int, string> entry = null;
        for (int count = field.Length; count >= 0; count--)
        {
            int num = ConvertToInt(field.Substring(0, count));
            if (num == -1)
                continue;
            string text = field.Substring(count);
            //Console.WriteLine($"{num}:{text}");
            entry = new Tuple<int, string>(num, text);
            break;
        }

        if (entry == null)
            return new Tuple<int, string>(-1, "");

        return entry;
    }
    private static int ConvertToInt(string field)
    {
        try
        {
            return int.Parse(field);
        }
        catch (Exception)
        {
            return -1;
        }
    }
    private static List<List<string>> SeperateEntries(List<string> input)
    {
        List<List<string>> entries = new List<List<string>>();
        List<string> tempEntry = new List<string>();

        foreach (string item in input)
        {
            if (item != "")
            {
                string[] entry = item.Split(' ');
                foreach (string attribute in entry)
                {
                    tempEntry.Add(attribute);
                    //Console.WriteLine(attribute);
                }
            }
            else
            {
                //Console.WriteLine();
                entries.Add(tempEntry);
                tempEntry = new List<string>();
            }
        }

        return entries;
    }
}
