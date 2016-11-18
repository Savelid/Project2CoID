using NetworkingUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking_Client
{
    class Program
    {
        static string name;

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

                Console.WriteLine("Ange Namn");
                name = Console.ReadLine();

                //localIP = Console.ReadLine();
                #endregion

                client = new TcpClient("192.168.220.116", 8080);
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
                Console.OutputEncoding = System.Text.Encoding.Unicode;

                try
                {
                    while (true)
                    {
                        NetworkStream n = client.GetStream();
                        message = new BinaryReader(n).ReadString();
                        if (message != null && !message.Equals(""))
                        {
                            var toWrite = JsonConvert.DeserializeObject<GameBoardProtocol>(message);

                            if (toWrite is GameBoardProtocol && toWrite.Version.Equals("1.0.0"))
                            {
                                Console.Clear();
                                Console.WriteLine(toWrite.Gameboard);

                                foreach (var interaction in toWrite.Interactions)
                                {
                                    Console.WriteLine(interaction);
                                }
                                foreach (var stat in toWrite.Stats)
                                {
                                    Console.WriteLine(stat);
                                }
                            }
                        }
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
                        if(message.Key.ToString().Equals("Spacebar") || message.Key.ToString().Equals("Z"))
                        {
                            SoundPlayer player = new SoundPlayer();
                            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\sword-gesture1.wav";
                            player.Play();
                        }
                        BinaryWriter w = new BinaryWriter(n);
                        ActionProtocol ap = new ActionProtocol(name, message.Key.ToString());
                        string json = JsonConvert.SerializeObject(ap);
                        w.Write(json);
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
