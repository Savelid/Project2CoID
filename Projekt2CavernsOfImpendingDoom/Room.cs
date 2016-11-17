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

                //for(int i = 0; i<Characters.Count; i++)
                //{
                //    if (Characters[i].Name.Equals(player.Name))
                //    {
                //        Characters.RemoveAt(i);
                //    }
                //}

                Characters.Remove(player);
                Console.WriteLine("Number of characters in room after removing player: " + Characters.Count);

                //foreach (Character character in Characters)
                //{
                //    if (character.Name.Equals(player.Name))
                //    {
                //        Characters.Remove(player);
                //    } 
                //}
            }
        }
    }
}