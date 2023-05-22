namespace ASCII
{
    internal static class UI
    {
        public static List<string> MenuList = new()
        {
            "SetForegroundColor",
            "SetBackgroundColor",
            "SaveScene",
            "LoadScene",
            "SetGridDirectory",
            "SetImagesDirectory",
            "SetGifsDirectory",
            "ExportGif",
            "Guide"
        };
        private static int currentOption = 0;
        private static int menuHeight = MenuList.Count + 1 + 2;
        private static int menuWidth = GetMenuWidth();
        private static int currentColorOption = 0;
        public static int CurrentColorOption
        {
            get => currentColorOption;
            set
            {
                if (value >= 0 && value <= 15) currentColorOption = value;
                else currentColorOption = 0;
            }
        }
        public static int MenuHeight { get => menuHeight; set => menuHeight = value; }
        public static int MenuWidth { get => menuWidth; set => menuWidth = value; }
        private static int GetMenuX()
        {
            if (MenuWidth > Console.WindowWidth - Brush.X) return Brush.X - MenuWidth;
            else return Brush.X;
        }
        private static int GetMenuY()
        {
            if (MenuHeight > Console.WindowHeight - Brush.Y) return Brush.Y - MenuHeight;
            else return Brush.Y;
        }
        public static int CurrentOption
        {
            get => currentOption;
            set
            {
                if (value >= 0 && value < MenuList.Count) currentOption = value;
                else currentOption = 0;
            }
        }
        private static int GetSelectionX()
        {
            int x = GetMenuX() + GetMenuWidth();
            if (13 > Console.WindowWidth - GetMenuX() - GetMenuWidth()) x = GetMenuX() - 13;
            if (x < 0) x = 0;
            return x;
        }
        private static int GetSelectionY()
        {
            int y = GetMenuY();
            if (18 > Console.WindowHeight - GetMenuY()) y = GetMenuY() - 18;
            if (y < 0) y = 0;
            return y;
        }
        private static int GetMenuWidth()
        {
            int length = 0;
            foreach (var item in MenuList)
            {
                if (item.Length > length) length = item.Length;
            }
            return length + 3;
        }
        public static void ChangeOption(bool input)
        {
            switch (input)
            {
                case true:
                    if (CurrentOption != MenuList.Count - 1) CurrentOption++;
                    else CurrentOption = 0;
                    break;
                case false:
                    if (CurrentOption != 0) CurrentOption--;
                    else CurrentOption = MenuList.Count - 1;
                    break;
            }
            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
            Console.Write(' ');
            Console.SetCursorPosition(GetMenuX() - 1 + MenuWidth, GetMenuY() + 1 +CurrentOption);
            Console.Write('<');
        }
        public static void DrawMenu()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 1; i <= MenuList.Count; i++)
            {
                Console.SetCursorPosition(GetMenuX() + 1, GetMenuY() + i);
                Console.Write(MenuList[i - 1]);
            }
            for (int i = GetMenuX() + 1; i < GetMenuX() + MenuWidth; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(i, GetMenuY());
                Console.Write('_');
                Console.SetCursorPosition(i, GetMenuY() + MenuHeight);
                Console.Write('_');
            }
            for (int i = GetMenuY(); i < GetMenuY() + MenuHeight; i++)
            {
                Console.SetCursorPosition(GetMenuX(), i + 1);
                Console.Write('|');
                Console.SetCursorPosition(GetMenuX() + MenuWidth, i + 1);
                Console.Write('|');
            }
            Console.SetCursorPosition(GetMenuX() + 1, GetMenuY() + MenuList.Count + 1);
            Console.Write("Grids: " + Threads.GridList.Count);
            Console.SetCursorPosition (GetMenuX() + 1, GetMenuY() + MenuList.Count + 2);
            Console.Write("Current Grid: " + Threads.GridList.IndexOf(Threads.CurrentGrid));
            Console.SetCursorPosition(GetMenuX() - 1 + MenuWidth, GetMenuY() + 1 + CurrentOption);
            Console.Write('<');
        }
        public static void MenuAction()
        {
            switch (CurrentOption)
            {
                case 0:
                    DrawColorMenu();
                    WakeThread(Threads.ColorSelectionThread);
                    break;
                case 1:
                    DrawColorMenu();
                    WakeThread(Threads.ColorSelectionThread);
                    break;
                case 2:
                    WakeThread(Threads.SaveSceneThread);
                    break;
                case 3:
                    WakeThread(Threads.LoadSceneThread);
                    break;
                case 4: case 5: case 6:
                    WakeThread(Threads.SetNewPathThread);
                    break;
                case 7:
                    WakeThread(Threads.ExportGifThread);
                    break;
                case 8:
                    UI.EraseMenu();
                    WakeThread(Threads.GuideThread);
                    break;


            }
        }
        private static void WakeThread(Thread t)
        {
            try
            {
                if (t.IsAlive) t.Interrupt();
                else t.Start();
            }catch (Exception) { }
            
        }
        public static void DrawColorMenu()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 1; i < 13; i++)
            {
                Console.SetCursorPosition(GetSelectionX() + i, GetSelectionY());
                Console.Write('_');
                Console.SetCursorPosition(GetSelectionX() + i, GetSelectionY() + 17);
                Console.Write('_');
            }
            for (int i = 1; i <= 17; i++)
            {
                Console.SetCursorPosition(GetSelectionX(), GetSelectionY() + i);
                Console.Write('|');
                Console.SetCursorPosition(GetSelectionX() + 13, GetSelectionY() + i);
                Console.Write('|');
            }
            for(int i = 1; i < 18; i++)
            {
                Console.SetCursorPosition(GetSelectionX() + 1, GetSelectionY() + i);
                Console.Write(Enum.GetName(typeof(ConsoleColor), i - 1));
            }
            
        }
        public static void EraseMenu()
        {
            for (int i = 0; i <= MenuHeight; i++)
            {
                Console.SetCursorPosition(GetMenuX(), GetMenuY() + i);
                for(int j = 0; j <= MenuWidth; j++)
                {
                    Console.Write(' ');
                }
            }
        }
        public static void EraseColorMenu()
        {
            for (int i = 0; i < 18; i++)
            {
                Console.SetCursorPosition(GetSelectionX(), GetSelectionY() + i);
                for (int j = 0; j <= 13; j++) Console.Write(' ');
            }
        }
        public static void ChangeColorOption(bool input)
        {
            Console.SetCursorPosition(GetSelectionX() + 12, GetSelectionY() + CurrentColorOption + 1);
            Console.Write(' ');
            switch (input)
            {
                case true:
                    if (CurrentColorOption != 15) CurrentColorOption++;
                    else CurrentColorOption = 0;
                    break;
                case false:
                    if (CurrentColorOption != 0) CurrentColorOption--;
                    else CurrentColorOption = 15;
                    break;
            }
            Console.SetCursorPosition(GetSelectionX() + 12, GetSelectionY() + CurrentColorOption + 1);
            Console.Write('<');
        }
        public static void DisplayGuide()
        {
            List<string> messages = new()
            {
                "Welcome to ASCIIPaint!",
                "To begin painting, press Esc.",
                "Once in the scene view, press any button that corresponds to a character, to paint it.",
                "You can change the color of the font and that of the background through the Tab menu.",
                "To add another scene, press F1, and F5 to delete the current scene.",
                "You can save the current scene to a png file through the Tab menu.",
                "You can load a scene from a txt file through the Tab menu.",
                "(Note: this only works with txt files formatted and created when you save a scene)",
                "All files are saved to a given folders on your computer.",
                "By default, those folders are in the same directory as the exe file.",
                "You can specify a different path for each folder through the Tab menu.",
                "(Note 1: You must specify those paths each time You open the app)",
                "(Note 2: to set the path back to default, write 'default'",
                "You can export all scenes as a gif through the Tab menu.",
                "(Note: opening a different window might cause problems with gif exporting)"
            };
            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i][0] == '(') Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(Console.WindowWidth/2 - messages[i].Length/2, i);
                Console.Write(messages[i]);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
    
}
