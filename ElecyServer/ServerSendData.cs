using Bindings;

namespace ElecyServer
{
    class ServerSendData
    {
        #region Send to Client

        public static void SendClientConnetionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            ServerTCP.SendClientConnection(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendClientAlert(int index, string message)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SAlert);
            buffer.WriteString(message);
            ServerTCP.SendDataToClient(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRegisterOk(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SRegisterOK);
            buffer.WriteString("Registration complite.");
            ServerTCP.SendDataToClient(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLoginOk(int index, int playerIndex, string nickname, int[][]accountdata)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SLoginOK);
            buffer.WriteInteger(playerIndex);
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
            ServerTCP.SendDataToClient(index, buffer.ToArray());
            buffer.Dispose();
        }

        #endregion

        #region Send to Player

        public static void SendPlayerConnectionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            buffer.WriteString("Hello sexy guy...We've been waiting for you =*");
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendGlChatMsg(int index, string Nickname, string GlChatMsg)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SGlChatMsg);
            buffer.WriteString(Nickname);
            buffer.WriteString(GlChatMsg);
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendQueueStarted(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SQueueStarted);
            ServerTCP.SendDataToPlayer(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendMatchFound(int index1, int index2, int roomIndex)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SMatchFound);
            buffer.WriteInteger(roomIndex);
            ServerTCP.SendDataToPlayer(index1, buffer.ToArray());
            ServerTCP.SendDataToPlayer(index2, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendGameData(int index, int roomIndex)
        {
            string[] nicknames = Global.arena[roomIndex].GetNicknames();
            float[][][] transforms = Global.arena[roomIndex].GetTransforms();

        }
        #endregion
    }
}
