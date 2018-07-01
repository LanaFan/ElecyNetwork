using Bindings;
using System;

namespace ElecyServer
{
    class ServerSendData
    {
        #region Send to Client

        public static void SendClientConnetionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            ServerTCP.SendClientConnection(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendClientAlert(int index, string message)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SAlert);
            buffer.WriteString(message);
            ServerTCP.SendDataToClient(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRegisterOk(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRegisterOK);
            ServerTCP.SendDataToClient(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLoginOk(int index, int playerIndex, string nickname, int[][]accountdata)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SLoginOK);
            buffer.WriteInteger(playerIndex);
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
            ServerTCP.SendDataToClient(index, buffer.ToArray());
            buffer.Dispose();
        }

        #endregion

        #region Send to Player

        public static void SendPlayerConnectionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            buffer.WriteString("Hello sexy guy...We've been waiting for you =*");
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendGlChatMsg(string Nickname, string GlChatMsg)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SGlChatMsg);
            buffer.WriteString(Nickname);
            buffer.WriteString(GlChatMsg);
            Global.serverForm.ShowChatMsg(Nickname, GlChatMsg);
            ServerTCP.SendDataToAllPlayers(buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendQueueStarted(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SQueueStarted);
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendQueueContinue(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SQueueContinue);
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendMatchFound(int mapIndex, int index1, int index2, int roomIndex)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SMatchFound);
            buffer.WriteInteger(roomIndex);
            buffer.WriteInteger(mapIndex);
            ServerTCP.SendDataToPlayer(index1, buffer.ToArray());
            ServerTCP.SendDataToPlayer(index2, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendPlayerAlert(int index,string alert)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SAlert);
            buffer.WriteString(alert);
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendPlayerLogOut(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SNetPlayerLogOut);
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendSkillBuild(int index, int[] spellIndexes, string race)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SBuildInfo);
            buffer.WriteString(race);
            buffer.WriteInteger(spellIndexes.Length);
            for(int i = 0; i < spellIndexes.Length; i++)
            {
                buffer.WriteInteger(spellIndexes[i]);
            }
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendBuildSaved(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SBuildSaved);
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        #endregion

        #region Send to GameRoom

        public static void SendGameData(int roomIndex, string nickname1, string nickname2, float[][]positions, float[][]rotations)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SLoadStarted);
            buffer.WriteString(nickname1);
            buffer.WriteString(nickname2);
            buffer.WriteFloat(positions[0][0]);
            buffer.WriteFloat(positions[0][1]);
            buffer.WriteFloat(positions[0][2]);
            buffer.WriteFloat(positions[1][0]);
            buffer.WriteFloat(positions[1][1]);
            buffer.WriteFloat(positions[1][2]);
            buffer.WriteFloat(rotations[0][0]);
            buffer.WriteFloat(rotations[0][1]);
            buffer.WriteFloat(rotations[0][2]);
            buffer.WriteFloat(rotations[0][3]);
            buffer.WriteFloat(rotations[1][0]);
            buffer.WriteFloat(rotations[1][1]);
            buffer.WriteFloat(rotations[1][2]);
            buffer.WriteFloat(rotations[1][3]);
            ServerTCP.SendDataToGamePlayers(roomIndex, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRockSpawned(int ID, int roomIndex, int[] ranges) 
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRockSpawn);
            int start = ranges[0];
            int end = ranges[1];
            buffer.WriteInteger(end - start);
            while(start <= end)
            {
                float[] pos = Global.arena[roomIndex].ObjectsList.Get(start).Position;
                float[] rot = Global.arena[roomIndex].ObjectsList.Get(start).Rotation;
                buffer.WriteInteger(start);
                buffer.WriteFloat(pos[0]);
                buffer.WriteFloat(pos[1]);
                buffer.WriteFloat(pos[2]);
                buffer.WriteFloat(rot[0]);
                buffer.WriteFloat(rot[1]);
                buffer.WriteFloat(rot[2]);
                buffer.WriteFloat(rot[3]);
                start++;
            }
            ServerTCP.SendDataToGamePlayer(roomIndex, ID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendTreeSpawned(int ID, int roomIndex, int[] ranges)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.STreeSpawn);
            int start = ranges[0];
            int end = ranges[1];
            buffer.WriteInteger(end - start);
            while (start <= end)
            {
                float[] pos = Global.arena[roomIndex].ObjectsList.Get(start).Position;
                float[] rot = Global.arena[roomIndex].ObjectsList.Get(start).Rotation;
                buffer.WriteInteger(start);
                buffer.WriteFloat(pos[0]);
                buffer.WriteFloat(pos[1]);
                buffer.WriteFloat(pos[2]);
                buffer.WriteFloat(rot[0]);
                buffer.WriteFloat(rot[1]);
                buffer.WriteFloat(rot[2]);
                buffer.WriteFloat(rot[3]);
                start++;
            }
            ServerTCP.SendDataToGamePlayer(roomIndex, ID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLoadSpells(int ID, int roomIndex, int[] spellsNumberFirst, int[] spellsNumberSecond)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SSpellLoad);
            buffer.WriteInteger(spellsNumberFirst.Length);
            buffer.WriteInteger(spellsNumberSecond.Length);
            for(int i = 0; i < spellsNumberFirst.Length; i++)
            {
                buffer.WriteInteger(spellsNumberFirst[i]);
            }
            for(int i = 0; i < spellsNumberSecond.Length; i++)
            {
                buffer.WriteInteger(spellsNumberSecond[i]);
            }
            ServerTCP.SendDataToGamePlayer(roomIndex, ID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRoomStart(int roomIndex)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRoomStart);
            ServerTCP.SendDataToGamePlayers(roomIndex, buffer.ToArray());
            buffer.Dispose();
        }

        //public static void SendTransform(int ID, int roomIndex, float[] pos, float[] rot)
        //{
        //    PacketBuffer buffer = new PacketBuffer();
        //    buffer.WriteInteger((int)ServerPackets.STransform);
        //    buffer.WriteFloat(pos[0]);
        //    buffer.WriteFloat(pos[1]);
        //    buffer.WriteFloat(pos[2]);
        //    buffer.WriteFloat(rot[0]);
        //    buffer.WriteFloat(rot[1]);
        //    buffer.WriteFloat(rot[2]);
        //    buffer.WriteFloat(rot[3]);
        //    ServerTCP.SendDataToGamePlayer(roomIndex, ID, buffer.ToArray());
        //    buffer.Dispose();
        //}

        public static void SendInstantiate(int ID, int roomIndex, float[] pos, float[] rot)
        {
            //here comes to sen to both player in the room info about object to instantiate
        }

        public static void SendEnemyProgress(int ID, int roomIndex, float loadProgress)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SEnemyLoadProgress);
            buffer.WriteFloat(loadProgress);
            ServerTCP.SendDataToGamePlayer(roomIndex, ID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRoomLogOut(int ID, int roomIndex)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SPlayerLogOut);
            ServerTCP.SendDataToGamePlayer(roomIndex, ID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendMatchEnded(int ID, int roomIndex, string winner)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SMatchResult);
            buffer.WriteString(winner);
            ServerTCP.SendDataToGamePlayer(roomIndex, ID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendMatchEnded(int roomIndex, string winner)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SMatchResult);
            buffer.WriteString(winner);
            ServerTCP.SendDataToGamePlayers(roomIndex, buffer.ToArray());
            buffer.Dispose();
        }

        #endregion
    }
}
