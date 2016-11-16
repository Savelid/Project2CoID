using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingUtils
{
    public class ActionProtocol
    {
        static int totalId;

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Version { get; set; }

        public ActionProtocol(string userName, string action)
        {
            UserName = userName;
            Action = action;
            totalId++;
            Id = totalId;
            Version = "1.0.0";
        }
    }
}
