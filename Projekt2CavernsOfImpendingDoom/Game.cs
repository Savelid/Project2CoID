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

        public string GetGameBoard()
        {
            string board = GameBoard.GetGameBoardString();


            return board;
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
                case "Spacebar":
                    //slå på en spelare
                    if (CheckRoomForOthers(player))
                    {
                        HitPlayers(player);
                    }


                    break;
                default:
                    Console.WriteLine("Key not allowed!");
                    break;
            }
            GameBoard.AddPlayerToRoom(player);
        }

        private void HitPlayers(Player player)
        {
            foreach (Character character in GameBoard.GetRoomCharacters(player) )
            {
                character.Health -= player.Strength;
            }
        }

        internal string GetProtocol()
        {
            GameBoardProtocol gp = new GameBoardProtocol(GameBoard.GetGameBoardString());
            foreach (Player otherplayer in Players)
            {
                string stats = otherplayer.Name + Environment.NewLine + "Health: " + otherplayer.Health + Environment.NewLine;
                stats += "Strength: " + otherplayer.Strength + Environment.NewLine;

                gp.Stats.Add(stats);

            }
            string toSend = JsonConvert.SerializeObject(gp);
            return toSend;
        }

        internal bool CheckRoomForOthers(Player player)
        {
            bool othersInRoom = false;
            if (GameBoard.GetRoomCharacters(player).Count > 1)
                othersInRoom = true;

            return othersInRoom;
        }
    }
}