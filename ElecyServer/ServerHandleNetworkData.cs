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
            Console.WriteLine("21");
            PacketBuffer buffer = new PacketBuffer();
            Console.WriteLine("22");
            buffer.WriteBytes(data);
            Console.WriteLine("23");
            buffer.ReadInteger();
            Console.WriteLine("24");
            string username = buffer.ReadString();
            Console.WriteLine("25");
            string password = buffer.ReadString();
            Console.WriteLine("26");
            string nickname = buffer.ReadString();
            Console.WriteLine("27");
            if (Global.data.AccountExist(index,username))
            {
                ServerSendData.SendAlert(index, "Username already exist");
                return;
            }
            Console.WriteLine("28");
            if (Global.data.NicknameExist(index, nickname))
            {
                ServerSendData.SendAlert(index, "Nickname already exist");
                return;
            }
            Console.WriteLine("29");
            Global.data.AddAccount(username, password, nickname);
            Console.WriteLine("210");
            buffer.Dispose();
            Console.WriteLine("211");
            ServerSendData.SendRegisterOk(index);
            Console.WriteLine("212");
            Console.WriteLine("Player: " + username + " succesfully registered.");
        }

        private static void HandleLoginTry(int index, byte[] data)
        {
            Console.WriteLine("31");
            PacketBuffer buffer = new PacketBuffer();
            Console.WriteLine("32");
            buffer.WriteBytes(data);
            Console.WriteLine("33");
            buffer.ReadInteger();
            Console.WriteLine("34");
            string username = buffer.ReadString();
            Console.WriteLine("35");
            string password = buffer.ReadString();
            Console.WriteLine("36");
            if (!Global.data.AccountExist(index, username))
            {
                ServerSendData.SendAlert(index, "Username does not exist.");
                return;
            }
            Console.WriteLine("37");
            if (!Global.data.PasswordIsOkay(index, username, password))
            {
                ServerSendData.SendAlert(index, "Invalid password.");
                return;
            }
            Console.WriteLine("38");
            buffer.Dispose();
            Console.WriteLine("39");
            string nickname = Global.data.GetAccountNickname(username);
            Console.WriteLine("310");
            int[][] accountdata = Global.data.GetAccountLevels(username);
            Console.WriteLine("311");
            ServerTCP.PlayerLogin(index, nickname, accountdata);
            Console.WriteLine("312");
            ServerSendData.SendLoginOk(index, nickname, accountdata);
            Console.WriteLine("Player: " + username + " logged in succesfully");
        }

        public static void HandleNetworkInformation(int index, byte[] data)
        {
            Console.WriteLine("11");
            int packetnum;
            Console.WriteLine("12");
            PacketBuffer buffer = new PacketBuffer();
            Console.WriteLine("13");
            Packet_ Packet;
            Console.WriteLine("14");
            buffer.WriteBytes(data);
            Console.WriteLine("15");
            packetnum = buffer.ReadInteger();
            Console.WriteLine("16");
            buffer.Dispose();
            Console.WriteLine("17");
            if (Packets.TryGetValue(packetnum, out Packet))
            {
                Console.WriteLine("18");
                Packet.Invoke(index,data);
            }
        }
    }
}
