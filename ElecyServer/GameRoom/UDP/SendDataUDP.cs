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
        }
    }
}
