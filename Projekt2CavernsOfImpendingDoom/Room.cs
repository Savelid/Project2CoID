using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekt2CavernsOfImpendingDoom
{
    public class Room
    {
        public Location Location { get; set; }
        public List<Character> Characters { get; set; }
        public Room(Location location)
        {
            Location = location;
            Characters = new List<Character>();
        }
    }
}