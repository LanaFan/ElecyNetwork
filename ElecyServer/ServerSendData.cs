using System;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    class ServerSendData
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void SendAlert(int index, string message)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SAlert);
            buffer.WriteString(message);
            ServerTCP.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendConnectionOk(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            ServerTCP.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRegisterOk(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRegisterOK);
            ServerTCP.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLoginOk(int index, string username)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SLoginOK);
            buffer.WriteString(username);
            ServerTCP.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }
    }
}
