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
            ServerTCP.SendData(index, buffer.ToArray());
            buffer.Dispose();
            Console.WriteLine("Send alert!");
        }

        public static void SendRegisterOk(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRegisterOK);
            buffer.WriteString("Registration complite.");
            ServerTCP.SendData(index, buffer.ToArray());
            Console.WriteLine("RegisterOK sended");
            buffer.Dispose();
        }

        public static void SendLoginOk(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SLoginOK);
            buffer.WriteString("Name");
            ServerTCP.SendData(index, buffer.ToArray());
            Console.WriteLine("LoginOK sended");
            buffer.Dispose();
        }
    }
}
