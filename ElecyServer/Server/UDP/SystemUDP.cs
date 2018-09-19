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
                Global.serverForm.StatusIndicator(2);
            }
            catch (SocketException ex)
            {
                Global.serverForm.StatusIndicator(2, ex);
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
                        _client.BeginReceive(new AsyncCallback(ReceiveCallback), _client);
                }
                catch (SocketException e)
                {
                    //try
                    //{
                    //    connectUDP.BeginReceive(new AsyncCallback(ReceiveCallback), connectUDP);
                    //    Global.serverForm.Debug("Flawless victory");
                    //}
                    //catch
                    //{
                        Global.serverForm.Debug(e + "");
                    //}


                }
            }
        }

        private static GamePlayerUDP CheckIP(IPEndPoint ipEndPoint)
        {
            foreach (GamePlayerUDP player in Global.connectedPlayersUDP)
            {
                if (player.ip.Equals(ipEndPoint))
                {
                    return player;
                }
            }
            foreach(GamePlayerUDP player in Global.unconectedPlayersUDP)
            {
                if(player.ip.Address.Equals(ipEndPoint.Address))
                {
                    player.ip = ipEndPoint;
                    return player;
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
                Global.serverForm.HidePtr(2);
            }
            catch (Exception ex)
            {
                Global.serverForm.StatusIndicator(2, ex);
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

        public static void SendToRoomPlayers(BaseGameRoom room, byte[] data)
        {
            try
            {
                for (int i = 0; i < room.PlayersCount; i++)
                {
                    try
                    {
                        if (room.playersUDP[i] != null)
                            connectUDP.Send(data, data.Length, room.playersUDP[i].ip);
                    }
                    catch (Exception ex)
                    {
                        //room.playersUDP[i].Close();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.serverForm.Debug(ex + "");
            }
        }
    }

    public class GamePlayerUDP
    {
        public IPEndPoint ip;
        public int ID { get; private set; }
        public BaseGameRoom room { get; private set; }

        public GamePlayerUDP(IPEndPoint ipEndpoint, int id, BaseGameRoom gameRoom)
        {
            ip = ipEndpoint;
            ID = id;
            room = gameRoom;
        }

        public void Connected()
        {
            room.UDPConnected(ID);
            SendDataUDP.SendConnectionOK(this);
            Global.unconectedPlayersUDP.Remove(this);
            Global.connectedPlayersUDP.Add(this);
        }
        
    }
}
