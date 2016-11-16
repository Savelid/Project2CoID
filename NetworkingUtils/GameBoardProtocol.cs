using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingUtils
{
    class GameBoardProtocol
    {
        static int totalId;

        public int Id { get; set; }
        public string Gameboard { get; set; }
        public string Version { get; set; }

        public GameBoardProtocol(string gameboard)
        {
            Gameboard = gameboard;
            totalId++;
            Id = totalId;
            Version = "1.0.0";
        }
    }
}
