﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2CavernsOfImpendingDoom
{
    class HealthPotion : Item
    {
        public HealthPotion() : base()
        {
            Symbol = '\u2665'; /*'\u2764';*/
            Name = "Health Potion";
        }

        public override void PickUp(Player player)
        {
            base.PickUp(player);
            player.Health += 10;
        }
    }


}
