using System;
using System.Net.Sockets;
using System.Net;
using Bindings;

namespace ElecyServer
{

    class ServerTCP
    {
        public static bool Closed { get; private set; } = true;

        private static Socket _serverSocket;
        private static byte[] _buffer;

        public static void SetupServer()
        {
            try
            {
                _buffer = new byte[Constants.TCP_BUFFER_SIZE];
                Closed = false;
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, Constants.PORT));
                _serverSocket.Listen(Constants.SERVER_LISTEN);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                UDPConnector.WaitConnect();
                Global.serverForm.StatusIndicator(4);
            }
            catch (Exception ex)
            {
                Global.serverForm.StatusIndicator(4, ex);
            }
        }

        public static void ServerClose()
        {
            if (!Closed)
            {
                Closed = true;
                try
                {
                    Global.ThreadsStop();
                    Global.FinalGlobals();
                    Global.serverForm.HidePtr(1);
                }
                catch (Exception ex)
                {
                    Global.serverForm.StatusIndicator(1, ex);
                }
                UDPConnector.Close();
                try
                {
                    _serverSocket.Dispose();
                    Global.serverForm.HidePtr(4);
                }
                catch(Exception ex)
                {
                    Global.serverForm.StatusIndicator(4, ex);
                }

            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            if (!Closed)
            {
                Socket socket = _serverSocket.EndAccept(ar);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                ClientTCP client = new ClientTCP(socket, socket.RemoteEndPoint as IPEndPoint);
                Global.clientList.Add(client);
                SendDataTCP.SendClientConnetionOK(client);
            }
        }

        #region SendData

        public static void SendClientConnection(ClientTCP client, byte[] data)
        {
            byte[] sizeinfo = new byte[4];
            sizeinfo[0] = (byte)data.Length;
            sizeinfo[1] = (byte)(data.Length >> 8);
            sizeinfo[2] = (byte)(data.Length >> 16);
            sizeinfo[3] = (byte)(data.Length >> 24);

            try
            {
                client.socket.Send(sizeinfo);
                client.socket.Send(data);
            }
            catch
            {
                client.Close();
            }
        }

        public static void SendDataToClient(ClientTCP client, byte[] data)
        {
            try
            {
                client.socket.Send(data);

            }
            catch
            {
                client.Close();
            }
        }

        public static void SendDataToAllClients(byte[] data)
        {
            try
            {
                foreach(ClientTCP client in Global.clientList)
                {
                    try
                    {
                        client.socket.Send(data);
                    }
                    catch
                    {
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.serverForm.Debug(ex + "");
            }
        }

        public static void SendChatMsgToAllClients(byte[] data)
        {
            try
            {
                foreach(ClientTCP client in Global.clientList)
                {
                    try
                    {
                        if (client.clientState == ClientTCPState.MainLobby)
                            client.socket.Send(data);
                    }
                    catch
                    {
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.serverForm.Debug(ex + "");
            }
        }

        public static void SendDataToRoomPlayers(BaseGameRoom room, byte[] data)
        {
            try
            {
                for(int i = 0; i < room.PlayersCount; i++)
                {
                    try
                    {
                        if (room.playersTCP[i] != null)
                            room.playersTCP[i].socket.Send(data);
                    }
                    catch (Exception ex)
                    {
                        room.playersTCP[i].Close();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.serverForm.Debug(ex + "");
            }
        }

        public static void SendDataToRoomPlayers(BaseGameRoom room, ClientTCP exceptClient, byte[] data)
        {
            try
            {
                for (int i = 0; i < room.PlayersCount; i++)
                {
                    try
                    {
                        if (room.playersTCP[i] != null && !exceptClient.Equals(room.playersTCP[i]))
                            room.playersTCP[i].socket.Send(data);
                    }
                    catch (Exception ex)
                    {
                        room.playersTCP[i].Close();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.serverForm.Debug(ex + "");
            }
        }

        #endregion
    }

    public class ClientTCP
    {

        #region Variables

        public readonly Socket socket;
        public readonly IPEndPoint ip;

        public ClientTCPState clientState;
        public NetPlayerState playerState;

        public string nickname;
        public int[] levels;
        public int[] ranks;

        public BaseGameRoom room;
        public GamePlayerUDP playerUDP;
        public int ID;
        public string race;
        public float load;

        private byte[] _buffer;

        #endregion

        #region Constructor

        public ClientTCP(Socket socket, IPEndPoint ip)
        {
            this.socket = socket;
            this.ip = ip;
            _buffer = new byte[Constants.TCP_BUFFER_SIZE];
            clientState = ClientTCPState.Entrance;
            Receive();
        }

        #endregion

        #region Receive

        public void Receive()
        {
            if(clientState != ClientTCPState.Sleep)
            {
                if (clientState == ClientTCPState.Entrance)
                    Global.serverForm.AddClient(this);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if(clientState != ClientTCPState.Sleep)
            {
                try
                {
                    int received = socket.EndReceive(ar);
                    if (received <= 0)
                    {
                        Close();
                    }
                    else
                    {
                        if (clientState != ClientTCPState.Sleep)
                            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
                        byte[] dataBuffer = new byte[received];
                        Array.Copy(_buffer, dataBuffer, received);
                        HandleDataTCP.HandleNetworkInformation(this, dataBuffer);
                    }
                }
                catch (Exception ex)
                {
                    Global.serverForm.Debug(ex.Message + " " + ex.Source);
                    Close();
                }
            }
        }

        #endregion

        #region Transitions

        public void LogIn(string username)
        {
            nickname = Global.data.GetAccountNickname(username);
            int[][] data = Global.data.GetAccountData(nickname);
            levels = data[0];
            ranks = data[1];
            playerState = NetPlayerState.InMainLobby;
            clientState = ClientTCPState.MainLobby;
            SendDataTCP.SendLoginOk(nickname, data, this);
        }

        public void EnterRoom(BaseGameRoom room, GamePlayerUDP playerUDP, int ID)
        {
            this.room = room;
            this.playerUDP = playerUDP;
            this.ID = ID;
            Global.unconectedPlayersUDP.Add(playerUDP);
        }

        public void LeaveRoom()
        {
            playerState = NetPlayerState.InMainLobby;
            clientState = ClientTCPState.MainLobby;
            race = null;
            room = null;
        }

        public void Close()
        {
            if (clientState != ClientTCPState.Sleep)
            {
                if (clientState == ClientTCPState.Entrance)
                {
                    clientState = ClientTCPState.Sleep;
                    try
                    {
                        socket.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Global.serverForm.Debug(ex + "");
                    }
                    Global.clientList.Remove(this);
                }
                else if (clientState == ClientTCPState.MainLobby)
                {
                    clientState = ClientTCPState.Sleep;
                    try
                    {
                        socket.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Global.serverForm.Debug(ex + "");
                    }
                    Global.clientList.Remove(this);
                    SendDataTCP.SendGlChatMsg("Server", $"Player { nickname } disconnected.");
                }
                else if (clientState == ClientTCPState.GameRoom)
                {
                    clientState = ClientTCPState.Sleep;
                    if (playerState == NetPlayerState.SearchingForMatch)
                    {
                        room.DeletePlayer(this);
                    }
                    else if (playerState == NetPlayerState.Playing)
                    {
                        room.AbortGameSession(this);
                    }
                    else if (playerState == NetPlayerState.EndPlaying)
                    {
                        room.DeletePlayer(this);
                    }
                    Global.serverForm.Debug($"GamePlayer {nickname} lost connection");
                    Global.clientList.Remove(this);
                }
                Global.serverForm.RemoveClient(this);
            }
        }

        #endregion

    }

}
