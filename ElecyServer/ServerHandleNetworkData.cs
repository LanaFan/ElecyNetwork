using System;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    class ServerHandleNetworkData
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Packets = new Dictionary<int, Packet_>
            {
                {(int)ClientPackets.CConnectcomplite, HandleConnect },
                {(int)ClientPackets.CRegisterTry, HandleRegisterTry },
                {(int)ClientPackets.CLoginTry, HandleLoginTry }
            };
        }

        private static void HandleConnect(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            Console.WriteLine(msg);
        }

        private static void HandleRegisterTry(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            if(Global.data.AccountExist(index,username))
            {
                return;
            }
            Global.data.AddAccount(username, password);
            buffer.Dispose();
            Console.WriteLine("Player: " + username + " succesfully registered.");
            ServerSendData.SendRegisterOk(index);
        }

        private static void HandleLoginTry(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            //if(!Global.data.AccountExist(index, username))
            //{
            //    //ServerSendData.SendAlert(index, "Invalid account name.");
            //    //ServerSendData.SendAlert(index, "Invalid account name.");
            //    return;
            //}
            //if(!Global.data.PasswordIsOkay(index, username, password))
            //{
            //    //ServerSendData.SendAlert(index, "Invalid password.");
            //    return;
            //}
            buffer.Dispose();
            ServerSendData.SendLoginOk(index);
            Console.WriteLine("Player: " + username + " logged in succesfully");
            //ServerSendData.SendLoginOk(index, username);
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
                Packet.Invoke(index,data);
            }
        }
    }
}
