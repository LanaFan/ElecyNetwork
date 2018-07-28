using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    class ServerHandleData
    {
        private delegate void Packet_(ClientTCP client, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Packets = new Dictionary<int, Packet_>
            {
                {(int)ClientPackets.CConnectComplite, HandleClientConnect },
                {(int)ClientPackets.CRegisterTry, HandleRegisterTry },
                {(int)ClientPackets.CLoginTry, HandleLoginTry },

                {(int)NetPlayerPackets.PConnectionComplite, HandlePlayerConnect },
                {(int)NetPlayerPackets.PGlChatMsg, HandleGlChatMsg },
                {(int)NetPlayerPackets.PQueueStart, HandleQueueStart },
                {(int)NetPlayerPackets.PQueueStop, HandleQueueStop },
                {(int)NetPlayerPackets.PStopPlayer, HandlePlayerStop },
                {(int)NetPlayerPackets.PGetSkillsBuild, HandleGetSkillBuild },
                {(int)NetPlayerPackets.PSaveSkillsBuild, HandleSaveSkillBuild },

                {(int)RoomPackets.RConnectionComplite, HandleRoomConnect },
                {(int)RoomPackets.RGetPlayers, HandlePlayerSpawn },
                {(int)RoomPackets.RLoadComplite, HandleComplete },
                {(int)RoomPackets.RGetRocks, HandleRockSpawn },
                {(int)RoomPackets.RGetTrees, HandleTreesSpawn },
                {(int)RoomPackets.RGetSpells, HandleGetSpells },
                //{(int)RoomPackets.RInstantiate, HandleInstantiate},
                {(int)RoomPackets.RSurrender, HandleSurrender },
                {(int)RoomPackets.RRoomLeave, HandleRoomLeave },
            };
        }

        public static void HandleNetworkInformation(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            Packet_ Packet;
            buffer.WriteBytes(data);
            int packetnum = buffer.ReadInteger();
            buffer.Dispose();
            if (Packets.TryGetValue(packetnum, out Packet))
            {
                Packet.Invoke(client, data);
            }
        }

        #region Entrance

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum
        /// </summary>
        private static void HandleClientConnect(ClientTCP client, byte[] data)
        {
            Global.serverForm.Debug("Client: " + client.ip + " - connected.");
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string username;
        ///                     string password;
        ///                     string nickname;
        /// </summary>
        private static void HandleRegisterTry(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            string nickname = buffer.ReadString();
            if (Global.data.LoginExist(username))
            {
                ServerSendData.SendClientAlert(client, "Username already exist");
                return;
            }
            if (Global.data.NicknameExist(nickname))
            {
                ServerSendData.SendClientAlert(client, "Nickname already exist");
                return;
            }
            Global.data.AddAccount(username, password, nickname);
            buffer.Dispose();
            ServerSendData.SendRegisterOk(client);
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string username;
        ///                     string password;
        /// </summary>
        private static void HandleLoginTry(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string username = buffer.ReadString();
            if (!Global.data.LoginExist(username))
            {
                ServerSendData.SendClientAlert(client, "Username does not exist.");
                return;
            }
            if (!Global.data.PasswordIsOkay(username, buffer.ReadString()))
            {
                ServerSendData.SendClientAlert(client, "Invalid password.");
                return;
            }
            client.LogIn(username);
            buffer.Dispose();
        }

        #endregion

        #region MainLobby

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        private static void HandlePlayerConnect(ClientTCP client, byte[] data)
        {
            ServerSendData.SendPlayerConnectionOK(client);
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string message;
        /// </summary>
        private static void HandleGlChatMsg(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            ServerSendData.SendGlChatMsg(client.nickname, buffer.ReadString() /* message */);
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     int matchType;
        ///                     string race;
        /// </summary>
        private static void HandleQueueStart(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            Queue.StartSearch(client, buffer.ReadInteger(), buffer.ReadString());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        private static void HandleQueueStop(ClientTCP client, byte[] data)
        {
            Queue.StopSearch(client);
        }


        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        private static void HandlePlayerStop(ClientTCP client, byte[] data) // rewrite (uncorrect use)
        {
            //Global.arena[player.RoomIndex].StartReceive(player);
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string race;
        /// </summary>
        private static void HandleGetSkillBuild(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string race = buffer.ReadString();
            buffer.Dispose();
            int[] skillBuild = Global.data.GetSkillBuildData(client.nickname, race);
            ServerSendData.SendSkillBuild(client, skillBuild, race);
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string race;
        ///                     int spellCount;
        ///                     int[spellCount] spellIndex
        /// </summary>
        private static void HandleSaveSkillBuild(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string race = buffer.ReadString();
            int spellCount = buffer.ReadInteger();
            int[] spellBuild = new int[spellCount];
            for (int i = 0; i < spellCount; i++)
            {
                spellBuild[i] = buffer.ReadInteger();
            }
            buffer.Dispose();
            Global.data.SetSkillBuildData(client.nickname, race, spellBuild);
            ServerSendData.SendBuildSaved(client);
        }

        #endregion

        #region GameRoom


        private static void HandleRoomConnect(ClientTCP client, byte[] data)
        {
            client.room.SetGameLoadData(client);
        }

        private static void HandlePlayerSpawn(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            client.room.SetLoadProgress(client, buffer.ReadFloat());
            buffer.Dispose();
            client.room.SpawnPlayers(client);
        }

        private static void HandleRockSpawn(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            client.room.SetLoadProgress(client, buffer.ReadFloat());
            client.room.SpawnRock(client, buffer.ReadInteger(), buffer.ReadBool(), buffer.ReadBool(), buffer.ReadBool());
            buffer.Dispose();
        }

        private static void HandleTreesSpawn(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            client.room.SetLoadProgress(client, buffer.ReadFloat());
            client.room.SpawnTree(client, buffer.ReadInteger(), buffer.ReadBool(), buffer.ReadBool(), buffer.ReadBool());
            buffer.Dispose();
        }

        private static void HandleComplete(ClientTCP client, byte[] data)
        {
            Global.serverForm.Debug("HAndle complite " + client.nickname);
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            client.room.SetLoadProgress(client, buffer.ReadFloat());
            buffer.Dispose();
            client.room.LoadComplete(client);
        }

        private static void HandleGetSpells(ClientTCP client, byte[] data)
        {
            Global.serverForm.Debug("Get Spells Handled " + client.nickname);
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            client.room.SetLoadProgress(client, buffer.ReadFloat());
            buffer.Dispose();
            client.room.LoadSpells(client);
        }

        //private static void HandleInstantiate(int ID, GameRoom Room, byte[] data)
        //{
        //    float[] pos = new float[3];
        //    float[] rot = new float[4];
        //    PacketBuffer buffer = new PacketBuffer();
        //    buffer.WriteBytes(data);
        //    buffer.ReadInteger();
        //    int objId = buffer.ReadInteger();
        //    int instanseType = buffer.ReadInteger();
        //    string objectReference = buffer.ReadString();
        //    pos[0] = buffer.ReadFloat();
        //    pos[1] = buffer.ReadFloat();
        //    pos[2] = buffer.ReadFloat();
        //    rot[0] = buffer.ReadFloat();
        //    rot[1] = buffer.ReadFloat();
        //    rot[2] = buffer.ReadFloat();
        //    rot[3] = buffer.ReadFloat();
        //    buffer.Dispose();
        //    //adding the object to array for start to observe
        //}

        //private static void HandleTransform(int ID, GameRoom Room, byte[] data)
        //{
        //    PacketBuffer buffer = new PacketBuffer();
        //    buffer.WriteBytes(data);
        //    buffer.ReadInteger();
        //    float[] pos = new float[] { buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat()};
        //    float[] rot = new float[] { buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat()};
        //    buffer.Dispose();
        //    Room.SetTransform(ID, pos, rot);
        //}

        private static void HandleSurrender(ClientTCP client, byte[] data)
        {
            client.room.Surrended(client);
        }

        private static void HandleRoomLeave(ClientTCP client, byte[] data)
        {
            ServerSendData.SendRoomLogOut(client);
            client.room.LeaveRoom(client);
        }


        #endregion
    }
}
