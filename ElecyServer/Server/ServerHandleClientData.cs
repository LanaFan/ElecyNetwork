using Bindings;
using System;
using System.Collections.Generic;

namespace ElecyServer
{
    class ServerHandleClientData
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Packets = new Dictionary<int, Packet_>
            {
                {(int)ClientPackets.CConnectComplite, HandleClientConnect },
                {(int)ClientPackets.CRegisterTry, HandleRegisterTry },
                {(int)ClientPackets.CLoginTry, HandleLoginTry },
                {(int)ClientPackets.CAlert, HandleAlert },
                {(int)ClientPackets.CReconnectComplite, HandleClientReconnect },
                {(int)ClientPackets.CClose, HandleClientClose },
                {(int)SystemPackets.SysExit, HandleClientExit},
            };
        }

        public static void HandleNetworkInformation(int index, byte[] data)
        {
            int packetnum;
            PacketBuffer buffer = new PacketBuffer();
            Packet_ Packet;
            buffer.WriteBytes(data);
            packetnum = buffer.ReadInteger();
            buffer.Dispose();
            if (Packets.TryGetValue(packetnum, out Packet))
            {
                Packet.Invoke(index, data);
            }
        }

        private static void HandleClientConnect(int index, byte[] data)
        {
            Console.WriteLine("Соединение с {0} установлено. Клиент находится под индексом {1}", Global.clients[index].IP, index);
        }

        private static void HandleRegisterTry(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            string nickname = buffer.ReadString();
            if (Global.data.LoginExist(username))
            {
                ServerSendData.SendClientAlert(index, "Username already exist");
                return;
            }
            if (Global.data.NicknameExist(nickname))
            {
                ServerSendData.SendClientAlert(index, "Nickname already exist");
                return;
            }
            Global.data.AddAccount(username, password, nickname);
            buffer.Dispose();
            ServerSendData.SendRegisterOk(index);
        }

        private static void HandleLoginTry(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            if (!Global.data.LoginExist(username))
            {
                ServerSendData.SendClientAlert(index, "Username does not exist.");
                return;
            }
            if (!Global.data.PasswordIsOkay(username, password))
            {
                ServerSendData.SendClientAlert(index, "Invalid password.");
                return;
            }
            buffer.Dispose();
            string nickname = Global.data.GetAccountNickname(username);
            int[][] accountdata = Global.data.GetAccountData(nickname);
            int playerIndex = ServerTCP.PlayerLogin(index, nickname, accountdata);
            if (playerIndex != 0)
                ServerSendData.SendLoginOk(index, playerIndex, nickname, accountdata);
        }

        private static void HandleAlert(int index, byte[] data) // DO IT!
        {
            //Handle Alert
        }

        private static void HandleClientClose(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int pindex = buffer.ReadInteger();
            if (pindex == 0)
                Global.clients[index].CloseClient();
            else
                Global.clients[index].CloseClient(pindex);
        }

        private static void HandleClientReconnect(int index, byte[] data)
        {
            // Send to WF that client reconnected
        }

        private static void HandleClientExit(int index, byte[] data)
        {
            ServerSendData.SendClientExit(index);
            Global.clients[index].CloseClient();
        }

    }
}
