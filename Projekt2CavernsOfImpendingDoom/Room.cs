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

        public List<Item> Items { get; set; }
        public Room(Location location)
        {
            Location = location;
            Characters = new List<Character>();
            Items = new List<Item>();
        }

        public void AddPlayer(Player player)
        {
            lock (Characters)
            {
                Characters.Add(player);
            }
        }

        public void RemovePlayer(Player player)
        {
            lock (Characters)
            {
                Characters.Remove(player);
            }
        }
    }
}