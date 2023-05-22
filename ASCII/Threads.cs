using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII
{
    internal static class Threads
    {
        public static Thread MainThread = new(new ThreadStart(Main));
        public static Thread MenuThread = new(new ThreadStart(Menu));
        public static Thread ColorSelectionThread = new(new ThreadStart(ColorSelection));
        public static Thread SaveSceneThread = new(new ThreadStart(SaveScene));
        public static Thread LoadSceneThread = new(new ThreadStart(LoadScene));
        public static Thread SetNewPathThread = new(new ThreadStart(SetNewPath));
        public static Thread ExportGifThread = new(new ThreadStart(ExportGif));
        public static Thread GuideThread = new(new ThreadStart(Guide));
        public static Grid InitialGrid = new();
        public static Grid CurrentGrid = InitialGrid;
        public static ConsoleColor MainForegroundColor = ConsoleColor.White;
        public static ConsoleColor MainBackgroundColor = ConsoleColor.Black;
        
        public static List<Grid> GridList = new()
        {
            InitialGrid
        };
        public static void Main()
        {
            GuideThread.Start();
            try
            {
                Thread.Sleep(Timeout.Infinite);
            } catch (Exception) { }
            SaveRead.DirectoriesIni();
            while (true)
            {
                Console.CursorVisible = true;
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.UpArrow) Brush.UpdateCoordinates(0, -1);
                else if (input.Key == ConsoleKey.DownArrow) Brush.UpdateCoordinates(0, 1);
                else if (input.Key == ConsoleKey.LeftArrow) Brush.UpdateCoordinates(-1, 0);
                else if (input.Key == ConsoleKey.RightArrow) Brush.UpdateCoordinates(1, 0);
                else if (input.Key == ConsoleKey.Tab)
                {
                    Console.CursorVisible = false;
                    UI.DrawMenu();
                    try
                    {
                        if (MenuThread.IsAlive) MenuThread.Interrupt();
                        else MenuThread.Start();
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch (Exception) { }
                }
                else if(input.Key == ConsoleKey.F1)
                {
                    GridList.Add(new Grid());
                }
                else if(input.Key == ConsoleKey.F5)
                {
                    if (GridList.Count > 1)
                    {
                        int index = GridList.IndexOf(CurrentGrid);
                        GridList.Remove(GridList[index]);
                        if (index == 0) CurrentGrid = GridList[index];
                        else CurrentGrid = GridList[index - 1];
                        CurrentGrid.DrawGrid();
                    }
                    else
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 14, Console.WindowHeight / 2);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Cannot remove the only grid!");
                        Thread.Sleep(700);
                        CurrentGrid.DrawGrid();
                    }
                }
                else if(input.Key == ConsoleKey.NumPad4)
                {
                    CurrentGrid = Grid.GetPreviousGrid(GridList, GridList.IndexOf(CurrentGrid));
                    CurrentGrid.DrawGrid();
                }
                else if(input.Key == ConsoleKey.NumPad6)
                {
                    CurrentGrid = Grid.GetNextGrid(GridList, GridList.IndexOf(CurrentGrid));
                    CurrentGrid.DrawGrid();
                }
                else
                {
                    CurrentGrid.GridArray[Brush.X, Brush.Y] = new Cell(MainBackgroundColor, MainForegroundColor, input.KeyChar);
                    Console.BackgroundColor = MainBackgroundColor;
                    Console.ForegroundColor = MainForegroundColor;
                    Console.Write(input.KeyChar);
                }
                if (Brush.X > Console.WindowWidth || Brush.Y > Console.WindowHeight)
                {
                    Brush.X = 0;
                    Brush.Y = 0;
                    CurrentGrid.DrawGrid();
                } 
                
                Console.SetCursorPosition(Brush.X, Brush.Y);
                
            }
        }
        public static void Menu()
        {
            while (true)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Tab)
                {
                    UI.EraseMenu();
                    CurrentGrid.DrawGrid();
                    try
                    {
                        MainThread.Interrupt();
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch (Exception) { }
                }
                else if (input.Key == ConsoleKey.UpArrow) UI.ChangeOption(false);
                else if (input.Key == ConsoleKey.DownArrow) UI.ChangeOption(true);
                else if (input.Key == ConsoleKey.Enter)
                {
                    UI.MenuAction();
                    try
                    {
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch(Exception) { }
                }
            }
        }
        public static void ColorSelection()
        {
            while (true)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Tab)
                {
                    UI.EraseColorMenu();
                    UI.DrawMenu();
                    try
                    {
                        MenuThread.Interrupt();
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch (Exception) { }
                }
                else if (input.Key == ConsoleKey.UpArrow) UI.ChangeColorOption(false);
                else if (input.Key == ConsoleKey.DownArrow) UI.ChangeColorOption(true);
                else if( input.Key == ConsoleKey.Enter)
                {
                    if (UI.CurrentOption == 0) MainForegroundColor = (ConsoleColor)UI.CurrentColorOption;
                    else if (UI.CurrentOption == 1) MainBackgroundColor = (ConsoleColor)UI.CurrentColorOption;
                }
            }
        }
        public static void SaveScene()
        {
            while (true)
            {
                Console.SetCursorPosition(Console.WindowWidth/2, Console.WindowHeight/2);
                Console.Write("Please specify the name of the file:");
                Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2 + 1);
                string fileName = Console.ReadLine();
                CurrentGrid.DrawGrid();
                SaveRead.SaveScene(fileName);
                SaveRead.SaveGrid(CurrentGrid, fileName);
                try
                {
                    MainThread.Interrupt();
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (Exception) { }
            }
        }
        public static void LoadScene()
        {
            while (true)
            {
                Console.SetCursorPosition(Console.WindowWidth/2, Console.WindowHeight/2);
                Console.Write("Please specify the full path of the file:");
                Console.SetCursorPosition (Console.WindowWidth / 2, Console.WindowHeight / 2 + 1);
                string s = Console.ReadLine();
                SaveRead.LoadGrid(s);
                try
                {
                    MainThread.Interrupt();
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (Exception) { }
            }
        }
        public static void SetNewPath()
        {
            while (true)
            {
                SaveRead.SetPath(UI.CurrentOption - 4);
                try
                {
                    MenuThread.Interrupt();
                    Thread.Sleep(Timeout.Infinite);
                } catch (Exception) { }
            }
        }
        public static void ExportGif()
        {
            while (true)
            {
                SaveRead.CreateGif();
                UI.DrawMenu();
                try
                {
                    MenuThread.Interrupt();
                    Thread.Sleep(Timeout.Infinite);
                } catch (Exception) { }
            }
        }
        public static void Guide()
        {
            
            while (true)
            {
                UI.DisplayGuide();
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    CurrentGrid.DrawGrid();
                    try
                    {
                        MainThread.Interrupt();
                        Thread.Sleep(Timeout.Infinite);
                    }catch (Exception) { }
                    
                }
            }
        }
    }
}
