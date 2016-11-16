using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekt2CavernsOfImpendingDoom
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public GameBoard GameBoard { get; set; }

        public Game(int width, int length)
        {
            Players = new List<Player>();
            GameBoard = new GameBoard(width, length);
            //var player = new Player("Pär");
            //player.Location = new Location(2, 2);
            //Players.Add(player);
            //GameBoard.AddPlayer(player);
        
        }

        public void HandlePlayerMovement(string message)
        {
            GameBoard.RemovePlayer(Players[0]);
            switch (message)
            {                    
                case "LeftArrow":
                    Players[0].Location.X--;
                    break;
                case "RightArrow":
                    Players[0].Location.X++;
                    break;
                case "UpArrow":
                    Players[0].Location.Y--;
                    break;
                case "DownArrow":
                    Players[0].Location.Y++;
                    break;
                default:
                    break;
            }
            GameBoard.AddPlayer(Players[0]);
        }

    }
}