using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekt2CavernsOfImpendingDoom
{
    public class Character : BoardPiece
    {
        public Location Location { get; set; }
        private int health;

        public int Health
        {
            get { return health; }
            set { health = health < 0 ? 0: value; }
        }

        public int Strength { get; set; }
        private bool isDead;

        public bool IsDead
        {
            get { return Health <= 0; }
        }

        public Character(string name)
        {
            Name = name;
            Health = 100;
            Strength = 10;
        }
    }
}