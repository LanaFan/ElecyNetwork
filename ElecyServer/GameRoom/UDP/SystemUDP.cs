using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Bindings;

namespace ElecyServer
{
    public static class UDPConnector
    {
        private static UdpClient connectUDP;
        private static byte[] buffer;
        private static bool receive;

        public static void WaitConnect()
        {
            HandleDataUDP.InitializeNetworkPackages();
            try
            {
                connectUDP = new UdpClient(Constants.UDP_PORT);
                buffer = new byte[Constants.UDP_BUFFER_SIZE];
                receive = true;
                connectUDP.BeginReceive(new AsyncCallback(ReceiveCallback), connectUDP);
                Global.serverForm.StatusIndicator(3);
            }
            catch (SocketException ex)
            {
                Global.serverForm.StatusIndicator(3, ex);
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            if (receive)
            {
                try
                {
                    UdpClient _client = (UdpClient)ar.AsyncState;
                    IPEndPoint ipEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] connectBuffer = _client.EndReceive(ar, ref ipEndpoint);
                    HandleDataUDP.HandleNetworkInformation(CheckIP(ipEndpoint), connectBuffer);
                    if (receive)
                        _client.BeginReceive(ReceiveCallback, _client);
                }
                catch (SocketException e)
                {
                    Global.serverForm.Debug(e.ErrorCode + "");
                }
            }
        }

        private static GamePlayerUDP CheckIP(IPEndPoint ipEndPoint)
        {
            foreach (GamePlayerUDP player in Global.playersUDP)
            {
                if (player.ip.Equals(ipEndPoint))
                {
                    return player;
                }
            }
            foreach (GameRoom room in Global.roomsList)
            {
                if (room.Status == RoomState.Loading)
                {
                    if (room.player1.ip.Equals(ipEndPoint.Address))
                    {
                        Global.playersUDP.Add(room.playerUDP1 = new GamePlayerUDP(ipEndPoint, 0, room));
                        if (room.playerUDP2 != null)
                            Global.roomsUDP.Add(room);
                        return room.playerUDP1;
                    }
                    else if (room.player2.ip.Equals(ipEndPoint.Address))
                    {
                        Global.playersUDP.Add(room.playerUDP2 = new GamePlayerUDP(ipEndPoint, 1, room));
                        if (room.playerUDP1 != null)
                            Global.roomsUDP.Add(room);
                        return room.playerUDP2;
                    }

                }
            }
            return null;
        }

        public static void Close()
        {
            receive = false;
            try
            {
                connectUDP.Close();
                Global.serverForm.HidePtr(3);
            }
            catch (Exception ex)
            {
                Global.serverForm.StatusIndicator(3, ex);
            }
        }

        public static void Send(GamePlayerUDP player, byte[] data)
        {
            try
            {
                connectUDP.Send(data, data.Length, player.ip);
            }
            catch
            {
                //Close
            }
        }
    }

    public class GamePlayerUDP
    {
        public readonly IPEndPoint ip;
        public int ID { get; private set; }
        public GameRoom room { get; private set; }

        public GamePlayerUDP(IPEndPoint ipEndpoint, int id, GameRoom gameRoom)
        {
            ip = ipEndpoint;
            ID = id;
            room = gameRoom;
            SendDataUDP.SendConnectionOK(this);
        }

        public void SetValues(int id, GameRoom gameRoom)
        {
            ID = id;
            room = gameRoom;
            SendDataUDP.SendConnectionOK(this);
        }
        
    }
}
