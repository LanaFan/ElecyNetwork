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

        public static void SendTransformUpdate(GamePlayerUDP player, byte ObjectType, int ObjectIndex, float[] Position, int UpdateIndex)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)UDPServerPackets.USTransformUpdate);
            buffer.WriteByte(ObjectType);
            buffer.WriteInteger(ObjectIndex);
            buffer.WriteInteger(UpdateIndex);
            buffer.WriteFloat(Position[0]);
            buffer.WriteFloat(Position[1]);
            UDPConnector.SendToBothClient(player, buffer.ToArray());
            buffer.Dispose();
        }
    }
}
