using Bindings;
using System;
using System.Collections.Generic;

namespace ElecyServer
{
    class ServerHandlePlayerData
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Packets = new Dictionary<int, Packet_>
            {
                {(int)NetPlayerPackets.PConnectionComplite, HandlePlayerConnect },
                {(int)NetPlayerPackets.PGlChatMsg, HandleGlChatMsg },
                {(int)NetPlayerPackets.PQueueStart, HandleQueueStart },
                {(int)NetPlayerPackets.PQueueStop, HandleQueueStop },
                {(int)NetPlayerPackets.PStopPlayer, HandlePlayerStop },
                {(int)NetPlayerPackets.PLogOut, HandlePlayerLogOut },
                {(int)NetPlayerPackets.PGetSkillsBuild, HandleGetSkillBuild },
                {(int)NetPlayerPackets.PSaveSkillsBuild, HandleSaveSkillBuild }
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

        private static void HandleGlChatMsg(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string GlChatMsg = buffer.ReadString();
            string Nickname = Global.players[index].Nickname;
            buffer.Dispose();
            ServerSendData.SendGlChatMsg(Nickname, GlChatMsg);
        }

        private static void HandlePlayerConnect(int index, byte[] data)
        {
            ServerSendData.SendPlayerConnectionOK(index);
        }

        private static void HandleQueueStart(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int matchType = buffer.ReadInteger();
            string race = buffer.ReadString();
            buffer.Dispose();
            if (!Queue.StartSearch(index, matchType, race))
                ServerSendData.SendPlayerAlert(index, "Queue is overcrowded. Try again later!");
        }

        private static void HandleQueueStop(int index, byte[] data)
        {
            Queue.StopSearch(index, Global.players[index].RoomIndex);
        }

        private static void HandlePlayerStop(int index, byte[] data)
        {
            Global.arena[Global.players[index].RoomIndex].StartReceive(index);
        }

        private static void HandlePlayerLogOut(int index, byte[] data)
        {
            int cIndex = ServerTCP.AddClient(Global.players[index].Socket);
            if (cIndex != 0)
            {
                ServerSendData.SendPlayerLogOut(index);
                Global.players[index].ClosePlayer();
                Global.clients[cIndex].StartClient();
            }
            else
            {
                ServerSendData.SendPlayerAlert(index, "No empty client slots. Try again later");
            }
        }

        private static void HandleGetSkillBuild(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string race = buffer.ReadString();
            int[] skillBuild = Global.data.GetSkillBuildData(Global.players[index].Nickname, race);
            ServerSendData.SendSkillBuild(index, skillBuild, race);
        }

        private static void HandleSaveSkillBuild(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string raceName = buffer.ReadString();
            int skillCount = buffer.ReadInteger();
            int[] skillBuild = new int[skillCount];
            for(int i = 0; i < skillCount; i++)
            {
                skillBuild[i] = buffer.ReadInteger();
            }
            Global.data.SetSkillBuildData(Global.players[index].Nickname, raceName, skillBuild);
            ServerSendData.SendBuildSaved(index);
        }
    }
}
