using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII
{
    internal class Cell
    {
        private ConsoleColor backgroundColor;
        private ConsoleColor foregroundColor;
        private char character;

        public ConsoleColor BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        public ConsoleColor ForegroundColor { get => foregroundColor; set => foregroundColor = value; }
        public char Character { get => character; set => character = value; }

        public Cell(ConsoleColor bC, ConsoleColor fC, char c)
        {
            BackgroundColor = bC;
            ForegroundColor = fC;
            Character = c;
        }
    }
}
