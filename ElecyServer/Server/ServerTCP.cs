using System;
using System.Net.Sockets;
using System.Net;
using Bindings;

namespace ElecyServer
{

    class ServerTCP
    {
        public static string ServerIP { get; private set; }
        public static bool Closed { get; private set; } = true;

        private static Socket _serverSocket;
        private static byte[] _buffer;

        public static void SetupServer()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _buffer = new byte[Constants.BUFFER_SIZE];
            ServerIP = GetLocalIPAddress();
            Closed = false;

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
            Global.serverForm.Debug("Сервер запущен на Ip адресе " + ServerIP + " и порте " + Constants.PORT + ".");
        }

        public static int PlayerLogin(int index, string nickname, int[][]accountdata)
        {
            for(int i = 1; i < Constants.MAX_PLAYERS; i++)
            { 
                if (Global.players[i].Socket == null)
                {
                    Global.players[i].SetVar(Global.clients[index].Socket, i, Global.clients[index].IP, nickname, accountdata[0], accountdata[1]);
                    Global.serverForm.AddNetPlayer(Global.players[i]);
                    return i;
                }
            }
            return 0;
        }

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
            if (!Closed)
            {
                Closed = true;
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

        private static void AcceptCallback(IAsyncResult ar)
        {
            if (!Closed)
            {
                Socket socket = _serverSocket.EndAccept(ar);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

                for (int i = 1; i < Constants.MAX_CLIENTS; i++)
                {
                    if (Global.clients[i].Socket == null)
                    {
                        Global.clients[i].SetVar(socket, socket.RemoteEndPoint.ToString(), i);
                        Global.clients[i].StartClient();
                        Global.serverForm.AddClient(Global.clients[i].IP);
                        ServerSendData.SendClientConnetionOK(i);
                        return;
                    }
                }
            }
        }

        #region Send to Client

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

        public static void SendDataToPlayer(int index, byte[] data)
        {
            try
            {
                Global.players[index].Socket.Send(data);
            }
            catch
            {
                Global.players[index].ClosePlayer();
            }
        }

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

    public class Client
    {
        public Socket Socket { get; private set; }
        public int Index { get; private set; }
        public string IP { get; private set; }

        private bool _closing;
        private byte[] _buffer;

        public Client()
        {
            _buffer = new byte[Constants.BUFFER_SIZE];
        }

        public void SetVar(Socket socket, string ip, int index)
        {
            Socket = socket;
            IP = ip;
            Index = index;
        }

        public void StartClient()
        {
            _closing = false;
            Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), Socket);
        }

        public void CloseClient()
        {
            _closing = true;
            Global.serverForm.Debug("Соединение от " + IP + " было разорвано.");
            Global.serverForm.RemoveClient(IP);
            Socket = null;
        }

        public void LogInClient(int pIndex)
        {
            _closing = true;
            Global.serverForm.RemoveClient(IP);
            Global.players[pIndex].StartPlayer();
            Socket = null;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!_closing)
            {
                try
                {
                    int received = Socket.EndReceive(ar);
                    if (received <= 0)
                    {
                        CloseClient();
                    }
                    else
                    {
                        byte[] dataBuffer = new byte[received];
                        Array.Copy(_buffer, dataBuffer, received);
                        ServerHandleClientData.HandleNetworkInformation(Index, dataBuffer); 
                        if(!_closing)
                            Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), Socket);
                    }

                }
                catch (Exception ex)
                {
                    Global.serverForm.Debug(ex.Message + " " + ex.Source);
                    CloseClient();
                }
            }
        }
    }

    public class NetPlayer
    {
        public int[] levels; // private set!
        public int[] ranks; //  private set!

        public int RoomIndex { get; private set; }
        public PlayerState State { get; private set; } 
        public int Index { get; private set; }
        public Socket Socket { get; private set; }
        public string IP { get; private set; }
        public string Nickname { get; private set; }
        public bool Stopped { get; private set; }

        private byte[] _buffer;

        public enum PlayerState
        {
            InMainLobby = 1,
            SearchingForMatch = 2,
            Playing = 3,
            EndPlaying = 4
        }

        public NetPlayer()
        {
            _buffer = new byte[Constants.BUFFER_SIZE];
            RoomIndex = 0;
        }

        public void SetVar(Socket socket, int index, string ip, string nickname, int[] levels, int[] ranks)
        {
            Socket = socket;
            Index = index;
            IP = ip;
            Nickname = nickname;
            this.levels = levels;
            this.ranks = ranks;
        }

        public void StartPlayer()
        { 
            Stopped = false;
            State = PlayerState.InMainLobby;
            Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallback), Socket);
        }
        #region States

        public void Searching(int roomIndex)
        {
            State = PlayerState.SearchingForMatch;
            RoomIndex = roomIndex;
        }

        public void Playing()
        {
            State = PlayerState.Playing;
        }

        public void EndPlaying(/* lvl, rating (to change) */)
        {
            State = PlayerState.EndPlaying;
        }

        public void InMainLobby()
        {
            State = PlayerState.InMainLobby;
            RoomIndex = 0;
        }

        #endregion
        public void NetPlayerStop()
        {
            Stopped = true;
        }

        public void ClosePlayer()
        {
            ServerSendData.SendGlChatMsg("Server", "Player " + Nickname + " disconnected");
            Global.serverForm.RemoveNetPlayer(Nickname);
            Stopped = true;
            Socket = null;
        }

        private void PlayerReceiveCallback(IAsyncResult ar)
        {
            if (!Stopped)
            {
                try
                {
                    int received = Socket.EndReceive(ar);

                    if (received <= 0)
                    {
                        ClosePlayer();
                    }
                    else
                    {
                        byte[] dataBuffer = new byte[received];
                        Array.Copy(_buffer, dataBuffer, received);
                        ServerHandlePlayerData.HandleNetworkInformation(Index, dataBuffer);
                        if (!Stopped)
                            Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallback), Socket);
                    }
                }
                catch(Exception ex)
                {
                    Global.serverForm.Debug(ex.Message + " " + ex.Source);
                    ClosePlayer();
                }
            }
        }

    }

}
