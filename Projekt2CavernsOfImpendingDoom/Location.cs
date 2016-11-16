using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekt2CavernsOfImpendingDoom
{
    public class Location
    {
        private int x;
        private int y;
        public int X { get { return x; } set { x = (value >= 0) ? value : 0; } }
        public int Y { get { return y; } set { y = (value >= 0) ? value : 0; } }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}