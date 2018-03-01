using System;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    class ServerHandleNetworkData
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Packets = new Dictionary<int, Packet_>
            {
                {(int)ClientPackets.CConnectcomplite, HandleConnect },
                {(int)ClientPackets.CRegisterTry, HandleRegisterTry },
                {(int)ClientPackets.CLoginTry, HandleLoginTry },
                {(int)ClientPackets.CAlert, HandleAlert },
                {(int)ClientPackets.CClose, HandleClientClose },
                {(int)ClientPackets.CGlChatMsg, HandleGlChatMsg }
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

        private static void HandleConnect(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            Console.WriteLine(msg);
        }

        private static void HandleRegisterTry(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            string nickname = buffer.ReadString();
            if (Global.data.AccountExist(index,username))
            {
                ServerSendData.SendAlert(index, "Username already exist");
                return;
            }
            if (Global.data.NicknameExist(index, nickname))
            {
                ServerSendData.SendAlert(index, "Nickname already exist");
                return;
            }
            Global.data.AddAccount(username, password, nickname);
            buffer.Dispose();
            ServerSendData.SendRegisterOk(index);
            Console.WriteLine("Player: " + username + " succesfully registered.");
        }

        private static void HandleLoginTry(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            if (!Global.data.AccountExist(index, username))
            {
                ServerSendData.SendAlert(index, "Username does not exist.");
                return;
            }
            if (!Global.data.PasswordIsOkay(index, username, password))
            {
                ServerSendData.SendAlert(index, "Invalid password.");
                return;
            }
            buffer.Dispose();
            string nickname = Global.data.GetAccountNickname(username);
            int[][] accountdata = Global.data.GetAccountLevels(username);
            ServerTCP.PlayerLogin(index, nickname, accountdata);
            ServerSendData.SendLoginOk(index, nickname, accountdata);
            //ServerTCP._clients[index].CloseClient(index);
            Console.WriteLine("Player: " + username + " logged in succesfully");
        }

        private static void HandleAlert(int index, byte[] data)
        {

        }

        private static void HandleClientClose(int index, byte[] data)
        {
            Console.WriteLine("I'm handling client close");
            ServerTCP._clients[index].CloseClient();
        }

        private static void HandleGlChatMsg(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string GlChatMsg = buffer.ReadString();
            string Nickname = ServerTCP._players[index].nickname;
            buffer.Dispose();
            ServerSendData.SendGlChatMsg(Nickname, GlChatMsg);
        }
    }
}
