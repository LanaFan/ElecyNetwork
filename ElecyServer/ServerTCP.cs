using System;
using System.Net.Sockets;
using System.Net;
using Bindings;

namespace ElecyServer
{
    //Server startup
    class ServerTCP
    {
        //Creating the main variables
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] _buffer = new byte[1024];

        //Creating clients array
        public static Client[] _clients = new Client[Constants.MAX_PLAYERS];
        public static Player[] _players = new Player[Constants.MAX_PLAYERS];

        //Server Setup
        public static void SetupServer()
        {

            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                _clients[i] = new Client();
                _players[i] = new Player();
            }
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 24985));
            _serverSocket.Listen(10);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            Console.WriteLine("Сервер запущен на Ip адресе {0} и порте {1}.", GetLocalIPAddress(), 24985);            
        }

        //Get the info about connetcion
        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            //Creating a copy of Client class for every clients 
            for(int i = 1; i < Constants.MAX_PLAYERS; i++)
            {
                if(_clients[i].socket == null)
                {
                    _clients[i].socket = socket;
                    _clients[i].index = i;
                    _clients[i].ip = socket.RemoteEndPoint.ToString();
                    _clients[i].StartClient();
                    Console.WriteLine("Соединение с {0} установлено. Клиент находится под индексом {1}", _clients[i].ip, i);
                    ServerSendData.SendConnetionOK(i);
                    return;
                }
            }
        }

        public static void PlayerLogin(int index, string nickname, int[][]accountdata)
        {
            for(int i = 1; i < Constants.MAX_PLAYERS; i++)
            {
                if (_players[i].playerSocket == null)
                {
                    _players[i].playerSocket = _clients[index].socket;
                    _players[i].index = i;
                    _players[i].ip = _clients[index].ip;
                    _players[i].nickname = nickname;
                    for (int leveli = 0; leveli < 5; leveli++)
                        _players[i].level[leveli] = accountdata[0][leveli];
                    for (int ranki = 0; ranki < 5; ranki++)
                        _players[i].Rank[ranki] = accountdata[1][ranki];
                    return;
                }
            }
        }
        //Send connection
        public static void SendDataTo(int index, byte[] data)
        {
            byte[] sizeinfo = new byte[4];
            sizeinfo[0] = (byte)data.Length;
            sizeinfo[1] = (byte)(data.Length >> 8);
            sizeinfo[2] = (byte)(data.Length >> 16);
            sizeinfo[3] = (byte)(data.Length >> 24);

            _clients[index].socket.Send(sizeinfo);
            _clients[index].socket.Send(data);
        }
        //Send data to single client
        public static void SendData(int index, byte[] data)
        {
            _clients[index].socket.Send(data);
        }

        public static void SendDataToAllClient(byte[] data)
        {
            foreach(Client client in _clients)
            {
                if(client.socket != null)
                {
                    client.socket.Send(data);
                }
            }
        }

        public static void SendDataToAllPlayers(byte[] data)
        {
            foreach (Player player in _players)
            {
                if (player.playerSocket != null)
                {
                    player.playerSocket.Send(data);
                }
            }
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
    }
    //Client class for every client what connected
    public class Client
    {
        //Properties
        public int index;
        public string ip;
        public Socket socket = null;
        public bool closing = false;
        private byte[] _buffer = new byte[1024];

        //Starting client 
        public void StartClient()
        {
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            closing = false;
        }

        //Get the callback from client
        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!closing)
            {
                socket = (Socket)ar.AsyncState;

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
                        PacketBuffer buffer = new PacketBuffer();
                        buffer.WriteBytes(dataBuffer);
                        int packetnum = buffer.ReadInteger();
                        ServerHandleNetworkData.HandleNetworkInformation(index, dataBuffer); 
                        if (packetnum != 5)
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
                    Console.WriteLine(ex);
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
            foreach(Player player in ServerTCP._players)
            {
                if(player.ip == ip)
                {
                    logged = true;
                    player.StartPlayer();
                }
            }
            if(!logged)
                Console.WriteLine("Соединение от {0} было разорвано.", ip);
            socket.Close();
            ServerTCP._clients[index].socket = null;
        }
    }
    //Player class for every player what logged in
    public class Player
    {
        public int index;
        public string ip;
        public string nickname;
        public bool playerClosing = false;
        public int[] level = new int[5];
        public int[] Rank = new int[5];
        public byte[] _buffer = new byte[1024];
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
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                int received = socket.EndReceive(ar);

                if (received <= 0)
                {
                    ClosePlayer(index);
                }
                else
                {
                    byte[] dataBuffer = new byte[received];
                    Array.Copy(_buffer, dataBuffer, received);
                    ServerHandleNetworkData.HandleNetworkInformation(index, dataBuffer);
                    playerSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallback), playerSocket);
                }
            }
            catch
            {
                ClosePlayer(index);
            }
        }

        private void ClosePlayer(int index)
        {
            playerClosing = true;
            //Console.WriteLine("Соединение от {0} было разорвано.", ip);
            //PlayerLeft();
            playerSocket.Close();
        }
    }
}
