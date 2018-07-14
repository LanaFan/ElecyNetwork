using Bindings;

namespace ElecyServer
{
    class SendDataUDP
    {

        /// <summary>
        /// Buffer:
        ///         int PacketNum;
        /// </summary>
        public static void SendConnectionOK(GamePlayerUDP player)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)UDPServerPackets.USConnectionOK);
            UDPConnector.Send(player, buffer.ToArray());
            buffer.Dispose();
            UDPConnector.PacketTest(player);
        }

        /// <summary>
        /// Test:
        ///      Buffer:
        ///             int PacketNum;
        ///             int number;
        /// </summary>
        public static void SendPacketTest(GamePlayerUDP player)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger(2);
            buffer.WriteInteger(1);
            UDPConnector.Send(player, buffer.ToArray());
            buffer.Dispose();
        }
    }
}
