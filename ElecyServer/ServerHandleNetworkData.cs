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
                {(int)NetPlayerPackets.PConnectionComplite, HandlePlayerConnect },
                {(int)NetPlayerPackets.PGlChatMsg, HandleGlChatMsg },
                {(int)NetPlayerPackets.PQueueStart, HandleNormalQueueStart },
                {(int)NetPlayerPackets.PSearch, HandleSearch },
                {(int)NetPlayerPackets.PQueueStop, HandleQueueStop },
                {(int)RoomPackets.RConnectionComplite, HandleRoomConnect },
                {(int)RoomPackets.RTransform, HandleTransform },
                {(int)RoomPackets.RLoadComplete, HandleComplete }
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
            Console.WriteLine("Соединение с {0} установлено. Клиент находится под индексом {1}", Global.clients[index].ip, index);
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
            Global.clients[index].CloseClient();
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
            string Nickname = Global.players[index].nickname;
            buffer.Dispose();
            ServerSendData.SendGlChatMsg(index,Nickname, GlChatMsg);
        }

        private static void HandlePlayerConnect(int index, byte[] data)
        {
            ServerSendData.SendPlayerConnectionOK(index);
        }

        private static void HandleNormalQueueStart(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int matchType = buffer.ReadInteger();
            buffer.Dispose();
            switch (matchType)
            {
                case 0:
                    for(int i = 0; i < Constants.MAX_PLAYERS; i++)
                    {
                        if (Global.normalQueue[i] == 0)
                        {
                            Global.normalQueue[i] = index;
                            break;
                        }
                    }
                    break;
            }
            ServerSendData.SendQueueStarted(index);
                    
        }

        private static void HandleSearch(int index, byte[] data)
        {
            int index2;
            int roomIndex = -1;
            bool found = false;
            for(int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if(Global.normalQueue[i] != 0 && Global.normalQueue[i] != index)
                {
                    index2 = i;
                    for(int j = 0; j < Constants.ARENA_SIZE; j++)
                    {
                        if(Global.arena[j] == null)
                        {
                            Global.players[index].NetPlayerStop();
                            Global.players[i].NetPlayerStop();
                            Global.arena[j] = new GameRoom(j, Global.players[index], Global.players[i]);
                            Queue.StopSearch(index, i);
                            roomIndex = j;
                            found = true;
                            break;
                        }
                        ServerSendData.SendPlayerAlert(index, "No more emty game room");
                    }
                    if (found)
                        ServerSendData.SendMatchFound(index, index2, roomIndex);
                    else
                        ServerSendData.SendQueueContinue(index);
                    break;
                }
            }
        }


        private static void HandleQueueStop(int index, byte[] data)
        {
            Queue.StopSearch(index);
        }

        #endregion

        #region Game Room Handle

        private static void HandleRoomConnect(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            buffer.Dispose();
            ServerSendData.SendGameData(ID, roomIndex);
        }

        private static void HandleComplete(int ID, byte[] data)
        {
            float[] pos = new float[3];
            float[] rot = new float[4];
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            pos[0] = buffer.ReadFloat();
            pos[1] = buffer.ReadFloat();
            pos[2] = buffer.ReadFloat();
            rot[0] = buffer.ReadFloat();
            rot[1] = buffer.ReadFloat();
            rot[2] = buffer.ReadFloat();
            rot[3] = buffer.ReadFloat();
            buffer.Dispose();
            Global.arena[roomIndex].SetTransform(ID, pos, rot);
        }

        private static void HandleTransform(int ID, byte[] data)
        {
            float[] pos = new float[3];
            float[] rot = new float[4];
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            pos[0] = buffer.ReadFloat();
            pos[1] = buffer.ReadFloat();
            pos[2] = buffer.ReadFloat();
            rot[0] = buffer.ReadFloat();
            rot[1] = buffer.ReadFloat();
            rot[2] = buffer.ReadFloat();
            rot[3] = buffer.ReadFloat();
            buffer.Dispose();
            Global.arena[roomIndex].SetTransform(ID, pos, rot);
        }

        #endregion

    }
}
