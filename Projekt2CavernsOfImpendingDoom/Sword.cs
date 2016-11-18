using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2CavernsOfImpendingDoom
{
    class Sword : Item
    {
        public Sword() : base()
        {
            Symbol = '\u2666';//'\u2020';
            Name = "Sword";
        }

        public override void PickUp(Player player)
        {
            base.PickUp(player);
            player.Strength += 5;
        }
    }
}
