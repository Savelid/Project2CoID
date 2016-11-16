using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client myClient = new Client();

            Thread clientThread = new Thread(myClient.Start);
            clientThread.Start();
            clientThread.Join();
        }

        public class Client
        {
            private TcpClient client;

            public void Start()
            {
                #region Get local IP
                IPHostEntry host;
                string localIP = "127.0.0.1";

                Console.WriteLine("Ange IP");
                //localIP = Console.ReadLine();
                #endregion

                //client = new TcpClient("192.168.220.116", 8080);
                client = new TcpClient(localIP, 5000);

                Thread listenerThread = new Thread(Listen);
                listenerThread.Start();

                Thread senderThread = new Thread(Send);
                senderThread.Start();

                senderThread.Join();
                listenerThread.Join();
            }

            public void Listen()
            {
                string message = "";

                try
                {
                    while (true)
                    {
                        NetworkStream n = client.GetStream();
                        message = new BinaryReader(n).ReadString();
                        Console.Clear();
                        Console.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void Send()
            {
                ConsoleKeyInfo message = new ConsoleKeyInfo();

                try
                {
                    NetworkStream n = client.GetStream();

                    while ((message.Key != ConsoleKey.Q) && (message.Key != ConsoleKey.Escape))
                    {
                        message = Console.ReadKey();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write(message.Key.ToString());
                        w.Flush();
                    }

                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
