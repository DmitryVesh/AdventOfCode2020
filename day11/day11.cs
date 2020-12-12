using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using AdventOfCode2020;
using System.Drawing;

public class day11
{
    //grid. Each position is either floor (.), an empty seat (L), or an occupied seat (#)

    //All decisions are based on the number of occupied seats adjacent to a given seat

    //If a seat is empty(L) and there are no occupied seats adjacent to it, the seat becomes occupied.
    //If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
    //Otherwise, the seat's state does not change.

    //Floor (.) never changes; seats don't move, and nobody sits on the floor
    public static int GridMaxDim = 0;
    public static int GridVerticalDim = 0;

    public static void Run()
    {
        List<string> input = ReadInputFile.GetInputAsLines("D:/C# projects/AdventOfCode2020/day11/day11.txt");
        List<GridPoint> grid = GetGrid(input);
        GridMaxDim = grid.Count / input.Count;
        GridVerticalDim = input.Count;
        int numFinalOccupied = GetFinalOccupied(grid);
        Console.WriteLine($"Final occupied: {numFinalOccupied}");
    }

    private static int GetFinalOccupied(List<GridPoint> grid)
    {
        int numOccupied = 0;
        int numRuns = 0;

        while (true)
        {
            //////////////////////////////
            int currentOccupied = 0;
            List<GridPoint> seatsToChanged = new List<GridPoint>();

            char[,] trueGrid = GetTrueGrid(grid);

            foreach (GridPoint gridPointCurrent in grid)
            {
                char currentItem = gridPointCurrent.Item;

                if (currentItem == '.')
                    continue;

                int numNeighbours = 0;

                //Foreach surrouding grid 
                List<GridPoint> surroundGrid = GetSurroundingGridPoints(gridPointCurrent, grid);

                foreach (GridPoint gridPointOther in surroundGrid)
                {
                    if (gridPointOther.Item == '#')
                        numNeighbours++;
                    else if (gridPointOther.Item == '.')
                    {
                        if (AddNeighbour(gridPointCurrent, gridPointOther, trueGrid))
                            numNeighbours++;
                    }
                }

                bool change = false;
                if (currentItem == 'L' && numNeighbours == 0)
                {
                    currentItem = '#';
                    change = true;
                }
                else if (currentItem == '#' && numNeighbours >= 5)
                {
                    currentItem = 'L';
                    change = true;
                }

                if (change)
                {
                    GridPoint toAdd = new GridPoint();
                    toAdd.Point = gridPointCurrent.Point;
                    toAdd.Item = currentItem;
                    seatsToChanged.Add(toAdd);
                }
            }

            List<GridPoint> newGrid = new List<GridPoint>(grid);

            for (int currentGridCount = 0; currentGridCount < grid.Count; currentGridCount++)
            {
                foreach(GridPoint replace in seatsToChanged)
                {
                    if (grid[currentGridCount].Point == replace.Point && grid[currentGridCount].Item != replace.Item)
                    {
                        newGrid[currentGridCount] = replace;
                        break;
                    }
                }
            }

            grid = newGrid;
            currentOccupied = CountItemsInGrid(grid, '#');
            //WriteBoard(grid);
            //////////////////////////////
            if (numOccupied == currentOccupied)
            {
                break;
            }
            numOccupied = currentOccupied;
            numRuns++;
        }

        return numOccupied;
    }

    private static bool AddNeighbour(GridPoint gridPointCurrent, GridPoint gridPointOther, char[,] trueGrid)
    {
        int currentX = gridPointCurrent.Point.X;
        int currentY = gridPointCurrent.Point.Y;

        int otherX = gridPointOther.Point.X;
        int otherY = gridPointOther.Point.Y;

        int differenceX = otherX - currentX;
        int differenceY = otherY - currentY;

        int directionGoing = GetDirection(differenceX, differenceY);



        bool[] onlyCheck = new bool[8];
        onlyCheck[directionGoing] = true;

        try
        {
            for (int gridLength = 0; gridLength < GridMaxDim; gridLength++)
            {

                if (onlyCheck[2] && trueGrid[otherY, otherX + 1 + gridLength] == '#')
                    return true;
                else if (onlyCheck[2] && trueGrid[otherY, otherX + 1 + gridLength] == 'L')
                    return false;

                if (onlyCheck[1] && trueGrid[otherY - 1 - gridLength, otherX + 1 + gridLength] == '#')
                    return true;
                else if (onlyCheck[1] && trueGrid[otherY - 1 - gridLength, otherX + 1 + gridLength] == 'L')
                    return false;

                if (onlyCheck[3] && trueGrid[otherY + 1 + gridLength, otherX + 1 + gridLength] == '#')
                    return true;
                else if (onlyCheck[3] && trueGrid[otherY + 1 + gridLength, otherX + 1 + gridLength] == 'L')
                    return false;


                if (onlyCheck[6] && trueGrid[otherY, otherX - 1 - gridLength] == '#')
                    return true;
                else if (onlyCheck[6] && trueGrid[otherY, otherX - 1 - gridLength] == 'L')
                    return false;

                if (onlyCheck[7] && trueGrid[otherY - 1 - gridLength, otherX - 1 - gridLength] == '#')
                    return true;
                else if (onlyCheck[7] && trueGrid[otherY - 1 - gridLength, otherX - 1 - gridLength] == 'L')
                    return false;

                if (onlyCheck[5] && trueGrid[otherY + 1 + gridLength, otherX - 1 - gridLength] == '#')
                    return true;
                else if (onlyCheck[5] && trueGrid[otherY + 1 + gridLength, otherX - 1 - gridLength] == 'L')
                    return false;

                if (onlyCheck[0] && trueGrid[otherY - 1 - gridLength, otherX] == '#')
                    return true;
                else if (onlyCheck[0] && trueGrid[otherY - 1 - gridLength, otherX] == 'L')
                    return false;

                if (onlyCheck[4] && trueGrid[otherY + 1 + gridLength, otherX] == '#')
                    return true;
                else if (onlyCheck[4] && trueGrid[otherY + 1 + gridLength, otherX] == 'L')
                    return false;


            }
        }
        catch (IndexOutOfRangeException) { }
        
        
        return false;
    }

    private static int GetDirection(int differenceX, int differenceY)
    {
        int direction = -1;
        switch (differenceX)
        {
            case 1:
                switch (differenceY)
                {
                    case -1:
                        direction = 1;
                        break;
                    case 0:
                        direction = 2;
                        break;
                    case 1:
                        direction = 3;
                        break;
                }
                break;
            case -1:
                switch (differenceY)
                {
                    case -1:
                        direction = 7;
                        break;
                    case 0:
                        direction = 6;
                        break;
                    case 1:
                        direction = 5;
                        break;
                }
                break;
            case 0:
                switch (differenceY)
                {
                    case -1:
                        direction = 0;
                        break;
                    case 1:
                        direction = 4;
                        break;
                }
                break;
        }
        return direction;
    }

    private static char[,] GetTrueGrid(List<GridPoint> grid)
    {
        char[,] trueGrid = new char[GridVerticalDim, GridMaxDim];
        int gridCount = 0;
        for (int y = 0; y < GridVerticalDim; y++)
        {
            for (int x = 0; x < GridMaxDim; x++)
            {
                trueGrid[y, x] = grid[gridCount].Item;
                gridCount++;
            }
        }
        return trueGrid;
    }

    private static List<GridPoint> GetSurroundingGridPoints(GridPoint gridPointCurrent, List<GridPoint> grid)
    {
        List<GridPoint> surroudingPoints = new List<GridPoint>();
        int myPointX = gridPointCurrent.Point.X;
        int myPointY = gridPointCurrent.Point.Y;


        foreach (GridPoint point in grid)
        {
            int otherPointX = point.Point.X;
            int otherPointY = point.Point.Y;

            if (otherPointX - 1 == myPointX && (otherPointY == myPointY || otherPointY - 1 == myPointY || otherPointY + 1 == myPointY))
                surroudingPoints.Add(point);
            else if (otherPointX + 1 == myPointX && (otherPointY == myPointY || otherPointY - 1 == myPointY || otherPointY + 1 == myPointY))
                surroudingPoints.Add(point);
            else if (otherPointX == myPointX && (otherPointY - 1 == myPointY || otherPointY + 1 == myPointY))
                surroudingPoints.Add(point);
        }

        return surroudingPoints;
    }

    private static void WriteBoard(List<GridPoint> grid)
    {
        int xCount = 0;
        foreach (GridPoint item in grid)
        {


            if (xCount > 0 && xCount % GridMaxDim == 0)
            {
                Console.WriteLine();
                Console.Write(item.Item);
            }
            else
                Console.Write(item.Item);
            xCount++;
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    private static int CountItemsInGrid(List<GridPoint> grid, char item)
    {
        int count = 0;
        foreach (GridPoint gridPoint in grid)
        {
            if (gridPoint.Item == item)
                count++;
        }
        return count;
    }

    private static bool AdjacentPoints(Point current, Point other, ref bool[] dirPresent)
    {
        bool adjacent = false;

        int currentX = current.X;
        int currentY = current.Y;

        int otherX = other.X;
        int otherY = other.Y;

        for (int gridLength = 0; gridLength <= GridMaxDim; gridLength++)
        {
            if (!dirPresent[2] && (otherX + 1 + gridLength) == currentX && otherY == currentY)
            {
                dirPresent[2] = true;
                adjacent = true;
            }
            else if (!dirPresent[1] && (otherX + 1 + gridLength) == currentX && (otherY + 1 + gridLength) == currentY)
            {
                dirPresent[1] = true;
                adjacent = true;
            }
            else if (!dirPresent[3] && (otherX + 1 + gridLength) == currentX && (otherY - 1 - gridLength) == currentY)
            {
                dirPresent[3] = true;
                adjacent = true;
            }

            else if (!dirPresent[6] && (otherX - 1 - gridLength) == currentX && otherY == currentY)
            {
                dirPresent[6] = true;
                adjacent = true;
            }
            else if (!dirPresent[7] && (otherX - 1 - gridLength) == currentX && (otherY + 1 + gridLength) == currentY)
            {
                dirPresent[7] = true;
                adjacent = true;
            }
            else if (!dirPresent[5] && (otherX - 1 - gridLength) == currentX && (otherY - 1 - gridLength) == currentY)
            {
                dirPresent[5] = true;
                adjacent = true;
            }

            else if (!dirPresent[0] && otherX == currentX && (otherY + 1 + gridLength) == currentY)
            {
                dirPresent[0] = true;
                adjacent = true;
            }
            else if (!dirPresent[4] && otherX == currentX && (otherY - 1 - gridLength) == currentY)
            {
                dirPresent[4] = true;
                adjacent = true;
            }

            if (adjacent)
                break;
        }
        

        return adjacent;
    }

    private static List<GridPoint> GetGrid(List<string> input)
    {
        List<GridPoint> grid = new List<GridPoint>();

        int y = 0;
        int x = 0;
        foreach (string line in input)
        {

            foreach (char item in line)
            {
                GridPoint gridPoint = new GridPoint();
                gridPoint.Point = new Point(x, y);
                gridPoint.Item = item;

                grid.Add(gridPoint);
                x++;
            }
            x = 0;
            y++;
        }

        return grid;
    }
}

public class GridPoint
{
    public Point Point { get; set; }
    public char Item { get; set; }
}