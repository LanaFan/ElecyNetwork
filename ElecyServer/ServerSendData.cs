using System;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    class ServerSendData
    {
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
            buffer.WriteString("Registration complite.");
            ServerTCP.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLoginOk(int index, string username)
        {
            Console.WriteLine("I'm sending LogOk");
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SLoginOK);
            buffer.WriteString(username);

            ServerTCP.SendDataTo(index, buffer.ToArray());
            Console.WriteLine(buffer.ReadInteger() + " || " + buffer.ReadString());
            buffer.Dispose();
        }
    }
}
