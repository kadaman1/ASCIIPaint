using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII
{
    internal class Grid
    {
        public Cell[,] GridArray { get; set; }
        public Grid()
        {
            GridArray = new Cell[Console.LargestWindowWidth,Console.LargestWindowHeight];
            for (int i = 0; i < GridArray.GetLength(0); i++)
            {
                for(int j = 0; j < GridArray.GetLength(1); j++)
                {
                    GridArray[i, j] = new Cell(ConsoleColor.Black, ConsoleColor.Black, ' ');
                }
            }
        }

        public void DrawGrid()
        {
            for(int i = 0; i < Console.WindowWidth; i++)
            {
                for (int j = 0; j < Console.WindowHeight; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.BackgroundColor = GridArray[i, j].BackgroundColor;
                    Console.ForegroundColor = GridArray[i, j].ForegroundColor;
                    Console.Write(GridArray[i, j].Character);
                }
            }
        }
        public static Grid GetNextGrid(List<Grid> gridList, int index)
        {
            if (index == gridList.Count - 1) return gridList[0];
            else return gridList[index + 1];
        }
        public static Grid GetPreviousGrid(List<Grid> gridList, int index)
        {
            if (index == 0) return gridList[gridList.Count - 1];
            else return gridList[index - 1];
        }
        public override string ToString()
        {
            string saveString = "";

            for (int i = 0; i < GridArray.GetLength(1); i++)
            {
                for (int j = 0; j < GridArray.GetLength(0); j++)
                {
                    saveString += Convert.ToString((int)GridArray[j, i].BackgroundColor, 16);
                    saveString += Convert.ToString((int)GridArray[j, i].ForegroundColor, 16);
                    saveString += GridArray[j, i].Character;
                }
                saveString += '/';
            }
            return saveString;
        }
        public static Grid ToGrid(string inputString)
        {
            string[] gridRows = inputString.Split('/');
            Grid grid = new();
            for (int i = 0; i < gridRows.Length; i++)
            {
                ConsoleColor background = ConsoleColor.Black;
                ConsoleColor foreground = ConsoleColor.White;
                char c;
                for (int j = 0; j < gridRows[i].Length; j++)
                {
                    switch (j % 3)
                    {
                        case 0:
                            background = (ConsoleColor)Int32.Parse(gridRows[i][j].ToString(), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 1:
                            foreground = (ConsoleColor)Int32.Parse(gridRows[i][j].ToString(), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                            c = gridRows[i][j];
                            grid.GridArray[(j - 2)/3, i] = new Cell(background, foreground, c);
                            break;
                    }
                }
            }
            return grid;
        }
    }
}
