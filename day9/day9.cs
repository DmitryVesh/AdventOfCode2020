using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;

public class day9
{

    //preamble of 25 numbers
    //After that, each number you receive should be the sum of any two of the 25 preamble nums
    //The two numbers will have different values, and there might be more than one such pair
    //nums like 100 are invalid, no sum of 25 pairs can sum to 100
    //26 is valid, 1 + 25

    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day9/day9.txt");
        List<long> nums = GetNums(input);
        int numPreamble = 25;
        (List<long> preamble, List<long> validation )= GetPreambleAndValidation(nums, numPreamble);
        long firstWrongValidationNum = GetFirstWrongValidationNum(preamble, validation);
        Console.WriteLine($"Solution 1: {firstWrongValidationNum}");

        (long min, long max) = GetMinAndMaxContigSet(firstWrongValidationNum, nums);
        Console.WriteLine($"Solution 2: {min + max}");
    }

    private static (long min, long max) GetMinAndMaxContigSet(long firstWrongValidationNum, List<long> nums)
    {
        int minIndex = -1;
        int maxIndex = -1;

        for (int numNums = 0; numNums < nums.Count; numNums++)
        {
            minIndex = numNums;
            long contigSum = 0;
            bool foundContigSum = false;

            for (int numsIndex = numNums; numsIndex < nums.Count; numsIndex++)
            {
                contigSum += nums[numsIndex];
                if (contigSum == firstWrongValidationNum)
                {
                    maxIndex = numsIndex;
                    foundContigSum = true;
                    break;
                }

            }
            if (foundContigSum)
                break;
        }



        long min = long.MaxValue, max = long.MinValue;
        //Console.WriteLine();
        for (int count = minIndex; count < maxIndex + 1; count++)
        {
            long currentVal = nums[count];
            //Console.WriteLine(currentVal);
            if (currentVal > max)
            {
                max = currentVal;
            }
            if (currentVal < min)
            {
                min = currentVal;
            }
        }
        //Console.WriteLine();

        return (min, max);
    }

    private static long GetFirstWrongValidationNum(List<long> preamble, List<long> validation)
    {
        List<Tuple<long, long>> validPairsUsed = new List<Tuple<long, long>>();
        List<long> validNumsFound = new List<long>();

        foreach (long valid in validation)
        {
            //Console.WriteLine($"Valid {valid}");
            bool validHasBeenFound = false;
            foreach (long num1 in preamble)
            {
                foreach (long num2 in preamble)
                {
                    if (num1 == num2)
                    {
                        continue;
                    }
                    else if (num1 + num2 == valid && (validPairsUsed.Contains(new Tuple<long, long>(num1, num2)) || validPairsUsed.Contains(new Tuple<long, long>(num2, num1))))
                    {
                        //Console.WriteLine($"Nums: {num1}, {num2} already have been used");
                        continue;
                    }
                    else if (num1 + num2 == valid || num2 + num1 == valid)
                    {
                        //Console.WriteLine($"Valid num: {valid}, has used: {num1}, {num2}");
                        validPairsUsed.Add(new Tuple<long, long>(num1, num2));
                        validNumsFound.Add(valid);
                        validHasBeenFound = true;
                        break;
                    }
                }
                if (validHasBeenFound)
                    break;
            }
            preamble = GetNewPreamble(preamble, valid);
        }

        foreach (long valid in validation)
        {
            if (!validNumsFound.Contains(valid))
                return valid;
        }
        return -1;
    }

    private static List<long> GetNewPreamble(List<long> preamble, long valid)
    {
        List<long> newPreamble = preamble;
        newPreamble.RemoveAt(0);
        newPreamble.Add(valid);
        return newPreamble;
    }

    private static Tuple<List<long>, List<long>> GetPreambleAndValidation(List<long> nums, int numPreamble)
    {
        List<long> preamble = new List<long>();
        List<long> validation = new List<long>();
        for (int count = 0; count < numPreamble; count++)
        {
            preamble.Add(nums[count]);
        }
        for (int count = numPreamble; count < nums.Count; count++)
        {
            validation.Add(nums[count]);
        }
        return new Tuple<List<long>, List<long>> (preamble, validation);
    }

    private static List<long> GetNums(List<string> input)
    {
        List<long> nums = new List<long>();
        foreach (string line in input)
        {
            nums.Add(long.Parse(line));
        }
        return nums;
    }
}
