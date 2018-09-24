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

        public static void SendPositonUpdate(BaseGameRoom room, ObjectType type, int ObjectIndex, float[] Position, int UpdateIndex)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteInteger((int)UDPServerPackets.USPositionUpdate);
                buffer.WriteInteger((int)type);
                buffer.WriteInteger(ObjectIndex);
                buffer.WriteInteger(UpdateIndex);
                buffer.WriteFloat(Position[0]);
                buffer.WriteFloat(Position[1]);
                buffer.WriteFloat(Position[2]);
                UDPConnector.SendToRoomPlayers(room, buffer.ToArray());
            }
        }

        public static void SendRotationUpdate(BaseGameRoom room, ObjectType type, int ObjectIndex, float[] Rotation, int UpdateIndex)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteInteger((int)UDPServerPackets.USRotationUpdate);
                buffer.WriteInteger((int)type);
                buffer.WriteInteger(ObjectIndex);
                buffer.WriteInteger(UpdateIndex);
                buffer.WriteFloat(Rotation[0]);
                buffer.WriteFloat(Rotation[1]);
                buffer.WriteFloat(Rotation[2]);
                buffer.WriteFloat(Rotation[3]);
                UDPConnector.SendToRoomPlayers(room, buffer.ToArray());
            }
        }

        public static void SendHealthUpdate(BaseGameRoom room, ObjectType type, int ObjectIndex, int Health, int UpdateIndex)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteInteger((int)UDPServerPackets.USHealthUpdate);
                buffer.WriteInteger((int)type);
                buffer.WriteInteger(ObjectIndex);
                buffer.WriteInteger(UpdateIndex);
                buffer.WriteInteger(Health);
                UDPConnector.SendToRoomPlayers(room, buffer.ToArray());
            }
        }

        public static void SendSynergyUpdate(BaseGameRoom room, ObjectType type, int ObjectIndex, int Synergy, int UpdateIndex)
        {
            using(PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteInteger((int)UDPServerPackets.USSynergyUpdate);
                buffer.WriteInteger((int)type);
                buffer.WriteInteger(ObjectIndex);
                buffer.WriteInteger(UpdateIndex);
                buffer.WriteInteger(Synergy);
                UDPConnector.SendToRoomPlayers(room, buffer.ToArray());
            }
        }
    }
}
