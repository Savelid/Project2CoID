using NetworkingUtils;
using Newtonsoft.Json;
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
            GameBoard.RemovePlayerFromRoom(player);
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
                    Console.WriteLine("Key not allowed!");
                    break;
            }
            GameBoard.AddPlayerToRoom(player);
        }

        internal string GetProtocol(string message, Player player)
        {
            GameBoardProtocol gp = new GameBoardProtocol(message);
            if (GameBoard.rooms[player.Location.X, player.Location.Y].Characters.Count > 1)
            {
                gp.Interactions.Add("HejHej");
            }
            string toSend = JsonConvert.SerializeObject(gp);
            return toSend;
        }
    }
}