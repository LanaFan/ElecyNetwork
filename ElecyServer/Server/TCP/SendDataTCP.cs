﻿using Bindings;
using System.Linq;

namespace ElecyServer
{
    class SendDataTCP
    {

        #region Send to Client

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        public static void SendClientConnetionOK(ClientTCP client)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            ServerTCP.SendClientConnection(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string message;
        /// </summary>
        public static void SendClientAlert(ClientTCP client, string message)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SAlert);
            buffer.WriteString(message);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        public static void SendRegisterOk(ClientTCP client)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRegisterOK);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string nickname;
        ///                     int[5] level;
        ///                     int[5] rank;
        /// </summary>
        public static void SendLoginOk(string nickname, int[][]accountdata, ClientTCP client)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SLoginOK);
            buffer.WriteString(nickname);
            buffer.WriteInteger(accountdata[0][0]);
            buffer.WriteInteger(accountdata[0][1]);
            buffer.WriteInteger(accountdata[0][2]);
            buffer.WriteInteger(accountdata[0][3]);
            buffer.WriteInteger(accountdata[0][4]);
            buffer.WriteInteger(accountdata[1][0]);
            buffer.WriteInteger(accountdata[1][1]);
            buffer.WriteInteger(accountdata[1][2]);
            buffer.WriteInteger(accountdata[1][3]);
            buffer.WriteInteger(accountdata[1][4]);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        #endregion

        #region Send to Player

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string message;
        /// </summary>
        public static void SendPlayerConnectionOK(ClientTCP player)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            buffer.WriteString("Hello sexy guy...We've been waiting for you =*");
            ServerTCP.SendDataToClient(player, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string nickname;
        ///                     string message;
        /// </summary>
        public static void SendGlChatMsg(string Nickname, string GlChatMsg)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SGlChatMsg);
            buffer.WriteString(Nickname);
            buffer.WriteString(GlChatMsg);
            Global.serverForm.ShowChatMsg(Nickname, GlChatMsg);
            ServerTCP.SendChatMsgToAllClients(buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        public static void SendQueueStarted(ClientTCP client)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SQueueStarted);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     int sceneIndex;
        /// </summary>
        public static void SendMatchFound(BaseGameRoom room)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SMatchFound);
            buffer.WriteInteger((int)room.roomType);
            ServerTCP.SendDataToRoomPlayers(room, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string alert;
        /// </summary>
        public static void SendPlayerAlert(ClientTCP client, string alert)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SAlert);
            buffer.WriteString(alert);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string race;
        ///                     int spellCount;
        ///                     int[spellCount] spellIndex;
        /// </summary>
        public static void SendSkillBuild(ClientTCP client, short[] spellIndexes, string race)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SBuildInfo);
            buffer.WriteString(race);
            buffer.WriteInteger(spellIndexes.Length);
            for(int i = 0; i < spellIndexes.Length; i++)
            {
                buffer.WriteShort(spellIndexes[i]);
            }
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        public static void SendBuildSaved(ClientTCP client)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SBuildSaved);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        #endregion

        #region Send to GameRoom
        
        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     int mapIndex;
        /// </summary>
        public static void SendMapData(int mapIndex, ClientTCP client)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SMapLoad);
            buffer.WriteInteger(mapIndex);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string firstPlayerNickname;
        ///                     string secondPlayerNickname;
        ///                     float[3] firstPlayerPosition;
        ///                     float[3] secondPlayerPosition;
        ///                     float[4] firstPlayerRotation;
        ///                     float[4] secondPlayerRotation;
        /// </summary>
        public static void SendPlayersSpawned(ClientTCP client, BaseGameRoom room)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SPlayerSpawned);
            buffer.WriteInteger(room.PlayersCount);
            for(int i = 0; i < room.PlayersCount; i++)
            {
                buffer.WriteString(room.playersTCP[i].nickname);
                buffer.WriteFloat(room.map.SpawnPoints.ToArray<SpawnPoint>()[i].PositionX);
                buffer.WriteFloat(room.map.SpawnPoints.ToArray<SpawnPoint>()[i].PositionY);
                buffer.WriteFloat(room.map.SpawnPoints.ToArray<SpawnPoint>()[i].RotationX);
                buffer.WriteFloat(room.map.SpawnPoints.ToArray<SpawnPoint>()[i].RotationY);
                buffer.WriteFloat(room.map.SpawnPoints.ToArray<SpawnPoint>()[i].RotationZ);
                buffer.WriteFloat(room.map.SpawnPoints.ToArray<SpawnPoint>()[i].RotationW);
            }
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     int rockCount;
        ///                     for(rockCount)
        ///                     {
        ///                         int Index;
        ///                         int Health;
        ///                         float[3] pos;
        ///                         float[4] rot;
        ///                     }
        ///                     
        /// </summary>
        public static void SendRockSpawned(ClientTCP client, int[] ranges) 
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRockSpawned);
            int start = ranges[0];
            int end = ranges[1];
            buffer.WriteInteger(end - start+1);
            while(start <= end)
            {
                var k = client.room.staticObjectsList.Get(start).GetInfo();
                buffer.WriteInteger(start);
                buffer.WriteInteger(k.Item1);
                buffer.WriteFloat(k.Item2[0]);
                buffer.WriteFloat(k.Item2[1]);
                buffer.WriteFloat(k.Item2[2]);
                buffer.WriteFloat(k.Item3[0]);
                buffer.WriteFloat(k.Item3[1]);
                buffer.WriteFloat(k.Item3[2]);
                buffer.WriteFloat(k.Item3[3]);
                start++;
            }
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     int treeCount;
        ///                     for(treeCount)
        ///                     {
        ///                         int Index;
        ///                         int Health;
        ///                         float[3] pos;
        ///                         float[4] rot;
        ///                     }
        ///                     
        /// </summary>
        public static void SendTreeSpawned(ClientTCP client, int[] ranges)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.STreeSpawned);
            int start = ranges[0];
            int end = ranges[1];
            buffer.WriteInteger(end - start+1);
            while (start <= end)
            {
                var k = client.room.staticObjectsList.Get(start).GetInfo();
                buffer.WriteInteger(start);
                buffer.WriteInteger(k.Item1);
                buffer.WriteFloat(k.Item2[0]);
                buffer.WriteFloat(k.Item2[1]);
                buffer.WriteFloat(k.Item2[2]);
                buffer.WriteFloat(k.Item3[0]);
                buffer.WriteFloat(k.Item3[1]);
                buffer.WriteFloat(k.Item3[2]);
                buffer.WriteFloat(k.Item3[3]);
                start++;
            }
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     int amount of spellBuilds (number of players)
        ///                     for(amount of spellBuilds)
        ///                         int spellBuildLength
        ///                         short[spellBuildLength] spellBuild
        /// </summary>
        public static void SendLoadSpells(ClientTCP client, short[][] spellBuilds, int totalNumberOfSpells) // Нужно согласовать как будет эта херь выглядеть (избавится от инта)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SSpellLoaded);
            buffer.WriteInteger(spellBuilds.Length);
            buffer.WriteInteger(totalNumberOfSpells);
            foreach(short[] spellBuild in spellBuilds)
            {
                for(int i = 0; i < spellBuild.Length; i++)
                {
                    buffer.WriteShort(spellBuild[i]);
                }
            }
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        public static void SendRoomStart(BaseGameRoom room)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRoomStart);
            ServerTCP.SendDataToRoomPlayers(room, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     float loadProgress;
        /// </summary>
        public static void SendEnemyProgress(BaseGameRoom room, ClientTCP client, float loadProgress)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SEnemyLoadProgress);
            buffer.WriteFloat(loadProgress);
            ServerTCP.SendDataToRoomPlayers(room, client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        /// </summary>
        public static void SendRoomLogOut(ClientTCP client)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SPlayerLogOut);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string winnerNickname;
        /// </summary>
        public static void SendMatchEnded(ClientTCP client)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SMatchResult);
            buffer.WriteString(client.nickname);
            ServerTCP.SendDataToClient(client, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     string winnerNickname;
        /// </summary>
        public static void SendMatchEnded(string nickname, BaseGameRoom room)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SMatchResult);
            buffer.WriteString(nickname);
            ServerTCP.SendDataToRoomPlayers(room, buffer.ToArray());
            buffer.Dispose();
        }

        /// <summary>
        ///             Buffer:
        ///                     int PacketNum;
        ///                     int SpellIndex; (index in client's spell array)
        ///                     int DynamicIndex; (index in dynamicObjects list)
        ///                     int parentIndex;
        ///                     float[3] position;
        ///                     float[4] rotation;
        ///                     int hp;
        ///                     string nickname; (nickname of master-client)
        /// </summary>
        public static void SendInstantiate(BaseGameRoom room, int spellIndex, int dynamicIndex, int parentIndex, float[] castPos, float[] targetPos, float[] rot, int hp, string nickname)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteInteger((int)ServerPackets.SInstantiate);
                buffer.WriteInteger(spellIndex);
                buffer.WriteInteger(dynamicIndex);
                buffer.WriteInteger(parentIndex);
                buffer.WriteFloat(castPos[0]);
                buffer.WriteFloat(castPos[1]);
                buffer.WriteFloat(castPos[2]);
                buffer.WriteFloat(targetPos[0]);
                buffer.WriteFloat(targetPos[1]);
                buffer.WriteFloat(targetPos[2]);
                buffer.WriteFloat(rot[0]);
                buffer.WriteFloat(rot[1]);
                buffer.WriteFloat(rot[2]);
                buffer.WriteFloat(rot[3]);
                buffer.WriteInteger(hp);
                buffer.WriteString(nickname);
                ServerTCP.SendDataToRoomPlayers(room, buffer.ToArray());
            }
        }

        public static void SendDestroy(BaseGameRoom room, int spellIndex)
        {
            using (PacketBuffer buffer = new PacketBuffer()) 
            {
                buffer.WriteInteger((int)ServerPackets.SDestroy);
                buffer.WriteInteger(spellIndex);
                ServerTCP.SendDataToRoomPlayers(room, buffer.ToArray());
            }
        }

        #endregion

    }
}
