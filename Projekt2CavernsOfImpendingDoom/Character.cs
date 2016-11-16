using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekt2CavernsOfImpendingDoom
{
    public class Character : BoardPiece
    {
        public Location Location { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public Character(string name)
        {
            Name = name;
        }
    }
}