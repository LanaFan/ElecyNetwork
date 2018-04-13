using Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                {(int)SystemPackets.SysExit, HandlePlayerExit },
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
            Global.serverForm.ShowChatMsg(Nickname, GlChatMsg);
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
            buffer.Dispose();
            if (!Queue.StartSearch(index, matchType))
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

        private static void HandlePlayerExit(int index, byte[] data)
        {
            ServerSendData.SendPlayerExit(index);
            Global.players[index].ClosePlayer();
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

    }
}
