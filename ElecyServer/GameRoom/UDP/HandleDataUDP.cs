using System.Collections.Generic;
using System.Net;
using Bindings;

namespace ElecyServer
{
    class HandleDataUDP
    {
        private delegate void Packet_(GamePlayerUDP player, byte[] data);
        private static Dictionary<int, Packet_> Packets;
        public static void InitializeNetworkPackages()
        {
            Packets = new Dictionary<int, Packet_>
            {
                {(int)UDPRoomPackets.URConnectionComplite, HandleConnectionComplite },
            };
        }

        public static void HandleNetworkInformation(GamePlayerUDP player, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            int packetnum = buffer.ReadInteger();
            buffer.Dispose();
            if (Packets.TryGetValue(packetnum, out Packet_ Packet))
            {
                Packet.Invoke(player, data);
            }
        }


        /// <summary>
        /// Buffer: 
        ///         int PacketNum;
        ///         int player's ID (1 or 2);
        ///         int roomIndex;
        /// </summary>
        private static void HandleConnectionComplite(GamePlayerUDP player, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            player.SetValues(buffer.ReadInteger(), Global.arena[buffer.ReadInteger()]);
            buffer.Dispose();
        }
    }
}
