using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingUtils
{
    public class GameBoardProtocol
    {
        static int totalId;

        public int Id { get; set; }
        public string Gameboard { get; set; }
        public string Version { get; set; }
        public List<string> Interactions { get; set; }


        public GameBoardProtocol(string gameboard)
        {
            Gameboard = gameboard;
            Interactions = new List<string>();
            totalId++;
            Id = totalId;
            Version = "1.0.0";
        }

    }
}
