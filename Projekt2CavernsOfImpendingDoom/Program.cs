using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NetworkingUtils;

namespace Projekt2CavernsOfImpendingDoom
{
    class Program
    {
        static Game game;
        public static Thread serverThread;
        public static TcpListener listener;
        static void Main(string[] args)
        {
            game = new Game(10,10);

            Server myServer = new Server();
            serverThread = new Thread(myServer.Run);
            serverThread.Start();

            Thread commandThread = new Thread(myServer.getCommand);
            commandThread.Start();

            serverThread.Join();
            commandThread.Join();
        }

        public class Server
        {

            List<ClientHandler> clients = new List<ClientHandler>();
            public void Run()
            {
                listener = new TcpListener(IPAddress.Any, 5000);
                Console.WriteLine("Server up and running, waiting for messages...");

                try
                {
                    listener.Start();

                    while (true)
                    {
                        TcpClient c = listener.AcceptTcpClient();
                        ClientHandler newClient = new ClientHandler(c, this);
                        clients.Add(newClient);

                        Thread clientThread = new Thread(newClient.Run);
                        clientThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (listener != null)
                        listener.Stop();
                }
            }

            public void Broadcast(ClientHandler client, string jsonToSend)
            {


                foreach (ClientHandler tmpClient in clients)
                {
                    //if (tmpClient == client) { 
                    //    message += "\nadded interactions";
                    //}

                    NetworkStream n = tmpClient.tcpclient.GetStream();
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write(jsonToSend);
                    w.Flush();
                }
            }

            public void DisconnectClient(ClientHandler client)
            {
                clients.Remove(client);
                Console.WriteLine("Client X has left the building...");
                Broadcast(client, "Client X has left the building...");
            }

            internal void getCommand()
            {
                Console.WriteLine("Waiting for commands");

                string message = "";
                while (message != "quit")
                {
                    message = Console.ReadLine();
                    Console.WriteLine(message);

                    switch (message)
                    {
                        case "quit":
                            //skicka quit till alla clients
                            foreach (var client in clients)
                            {
                                NetworkStream n = client.tcpclient.GetStream();
                                BinaryWriter w = new BinaryWriter(n);
                                w.Write("quit");
                                w.Flush();
                            }
                            //stäng server
                            try
                            {
                                serverThread.Interrupt();
                                //serverThread.Abort();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            finally
                            {
                                if (listener != null)
                                {
                                    listener.Stop();
                                }
                            }
                            break;
                        case "clients":
                            //skriv ut antal clients
                            Console.WriteLine($"I have {clients.Count} clients.");
                            break;
                    }
                }
            }
        }

        public class ClientHandler
        {
            public TcpClient tcpclient;
            private Server myServer;
            public ClientHandler(TcpClient c, Server server)
            {
                tcpclient = c;
                myServer = server;
            }

            public void Run()
            {
                //var name = "";
                //try
                //{
                //    while (name == "")
                //    {
                //        NetworkStream n = tcpclient.GetStream();
                //        name = new BinaryReader(n).ReadString();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}

                //var newPlayer = new Player(name);
                //Console.WriteLine("Name:" + name);
                //newPlayer.Location = new Location(1, 1);
                //game.Players.Add(newPlayer);
                //game.GameBoard.AddPlayer(newPlayer);

                try
                {
                    Player newPlayer = null;
                    string gameBoardString = "";
                    string message = "";
                    while (!message.Equals("quit"))
                    {
                        NetworkStream n = tcpclient.GetStream();
                        message = new BinaryReader(n).ReadString();
                        var ap = JsonConvert.DeserializeObject<ActionProtocol>(message);

                        Console.WriteLine(ap.Action, ap.UserName);

                        if (newPlayer == null)
                        {
                            newPlayer = CreateNewPlayer(ap);
                        }

                        //fixa interactions
                        game.HandlePlayerMovement(ap.Action, newPlayer);
                        gameBoardString = game.GameBoard.GetGameBoardString();
                        string jsonToSend = game.GetProtocol(gameBoardString, newPlayer);

                        myServer.Broadcast(this, jsonToSend);

                        Console.WriteLine(ap.Action);
                    }

                    myServer.DisconnectClient(this);
                    tcpclient.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            private static Player CreateNewPlayer(ActionProtocol ap)
            {
                Player newPlayer = new Player(ap.UserName);
                newPlayer.Location = new Location(1, 1);
                game.Players.Add(newPlayer);
                game.GameBoard.AddPlayerToRoom(newPlayer);
                return newPlayer;
            }
        }
    }
}
