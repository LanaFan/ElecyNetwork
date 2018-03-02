using System;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    class ServerSendData
    {
        public static void SendConnetionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            buffer.WriteString("Connection to server with IP '" + ServerTCP.GetLocalIPAddress() + "' succesfull.");
            ServerTCP.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendAlert(int index, string message)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SAlert);
            buffer.WriteString(message);
            ServerTCP.SendData(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRegisterOk(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRegisterOK);
            buffer.WriteString("Registration complite.");
            ServerTCP.SendData(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLoginOk(int index, string nickname, int[][]accountdata)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SLoginOK);
            buffer.WriteString(nickname);
            buffer.WriteInteger(accountdata[0][0]);
            buffer.WriteInteger(accountdata[0][1]);
            buffer.WriteInteger(accountdata[0][2]);
            buffer.WriteInteger(accountdata[0][3]);
            buffer.WriteInteger(accountdata[0][4]);
            buffer.WriteInteger(accountdata[1][0]);
            buffer.WriteInteger(accountdata[1][1]);
            buffer.WriteInteger(accountdata[1][2]);
            buffer.WriteInteger(accountdata[1][3]);
            buffer.WriteInteger(accountdata[1][4]);
            ServerTCP.SendData(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendGlChatMsg(int index, string Nickname, string GlChatMsg)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SGlChatMsg);
            buffer.WriteString(Nickname);
            buffer.WriteString(GlChatMsg);
            Console.WriteLine("In send");
            Console.WriteLine(buffer.ReadInteger() + " - packet num; " + buffer.ReadString() + " - nickname; " + buffer.ReadString() + " - msg; ");
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            Console.WriteLine("Msg sended");
            buffer.Dispose();
        }
    }
}
