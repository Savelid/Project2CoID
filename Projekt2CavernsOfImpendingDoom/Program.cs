using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projekt2CavernsOfImpendingDoom
{
    class Program
    {
        static Game game;
        public static Thread serverThread;
        public static TcpListener listener;
        static void Main(string[] args)
        {
            game = new Game(5,3);

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

            public void Broadcast(ClientHandler client, string message)
            {
                foreach (ClientHandler tmpClient in clients)
                {
                        NetworkStream n = tmpClient.tcpclient.GetStream();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write(message);
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
                var newPlayer = new Player("Kalle Kungen XVI");
                newPlayer.Location = new Location(1, 1);
                game.Players.Add(newPlayer);
                game.GameBoard.AddPlayer(newPlayer);

            }

            public void Run()
            {
                try
                {
                    string gameBoardString = "";
                    string message = "";
                    while (!message.Equals("quit"))
                    {
                        NetworkStream n = tcpclient.GetStream();
                        message = new BinaryReader(n).ReadString();

                        game.HandlePlayerMovement(message);

                        gameBoardString = game.GameBoard.GetGameBoardString();
                        myServer.Broadcast(this, gameBoardString);

                        Console.WriteLine(message);
                    }

                    myServer.DisconnectClient(this);
                    tcpclient.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
