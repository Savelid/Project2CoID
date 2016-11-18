using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2CavernsOfImpendingDoom
{
    public abstract class Item : BoardPiece
    {
        public static int counter = 0;
        public Item()
        {
            counter++;
        }

        public virtual void PickUp(Player player)
        {
            counter--;
            
        }
    }
}
