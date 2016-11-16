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

        public void HandlePlayerMovement(string message, Player player)
        {
            GameBoard.RemovePlayer(player);
            switch (message)
            {                    
                case "LeftArrow":
                    player.Location.X--;
                    break;
                case "RightArrow":
                    if (player.Location.X < GameBoard.Width -1)
                        player.Location.X++;
                    break;
                case "UpArrow":
                    player.Location.Y--;
                    break;
                case "DownArrow":
                    if (player.Location.Y < GameBoard.Height - 1)
                        player.Location.Y++;
                    break;
                default:
                    break;
            }
            GameBoard.AddPlayer(player);
        }

    }
}