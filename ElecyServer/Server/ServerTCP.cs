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

        public static bool Closed
        {
            get { return _closed; }
        }

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
            Global.serverForm.Debug("Сервер запущен на Ip адресе " + GetLocalIPAddress() + " и порте " + Constants.PORT + ".");
        }

        //Add new player in Players if there is a place
        public static int PlayerLogin(int index, string nickname, int[][]accountdata)
        {
            for(int i = 1; i < Constants.MAX_PLAYERS; i++)
            {
                if (Global.players[i].Socket == null)
                {
                    Global.players[i].SetVar(Global.clients[index].Socket, i, Global.clients[index].IP, nickname, accountdata[0], accountdata[1]);
                    Global.clients[index].Logged = true;
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
                for (int i = 0; i < Constants.MAX_CLIENTS; i++)
                {
                    if (Global.clients[i].Socket != null)
                        Global.clients[i].CloseClient();
                    Global.clients[i] = null;
                }
                for (int i = 0; i < Constants.MAX_PLAYERS; i++)
                {
                    if (Global.players[i].Socket != null)
                        Global.players[i].ClosePlayer();
                    Global.players[i] = null;
                }
                for (int i = 0; i < Constants.ARENA_SIZE; i++)
                {
                    Global.arena[i] = null;
                }
                _serverSocket.Dispose();
                Global.serverForm.Debug("Server closed...");
            }
        }

        public static int AddClient(Socket socket)
        {
            for (int i = 1; i < Constants.MAX_CLIENTS; i++)
            {
                if (Global.clients[i].Socket == null)
                {
                    Global.clients[i].SetVar(socket, socket.RemoteEndPoint.ToString(), i);
                    return i;
                }
            }
            return 0;
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
                if(Global.clients[i].Socket == null)
                {
                    Global.clients[i].SetVar(socket, socket.RemoteEndPoint.ToString(), i);
                    Global.clients[i].StartClient();
                    Global.serverForm.AddClient(Global.clients[i].IP);
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

            try
            {
                Global.clients[index].Socket.Send(sizeinfo);
                Global.clients[index].Socket.Send(data);
            }
            catch
            {
                Global.clients[index].CloseClient();
            }
        }

        //Send data to single client
        public static void SendDataToClient(int index, byte[] data)
        {
            try
            {
                Global.clients[index].Socket.Send(data);
            }
            catch
            {
                Global.clients[index].CloseClient();
            }
        }

        //Send data to all clients(sender indeed)
        public static void SendDataToAllClient(byte[] data)
        {
            foreach(Client client in Global.clients)
            {
                if(client.Socket != null)
                {
                    try
                    {
                        client.Socket.Send(data);
                    }
                    catch
                    {
                        client.CloseClient();
                    }
                }
            }
        }

        #endregion

        #region Send to Player

        //Send data to single player
        public static void SendDataToPlayer(int index, byte[] data)
        {
            try
            {
                Global.players[index].Socket.Send(data);
            }
            catch
            {
                ServerSendData.SendGlChatMsg("Server", "Player " + Global.players[index].Nickname + " disconnected");
                Global.players[index].ClosePlayer();
            }
        }

        //Send data to all players(sender indeed)
        public static void SendDataToAllPlayers(byte[] data)
        {
            foreach (NetPlayer player in Global.players)
            {
                if (player.Socket != null)
                {
                    try
                    {
                        player.Socket.Send(data);
                    }
                    catch
                    {
                        ServerSendData.SendGlChatMsg("Server", "Player " + player.Nickname + " disconnected");
                        player.ClosePlayer();
                    }
                }
            }
        }

        #endregion

        #region Send to GamePlayer
        public static void SendDataToGamePlayer(int roomIndex, int ID, byte[] data)
        {
            try
            {
                Global.arena[roomIndex].GetSocket(ID).Send(data);
            }
            catch
            {
                Global.arena[roomIndex].AbortGameSession(ID);
            }
        }

        public static void SendDataToGamePlayers(int roomIndex, byte[] data)
        {
            try
            {
                Global.arena[roomIndex].Player1Socket.Send(data);
            }
            catch
            {
                Global.arena[roomIndex].AbortGameSession(1);
                return;
            }
            try
            {
                Global.arena[roomIndex].Player2Socket.Send(data);
            }
            catch
            {
                Global.arena[roomIndex].AbortGameSession(2);
            }


        }
        #endregion

    }

    //Client class for every client what connected
    public class Client
    {
        //Properties
        private int _index;
        private string _ip;
        private Socket _socket;
        private bool _closing;
        private byte[] _buffer;
        private bool _logged;

        #region Public Var

        public bool Logged
        {
            set { _logged = value; }
        }
        public Socket Socket
        {
            get { return _socket; }
        }
        public string IP
        {
            get { return _ip; }
        }
        public int Index
        {
            get { return _index; }
        }

        #endregion

        public Client()
        {
            _logged = false;
            _buffer = new byte[Constants.BUFFER_SIZE];
        }

        public void SetVar(Socket socket, string ip, int index)
        {
            _socket = socket;
            _ip = ip;
            _index = index;
        }

        //Starting client 
        public void StartClient()
        {
            _closing = false;
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _socket);
        }

        //Get the callback from client
        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!_closing)
            {
                try
                {
                    int received = _socket.EndReceive(ar);
                    if (received <= 0)
                    {
                        CloseClient();
                    }
                    else
                    {
                        byte[] dataBuffer = new byte[received];
                        Array.Copy(_buffer, dataBuffer, received);
                        ServerHandleClientData.HandleNetworkInformation(_index, dataBuffer); 
                        if(!_closing)
                        {
                            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _socket);
                        }
                        else
                        {
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Global.serverForm.Debug(ex.Message + " " + ex.Source);
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
            _closing = true;
            if(!_logged)
                Console.WriteLine("Соединение от {0} было разорвано.", _ip);
            _socket = null;
        }

    }

    //Player class for every player what logged in
    public class NetPlayer
    {
        public playerState state;
        public int[] levels;
        public int[] ranks;
        public int roomIndex;

        private byte[] _buffer;
        private int _index;
        private Socket _socket;
        private string _ip;
        private string _nickname;
        private bool _playerStopped;

        #region Public Var

        public Socket Socket
        {
            get { return _socket; }
        }
        public int Index
        {
            get { return _index; }
        }
        public string IP
        {
            get { return _ip; }
        }
        public string Nickname
        {
            get { return _nickname; }
        }
        public bool Stopped
        {
            get { return _playerStopped; }
        }

        #endregion

        public enum playerState
        {
            InMainLobby = 1,
            SearchingForMatch = 2,
            Playing = 3,
            EndPlaying = 4
        }

        public NetPlayer()
        {
            _buffer = new byte[Constants.BUFFER_SIZE];
        }

        public void SetVar(Socket socket, int index, string ip, string nickname, int[] levels, int[] ranks)
        {
            _socket = socket;
            _index = index;
            _ip = ip;
            _nickname = nickname;
            this.levels = levels;
            this.ranks = ranks;
        }

        public void StartPlayer()
        { 
            _playerStopped = false;
            state = playerState.InMainLobby;
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallback), _socket);
        }

        public void PlayerReceiveCallback(IAsyncResult ar)
        {
            if (!_playerStopped)
            {
                try
                {
                    int received = _socket.EndReceive(ar);

                    if (received <= 0)
                    {
                        ClosePlayer();
                    }
                    else
                    {
                        byte[] dataBuffer = new byte[received];
                        Array.Copy(_buffer, dataBuffer, received);
                        ServerHandlePlayerData.HandleNetworkInformation(_index, dataBuffer);
                        if (!_playerStopped)
                            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallback), _socket);
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
            _playerStopped = true;
        }

        public void ClosePlayer()
        {
            _playerStopped = true;
            _socket = null;
        }
    }


}
