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
                {(int)ClientPackets.CConnectComplite, HandleClientConnect },
                {(int)ClientPackets.CRegisterTry, HandleRegisterTry },
                {(int)ClientPackets.CLoginTry, HandleLoginTry },
                {(int)ClientPackets.CAlert, HandleAlert },
                {(int)ClientPackets.CClose, HandleClientClose },
                {(int)ClientPackets.CReconnectComplite, HandleReconnect },
                {(int)PlayerPackets.PConnectionComplite, HandlePlayerConnect },
                {(int)PlayerPackets.PGlChatMsg, HandleGlChatMsg }
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

        #region Client Handlers

        private static void HandleClientConnect(int index, byte[] data)
        {
            Console.WriteLine("Соединение с {0} установлено. Клиент находится под индексом {1}", Global._clients[index].ip, index);
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
            if(playerIndex != 0)
                ServerSendData.SendLoginOk(index, playerIndex, nickname, accountdata);
        }

        private static void HandleAlert(int index, byte[] data) // DO IT!
        {
            //Handle Alert
        }

        private static void HandleClientClose(int index, byte[] data)
        {
            Global._clients[index].CloseClient();
        }

        private static void HandleReconnect(int index, byte[] data) //DO IT, TOO!
        {
            //Handle!
        }

        #endregion

        #region Player Handlers

        private static void HandleGlChatMsg(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string GlChatMsg = buffer.ReadString();
            string Nickname = Global._players[index].nickname;
            buffer.Dispose();
            ServerSendData.SendGlChatMsg(index,Nickname, GlChatMsg);
        }

        private static void HandlePlayerConnect(int index, byte[] data)
        {
            ServerSendData.SendPlayerConnectionOK(index);
        }

        #endregion
    }
}
