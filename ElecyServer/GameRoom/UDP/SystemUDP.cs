using System;
using System.Net;
using System.Net.Sockets;
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
                Global.serverForm.Debug("UDP система запущена и ожидает подключений.");
                receive = true;
                connectUDP.BeginReceive(new AsyncCallback(ReceiveCallback), connectUDP);
            }
            catch (SocketException ex)
            {
                Global.serverForm.Debug(ex.ErrorCode + "");
            }

        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            if (receive)
            {
                try
                {
                    IPEndPoint ipEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] connectBuffer = connectUDP.EndReceive(ar, ref ipEndpoint);
                    HandleDataUDP.HandleNetworkInformation(CheckIP(ipEndpoint), connectBuffer);
                }
                catch (SocketException e)
                {
                    Global.serverForm.Debug(e.ErrorCode + "");
                }
                if(receive)
                    connectUDP.BeginReceive(ReceiveCallback, connectUDP);
            }
        }

            private static GamePlayerUDP CheckIP(IPEndPoint ipEndPoint)
        {
            foreach(GamePlayerUDP player in Global.playersUDP)
            {
                if (player.ip == ipEndPoint)
                {
                    return player;
                }
            }
            GamePlayerUDP playerUDP = new GamePlayerUDP(ipEndPoint);
            Global.playersUDP.Add(playerUDP);
            return playerUDP;
        }

        public static void Close()
        {
            receive = false;
            try
            {
                connectUDP.Close();
            }
            catch { }
        }

        public static void SendConnect(GamePlayerUDP player, byte[] data)
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

        public GamePlayerUDP(IPEndPoint ipEndpoint)
        {
            ip = ipEndpoint;
        }

        public void SetValues(int id, GameRoom gameRoom)
        {
            ID = id;
            room = gameRoom;
            SendDataUDP.SendConnectionOK(this);
        }
        
    }
}
