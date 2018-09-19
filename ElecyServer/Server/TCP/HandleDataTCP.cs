<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
=======
﻿using System.Collections.Generic;
using System.Linq;
>>>>>>> DataBase_rework
using Bindings;

namespace ElecyServer
{
    class HandleDataTCP
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
                {(int)NetPlayerPackets.PGetSkillsBuild, HandleGetSkillBuild },
                {(int)NetPlayerPackets.PSaveSkillsBuild, HandleSaveSkillBuild },
                {(int)NetPlayerPackets.PTestRoom, HandleTestRoom },
                {(int)NetPlayerPackets.PAddFriend, HandleAddFriend },

                {(int)RoomPackets.RConnectionComplite, HandleRoomConnect },
                {(int)RoomPackets.RGetPlayers, HandlePlayerSpawn },
                {(int)RoomPackets.RGetRocks, HandleRockSpawn },
                {(int)RoomPackets.RGetTrees, HandleTreesSpawn },
                {(int)RoomPackets.RGetSpells, HandleGetSpells },
                {(int)RoomPackets.RLoadComplite, HandleComplete },
                {(int)RoomPackets.RSurrender, HandleSurrender },
                {(int)RoomPackets.RRoomLeave, HandleRoomLeave },
                {(int)RoomPackets.RInstantiate, HandleInstantiate },
                {(int)RoomPackets.RDestroy, HandleDestroy },
                {(int)RoomPackets.RDamage, HandleDamage }
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
                SendDataTCP.SendClientAlert(client, "Username already exist");
                return;
            }
            if (Global.data.NicknameExist(nickname))
            {
                SendDataTCP.SendClientAlert(client, "Nickname already exist");
                return;
            }
            Global.data.AddAccount(username, password, nickname);
            buffer.Dispose();
            SendDataTCP.SendRegisterOk(client);
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
                SendDataTCP.SendClientAlert(client, "Username does not exist.");
                return;
            }
            if (!Global.data.PasswordIsOkay(username, buffer.ReadString()))
            {
                SendDataTCP.SendClientAlert(client, "Invalid password.");
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
            SendDataTCP.SendPlayerConnectionOK(client);
            SendDataTCP.SendFriendsInfo(client);
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
            SendDataTCP.SendGlChatMsg(client.nickname, buffer.ReadString() /* message */);
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
            foreach(ClientTCP friend in client.friends)
            {
                SendDataTCP.SendFriendChange(client, friend);
            }
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        private static void HandleQueueStop(ClientTCP client, byte[] data)
        {
            Queue.StopSearch(client);
            foreach (ClientTCP friend in client.friends)
            {
                SendDataTCP.SendFriendChange(client, friend);
            }
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
            short[] skillBuild = client.accountData.AccountSkillBuilds.IgnisBuild.ToArray();
            SendDataTCP.SendSkillBuild(client, skillBuild, race);
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string race;
        ///                     int spellCount;
        ///                     short[spellCount] spellIndex
        /// </summary>
        private static void HandleSaveSkillBuild(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string race = buffer.ReadString();
            int spellCount = buffer.ReadInteger();
            string[] spellBuild = new string[spellCount];
            for (int i = 0; i < spellCount; i++)
            {
                string spellIndex = "" + buffer.ReadShort();
                while (spellIndex.Length != 4)
                {
                    spellIndex = "0" + spellIndex;
                }
                string spellType = "" + buffer.ReadShort();
                while (spellType.Length != 4)
                {
                    spellType = "0" + spellType;
                }
                spellBuild[i] = spellIndex + "" + spellType;
            }
            buffer.Dispose();
            //Global.data.SetSkillBuildData(client.nickname, race, spellBuild);
            SendDataTCP.SendBuildSaved(client);
        }

        private static void HandleTestRoom(ClientTCP client, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                int mapIndex = buffer.ReadInteger();
                client.race = buffer.ReadString(); 
                Global.roomsList.Add(new TestRoom(client, mapIndex));
            }
        }

        private static void HandleAddFriend(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string guideTag = buffer.ReadString();
            buffer.Dispose();
            client.AddFriend(guideTag);
        }

        #endregion

        #region GameRoom

        private static void HandleRoomConnect(ClientTCP client, byte[] data)
        {
            client.room.SetGameArea(client);
        }

        private static void HandlePlayerSpawn(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            client.room.SetLoadProgress(client, buffer.ReadFloat());
            buffer.Dispose();
            client.room.SetPlayers(client);
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
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            client.room.SetLoadProgress(client, buffer.ReadFloat());
            buffer.Dispose();
            client.room.LoadComplite(client);
        }

        private static void HandleGetSpells(ClientTCP client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            client.room.SetLoadProgress(client, buffer.ReadFloat());
            buffer.Dispose();
            client.room.LoadSpells(client);
        }

        private static void HandleSurrender(ClientTCP client, byte[] data)
        {
            client.room.Surrended(client);
        }

        private static void HandleRoomLeave(ClientTCP client, byte[] data)
        {
            SendDataTCP.SendRoomLogOut(client);
            client.room.DeletePlayer(client);
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     int SpellIndex; (index in client's spell array)
        ///                     int ParentIndex;
        ///                     float[3] position;
        ///                     float[4] rotation;
        ///                     int hp;
        /// </summary>
        private static void HandleInstantiate(ClientTCP client, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                client.room.dynamicObjectsList.Add(
                                            client.room,
                                            buffer.ReadInteger(),
                                            buffer.ReadInteger(),
                                            new float[] {
                                                        buffer.ReadFloat(),
                                                        buffer.ReadFloat(),
                                                        buffer.ReadFloat()
                                                        },
                                            new float[] {
                                                        buffer.ReadFloat(),
                                                        buffer.ReadFloat(),
                                                        buffer.ReadFloat()
                                                        },
                                            new float[] {
                                                        buffer.ReadFloat(),
                                                        buffer.ReadFloat(),
                                                        buffer.ReadFloat(),
                                                        buffer.ReadFloat()
                                                        },
                                            buffer.ReadInteger(),
                                            client.nickname
                                            );
            }
        }

        private static void HandleDestroy(ClientTCP client, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                client.room.dynamicObjectsList.Destroy(buffer.ReadInteger());
            }
        }


        private static void HandleDamage(ClientTCP client, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index;
                int damage;
                switch (type)
                {
                    case ObjectType.player:
                        index = buffer.ReadInteger();
                        damage = buffer.ReadInteger();
                        client.room.roomPlayers[index].TakeDamage(damage);
                        break;
                    case ObjectType.spell:
                        index = buffer.ReadInteger();
                        damage = buffer.ReadInteger();
                        client.room.dynamicObjectsList[index].TakeDamage(damage);
                        break;
                }
            }
        }

        #endregion
    }
}
