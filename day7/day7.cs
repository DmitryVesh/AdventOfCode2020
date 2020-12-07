using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;
using System.Reflection;

public class day7
{
    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day7/day7.txt");
        List<Bag> bags = SetBags(input);
        //Bag.PrintBags(bags);
        Bag goldBag = new Bag("shiny gold");
        //int numGoldBags = Bag.GetSumNumBags(goldBag, bags);
        //Console.WriteLine($"Solution 1: {numGoldBags}");

        Console.WriteLine($"Solution 2: {Bag.GetNumBagsInsideBag(goldBag, bags)}");
    }
    private static List<Bag> SetBags(List<string> input)
    {
        List<Bag> bags = new List<Bag>();

        Dictionary<string, Bag> bagsAdded = new Dictionary<string, Bag>();

        foreach (string line in input)
        {
            string[] nameAndContains = line.Split(" contain ");
            string[] name = nameAndContains[0].Split(' ');

            string contains = nameAndContains[1];

            Dictionary<Bag, int> containBags;
            if (contains == "no other bags.") { containBags = null; }
            else
            {
                containBags = new Dictionary<Bag, int>();

                string[] manyBagsContain = contains.Split(", ");
                foreach (string bagContains in manyBagsContain)
                {
                    string[] numAndType = bagContains.Split(' ');
                    int num = int.Parse(numAndType[0]);
                    string type = string.Concat(numAndType[1], ' ', numAndType[2]);

                    Bag bagToAdd;
                    if (bagsAdded.ContainsKey(type)) 
                        bagToAdd = bagsAdded[type];
                    else
                    {
                        bagToAdd = new Bag(type);
                        bagsAdded.Add(type, bagToAdd);
                    }

                    containBags.Add(bagToAdd, num);
                }
            }


            Bag bag = new Bag(string.Concat(name[0], ' ', name[1]));
            bag.SetBagsCanHold(containBags);

            bags.Add(bag);
        }
        return bags;
    }
}
public class Bag
{
    public string Name { get; set; }
    public Dictionary<Bag, int> BagsCanHold { get; set; }
    public static Dictionary<string, Bag> BagRelationships { get; set; }
    public static string CounterAdding { get; set; } = "";

    public static void SetupBagRelationships(List<Bag> bags)
    {
        BagRelationships = new Dictionary<string, Bag>();
        foreach(Bag bag in bags)
        {
            BagRelationships.Add(bag.Name, bag);
        }
    }
    public Bag(string name)
    {
        Name = name;
    }
    public void SetBagsCanHold(Dictionary<Bag, int> bagsCanHold)
    {
        BagsCanHold = bagsCanHold;
    }
    public static void PrintBags(List<Bag> bags)
    {
        foreach(Bag bag in bags)
        {
            bag.PrintBag();
        }
    }
    //Solution 1
    public static int GetSumNumBags(Bag bag, List<Bag> bags)
    {
        int numBags = 0;
        SetupBagRelationships(bags);
        foreach(Bag bagInList in bags)
        {
            numBags += bagInList.GetBagCanHoldCount(bag);
        }
        return numBags;
    }
    //Solution 2
    public static int GetNumBagsInsideBag(Bag bag, List<Bag> bags)
    {
        int numBags = 0;
        SetupBagRelationships(bags);

        numBags = bag.TraverseAllBags() - 1;
        Console.WriteLine($"\n{CounterAdding}");
        return numBags;
    }

    private int TraverseAllBags()
    {
        int count = 0;
        //Console.WriteLine($"My name: {Name}");
        Bag myBag = GetBagsCanHold(Name);

        if (myBag.BagsCanHold == null)
        {
            //Console.WriteLine($"Dont have other bags");
            count += 1;
            return count;
        }
        foreach (KeyValuePair<Bag, int> pair in myBag.BagsCanHold)
        {
            Bag bagCanHold = pair.Key;
            for (int pairCount = 0; pairCount < pair.Value; pairCount++)
            {
                count += bagCanHold.TraverseAllBags();
            }
            //Console.WriteLine($"My name: {myBag.Name}, Can hold: {bagCanHold.Name}");
            
        }
        


        count++;
        return count;
    }

    private void PrintBag()
    {
        Console.Write($"Name: {Name}, ");
        if (BagsCanHold == null)
        {
            Console.WriteLine("No other bags.");
            return;
        }
        foreach (KeyValuePair<Bag, int> bagCanHold in BagsCanHold)
        {
            Console.Write($"Number: {bagCanHold.Value}, Bag: {bagCanHold.Key.Name}");
        }
        Console.WriteLine();
    }
    private Bag GetBagsCanHold(string name)
    {
        foreach (KeyValuePair<string, Bag> relationShip in BagRelationships)
        {
            if (relationShip.Key == name)
                return relationShip.Value;
        }
        return null;
    }

    public int GetBagCanHoldCount(Bag bag)
    {
        int count = 0;
        //Console.WriteLine($"My name: {Name}");
        Bag myBag = GetBagsCanHold(Name);

        if (myBag.BagsCanHold == null)
        {
            //Console.WriteLine($"Dont have other bags");
            return count;
        }

        foreach (KeyValuePair<Bag, int> pair in myBag.BagsCanHold)
        {
            Bag bagCanHold = pair.Key;
            //Console.WriteLine($"My name: {myBag.Name}, Can hold: {bagCanHold.Name}");
            if (bagCanHold.Name == bag.Name)
            {
                //Console.WriteLine($"\nAdded bag\n");
                count += 1;
            }
            else
            {
                if (count == 1)
                    return count;

                //Console.WriteLine("Calling");
                count += bagCanHold.GetBagCanHoldCount(bag);
            }
            
        }

        return count;
    }

}
