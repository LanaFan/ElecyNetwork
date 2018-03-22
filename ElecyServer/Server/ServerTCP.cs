using System;
using System.Net.Sockets;
using System.Net;
using Bindings;

namespace ElecyServer
{
   
    class ServerTCP
    {
        //Creating the main variables
        private static Socket _serverSocket;
        private static byte[] _buffer;
        private static bool _closed = true;

        //Server Setup
        public static void SetupServer()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _buffer = new byte[Constants.BUFFER_SIZE];
            _closed = false;

            for (int i = 0; i < Constants.MAX_CLIENTS; i++)
            {
                Global.clients[i] = new Client();
            }
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                Global.players[i] = new NetPlayer();
            }
            for(int i = 0; i < Constants.ARENA_SIZE; i++)
            {
                Global.arena[i] = new GameRoom(i);
            }

            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, Constants.PORT));
            _serverSocket.Listen(Constants.SERVER_LISTEN);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            //Console.WriteLine("Сервер запущен на Ip адресе {0} и порте {1}.", GetLocalIPAddress(), Constants.PORT);
            Global.serverForm.Debug("Сервер запущен на Ip адресе " + GetLocalIPAddress() + " и порте " + Constants.PORT + ".");
        }



        //Add new player in Players if there is a place
        public static int PlayerLogin(int index, string nickname, int[][]accountdata)
        {
            for(int i = 1; i < Constants.MAX_PLAYERS; i++)
            {
                if (Global.players[i].playerSocket == null)
                {
                    Global.players[i].playerSocket = Global.clients[index].socket;
                    Global.players[i].index = i;
                    Global.players[i].ip = Global.clients[index].ip;
                    Global.players[i].nickname = nickname;
                    for (int leveli = 0; leveli < Constants.RACES_NUMBER; leveli++)
                        Global.players[i].level[leveli] = accountdata[0][leveli];
                    for (int ranki = 0; ranki < Constants.RACES_NUMBER; ranki++)
                        Global.players[i].rank[ranki] = accountdata[1][ranki];
                    Global.serverForm.AddNetPlayer(Global.players[i]);
                    return i;
                }
            }
            return 0;
        }

        //Get server start point IP
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static void ServerClose()
        {
            if (!_closed)
            {
                _closed = true;
                Global.serverForm.Debug("Server closed...");
                for (int i = 0; i < Constants.MAX_CLIENTS; i++)
                {
                    if (Global.clients[i].socket != null)
                        Global.clients[i].CloseClient();
                    Global.clients[i] = null;
                }
                for (int i = 0; i < Constants.MAX_PLAYERS; i++)
                {
                    if (Global.players[i].playerSocket != null)
                        Global.players[i].ClosePlayer();
                    Global.players[i] = null;
                }
                for (int i = 0; i < Constants.ARENA_SIZE; i++)
                {
                    Global.arena[i] = null;
                }
                _serverSocket.Dispose();
            }
        }

       //Get the info about connetcion
        private static void AcceptCallback(IAsyncResult ar)
        {
            if (_closed)
                return;

            Socket socket = _serverSocket.EndAccept(ar);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            //Creating a copy of Client class for every clients 
            for(int i = 1; i < Constants.MAX_CLIENTS; i++)
            {
                if(Global.clients[i].socket == null)
                {
                    Global.clients[i].socket = socket;
                    Global.clients[i].index = i;
                    Global.clients[i].ip = socket.RemoteEndPoint.ToString();
                    Global.clients[i].StartClient();
                    Global.serverForm.AddClient(Global.clients[i].ip);
                    ServerSendData.SendClientConnetionOK(i);
                    return;
                }
            }
        }

        #region Send to Client

        //Send client connection
        public static void SendClientConnection(int index, byte[] data)
        {
            byte[] sizeinfo = new byte[4];
            sizeinfo[0] = (byte)data.Length;
            sizeinfo[1] = (byte)(data.Length >> 8);
            sizeinfo[2] = (byte)(data.Length >> 16);
            sizeinfo[3] = (byte)(data.Length >> 24);

            Global.clients[index].socket.Send(sizeinfo);
            Global.clients[index].socket.Send(data);
        }

        //Send data to single client
        public static void SendDataToClient(int index, byte[] data)
        {
            Global.clients[index].socket.Send(data);
        }

        //Send data to all clients(sender indeed)
        public static void SendDataToAllClient(byte[] data)
        {
            foreach(Client client in Global.clients)
            {
                if(client.socket != null)
                {
                    client.socket.Send(data);
                }
            }
        }

        #endregion

        #region Send to Player

        //Send data to single player
        public static void SendDataToPlayer(int index, byte[] data)
        {
            Global.players[index].playerSocket.Send(data);
        }

        //Send data to all players(sender indeed)
        public static void SendDataToAllPlayers(byte[] data)
        {
            foreach (NetPlayer player in Global.players)
            {
                if (player.playerSocket != null)
                {
                    player.playerSocket.Send(data);
                }
            }
        }

        #endregion

        #region Send to GamePlayer
        public static void SendDataToGamePlayer(int roomIndex, int ID, byte[] data)
        {
            Global.arena[roomIndex].GetSocket(ID).Send(data);
        }

        public static void SendDataToGamePlayers(int roomIndex, byte[] data)
        {
            Global.arena[roomIndex].GetP1Socket().Send(data);
            Global.arena[roomIndex].GetP2Socket().Send(data);
        }
        #endregion

        public static bool isClosed()
        {
            return _closed;
        }

    }

    //Client class for every client what connected
    public class Client
    {
        //Properties
        public int index;
        public string ip;
        public Socket socket = null;
        public bool closing;
        private byte[] _buffer = new byte[Constants.BUFFER_SIZE];

        //Starting client 
        public void StartClient()
        {
            closing = false;
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);

        }

        //Get the callback from client
        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!closing)
            {
                try
                {
                    int received = socket.EndReceive(ar);
                    if (received <= 0)
                    {
                        CloseClient();
                    }
                    else
                    {
                        byte[] dataBuffer = new byte[received];
                        Array.Copy(_buffer, dataBuffer, received);
                        ServerHandleNetworkData.HandleNetworkInformation(index, dataBuffer); 
                        if(!closing)
                        {
                            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                        }
                        else
                        {
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " " + ex.Source);
                    CloseClient();
                }
            }
            else
            {
                return;
            }
        }

        //Close client
        public void CloseClient()
        {
            closing = true;
            bool logged = false;
            foreach(NetPlayer player in Global.players)
            {
                if(player.ip == ip)
                {
                    logged = true;
                    player.StartPlayer();
                }
            }
            if(!logged)
                Console.WriteLine("Соединение от {0} было разорвано.", ip);
            Global.clients[index].socket = null;
        }
    }

    //Player class for every player what logged in
    public class NetPlayer
    {
        public int index;
        public int roomIndex;
        public string ip;
        public string nickname;
        public bool playerClosing = false;
        public int[] level = new int[Constants.RACES_NUMBER];
        public int[] rank = new int[Constants.RACES_NUMBER];
        public byte[] _buffer = new byte[Constants.BUFFER_SIZE];
        public playerState state;
        public Socket playerSocket = null;

        public enum playerState
        {
            InMainLobby = 1,
            SearchingForMatch = 2,
            Playing = 3,
            EndPlaying = 4
        }

        public void StartPlayer()
        { 
            state = playerState.InMainLobby;
            playerSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallback), playerSocket);
            playerClosing = false;
        }

        public void PlayerReceiveCallback(IAsyncResult ar)
        {
            if (!playerClosing)
            {
                try
                {
                    int received = playerSocket.EndReceive(ar);

                    if (received <= 0)
                    {
                        ClosePlayer();
                    }
                    else
                    {
                        byte[] dataBuffer = new byte[received];
                        Array.Copy(_buffer, dataBuffer, received);
                        ServerHandleNetworkData.HandleNetworkInformation(index, dataBuffer);
                        if (!playerClosing)
                            playerSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallback), playerSocket);
                        else
                            return;
                    }
                }
                catch
                {
                    ClosePlayer();
                }
            }
        }

        public void NetPlayerStop()
        {
            playerClosing = true;
        }

        public void ClosePlayer()
        {
            playerClosing = true;
            Global.players[index].playerSocket = null;
        }
    }


}
