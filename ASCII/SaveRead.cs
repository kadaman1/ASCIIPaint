using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using AnimatedGif;

namespace ASCII
{
    internal static class SaveRead
    {
        public static string DefaultGridsDirectory = Directory.GetCurrentDirectory() + @"\Grids";
        public static string DefaultImagesDirectory = Directory.GetCurrentDirectory() + @"\Images";
        public static string DefaultGifsDirectory = Directory.GetCurrentDirectory() + @"\Gifs";
        public static string GridsDirectory = DefaultGridsDirectory;
        public static string ImagesDirectory = DefaultImagesDirectory;
        public static string GifsDirectory = DefaultGifsDirectory;
        public static void SaveScene(string fileName)
        {
            string formattedFileName = fileName.Split('.')[0];
            Bitmap image = ScreenCapture.CaptureActiveWindow();
            image.Save(ImagesDirectory + @"\" + formattedFileName + ".png");

        }
        public static void SaveGrid(Grid grid, string fileName)
        {
            string formattedFileName = fileName.Split('.')[0];
            using (StreamWriter sw = new(GridsDirectory + @"\" + formattedFileName + ".txt"))
            {
                sw.WriteLine(grid.ToString());
            }
        }
        public static void DirectoriesIni()
        {
            Directory.CreateDirectory(GridsDirectory);
            Directory.CreateDirectory(ImagesDirectory);
            Directory.CreateDirectory(GifsDirectory);
        }
        public static void SetPath(int option)
        {
            Console.SetCursorPosition(Console.WindowWidth/2, Console.WindowHeight/2);
            Console.Write("Please specify the new path: ");
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2 + 1);
            string newPath = Console.ReadLine();
            try
            {
                if (Directory.Exists(newPath))
                {
                    switch (option)
                    {
                        case 0:
                            GridsDirectory = newPath;
                            break;
                        case 1:
                            ImagesDirectory = newPath;
                            break;
                        case 2:
                            GifsDirectory = newPath;
                            break;
                    }
                }
                else if (newPath == "default")
                {
                    switch (option)
                    {
                        case 0:
                            GridsDirectory = DefaultGridsDirectory;
                            break;
                        case 1:
                            ImagesDirectory = DefaultImagesDirectory;
                            break;
                        case 2:
                            GifsDirectory = DefaultGifsDirectory;
                            break;
                    }
                }
                else
                {
                    DisplayErrorMessage("Invalid path");
                }
            }catch (Exception)
            {
                DisplayErrorMessage("Invalid path");
            }
           
            
            Threads.CurrentGrid.DrawGrid();
            UI.DrawMenu();
        }
        public static void LoadGrid(string loadPath)
        {
            if (File.Exists(loadPath))
            {
                using (StreamReader sr = new(loadPath))
                {
                    Threads.GridList.Add(Grid.ToGrid(sr.ReadLine()));
                }
            }
            else DisplayErrorMessage("Invalid path");
            
            UI.EraseMenu();
            Threads.CurrentGrid.DrawGrid();
        }
        public static void DisplayErrorMessage(string s)
        {
            Threads.CurrentGrid.DrawGrid();
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(s);
            Thread.Sleep(500);
        }
        public static void CreateGif()
        {
            List<Bitmap> sceneList = new();

            Console.SetCursorPosition((Console.WindowWidth / 2), Console.WindowHeight / 2);
            Console.Write("Please specify the name of the file:");
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2 + 1);
            string fileName = Console.ReadLine();
            Console.SetCursorPosition((Console.WindowWidth / 2), Console.WindowHeight / 2 + 2);
            Console.Write("Please specify the delay between the frames: (ms)");
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2 + 3);
            string delay = Console.ReadLine();
            try
            {
                if (int.TryParse(delay, out int intDelay))
                {
                    AnimatedGifCreator creator = new(GifsDirectory + @"\" + fileName.Split('.')[0] + ".gif", intDelay, int.MaxValue);
                    for (int i = 0; i < Threads.GridList.Count; i++)
                    {
                        Threads.CurrentGrid = Threads.GridList[i];
                        Threads.CurrentGrid.DrawGrid();
                        sceneList.Add(ScreenCapture.CaptureActiveWindow());
                    }
                    foreach (var item in sceneList)
                    {
                        creator.AddFrame(item);
                    }
                    creator.Dispose();
                }
                else
                {       
                    DisplayErrorMessage("Invalid value");
                    Threads.CurrentGrid.DrawGrid();
                }
            }catch (Exception)
            {
                DisplayErrorMessage("Invalid path");
                Threads.CurrentGrid.DrawGrid();
            }
        }
    }
}
