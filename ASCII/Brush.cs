using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII
{
    internal static class Brush
    {
        private static int x = 0;
        private static int y = 0;

        public static int X
        {
            get { return x; }
            set { if (value >= 0 && value < Console.WindowWidth) x = value; }
        }
        public static int Y
        {
            get { return y; }
            set { if (value >= 0 && value < Console.WindowHeight) y = value; }
        }

        public static void UpdateCoordinates(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }
    }
}
