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
            game = new Game(10, 10);

            Server myServer = new Server();
            serverThread = new Thread(myServer.Run);
            serverThread.Start();

            Thread commandThread = new Thread(myServer.getCommand);
            commandThread.Start();

            // item thread start
            ItemHandler myItemHandler = new ItemHandler();
            Thread itemThread = new Thread(myItemHandler.Run);
            itemThread.Start();

            serverThread.Join();
            commandThread.Join();
        }

        public class ItemHandler
        {
            public void Run()
            {
                //lägg ut items i intervall

                Random rand = new Random();

                while (true)
                {
                    Thread.Sleep(rand.Next(1000, 2000));


                    if (Item.counter <= 10)
                    {
                        Item item = null;
                        int choice = rand.Next(1, 3);
                        switch (choice)
                        {
                            case 1:
                                item = new Sword();
                                break;
                            case 2:
                                item = new HealthPotion();
                                break;
                            default:
                                break;
                        }

                        game.GameBoard.AddItemToRoom(item);
                    }
                }

            }
        }


        public class Server
        {

            List<ClientHandler> clients = new List<ClientHandler>();
            public void Run()
            {
                listener = new TcpListener(IPAddress.Any, 5000);
                Console.WriteLine("Server up and running, waiting for messages...");
                bool isFull = false;
                int maxPlayers = 4;

                try
                {
                    listener.Start();

                    do
                    {
                        TcpClient c = listener.AcceptTcpClient();
                        ClientHandler newClient = new ClientHandler(c, this);
                        clients.Add(newClient);

                        Thread clientThread = new Thread(newClient.Run);
                        clientThread.Start();

                        if (clients.Count == maxPlayers)
                        {
                            isFull = true;
                        }
                    } while (!isFull);

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
                    if (tmpClient.tcpclient.Connected)
                    {
                        NetworkStream n = tmpClient.tcpclient.GetStream();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write(jsonToSend);
                        w.Flush();
                    }
                }
            }

            public void DisconnectClient(ClientHandler client)
            {
                clients.Remove(client);
                Console.WriteLine("Client X has left the building...");
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
                Player thisPlayer = null;

                try
                {
                    //string gameBoardString = "";
                    string message = "";
                    while (tcpclient.Connected)
                    {
                        NetworkStream n = tcpclient.GetStream();

                        if (tcpclient.Connected)
                        {
                            message = new BinaryReader(n).ReadString();
                            if (tcpclient.Connected)
                            {
                                var ap = JsonConvert.DeserializeObject<ActionProtocol>(message);

                                if (thisPlayer == null)
                                {
                                    thisPlayer = CreateNewPlayer(ap);
                                }

                                //fixa interactions
                                game.HandlePlayerMovement(ap.Action, thisPlayer);

                                //lever vi? om död,ta bort player från game och room + break
                                if (thisPlayer.IsDead)
                                {
                                    break;
                                }
                                string jsonToSend = game.GetProtocol();

                                myServer.Broadcast(this, jsonToSend);

                                Console.WriteLine(ap.Action);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    game.GameBoard.RemovePlayerFromRoom(thisPlayer);
                    game.Players.Remove(thisPlayer);
                    myServer.DisconnectClient(this);
                    tcpclient.Close();
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
