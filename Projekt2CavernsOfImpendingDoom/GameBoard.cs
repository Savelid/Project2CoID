using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekt2CavernsOfImpendingDoom
{
    public class GameBoard
    {
        public Room[,] rooms { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public GameBoard(int xSize, int ySize)
        {
            Width = xSize;
            Height = ySize;
            CreateRooms();
        }

        private void CreateRooms()
        {
            rooms = new Room[Width, Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    rooms[x, y] = new Room(new Location(x, y));
                }
            }
        }

        public string GetGameBoardString()
        {
            string roomString = "";

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (rooms[x, y].Characters.Count == 0)
                    {
                        roomString += "[ ]";
                    }
                    else
                    {
                        roomString += "[P]";
                    }
                }
                roomString += Environment.NewLine;

            }
            return roomString;
        } 
    }
}