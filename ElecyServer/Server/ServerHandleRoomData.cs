using System;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    public static class ServerHandleRoomData
    {
        private delegate void Packet_(int ID, GameRoom room, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Packets = new Dictionary<int, Packet_>
            {
                {(int)RoomPackets.RConnectionComplite, HandleRoomConnect },
                {(int)RoomPackets.RPlayerSpawned, HandlePlayerSpawn },
                {(int)RoomPackets.RLoadComplite, HandleComplete },
                {(int)RoomPackets.RRockSpawned, HandleRockSpawned },
                {(int)RoomPackets.RTransform, HandleTransform },
                {(int)RoomPackets.RInstantiate, HandleInstantiate},
                {(int)RoomPackets.RSurrender, HandleSurrender },
                {(int)RoomPackets.RRoomLeave, HandleRoomLeave },
            };
        }

        public static void HandleNetworkInformation(int index, GameRoom room, byte[] data)
        {
            int packetnum;
            PacketBuffer buffer = new PacketBuffer();
            Packet_ Packet;
            buffer.WriteBytes(data);
            packetnum = buffer.ReadInteger();
            buffer.Dispose();
            if (Packets.TryGetValue(packetnum, out Packet))
            {
                Packet.Invoke(index, room, data);
            }
        }

        private static void HandleRoomConnect(int ID, GameRoom Room, byte[] data)
        {
            Room.SetGameLoadData(ID);
        }

        private static void HandlePlayerSpawn(int ID, GameRoom Room, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            float loadProgress = buffer.ReadFloat();
            buffer.Dispose();
            Room.SetLoadProgress(ID, loadProgress);
            Room.SpawnRock(ID);
        }

        private static void HandleRockSpawned(int ID, GameRoom Room, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            float loadProgress = buffer.ReadFloat();
            buffer.Dispose();
            Room.SetLoadProgress(ID, loadProgress);
            Room.SpawnTree(ID);
        }

        private static void HandleComplete(int ID, GameRoom Room, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            float loadProgress = buffer.ReadFloat();
            buffer.Dispose();
            Room.SetLoadProgress(ID, loadProgress);
            Room.LoadComplete(ID);
        }

        private static void HandleInstantiate(int ID, GameRoom Room, byte[] data)
        {
            float[] pos = new float[3];
            float[] rot = new float[4];
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int objId = buffer.ReadInteger();
            int instanseType = buffer.ReadInteger();
            string objectReference = buffer.ReadString();
            pos[0] = buffer.ReadFloat();
            pos[1] = buffer.ReadFloat();
            pos[2] = buffer.ReadFloat();
            rot[0] = buffer.ReadFloat();
            rot[1] = buffer.ReadFloat();
            rot[2] = buffer.ReadFloat();
            rot[3] = buffer.ReadFloat();
            buffer.Dispose();
            //adding the object to array for start to observe
        }

        private static void HandleTransform(int ID, GameRoom Room, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            float[] pos = new float[] { buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat()};
            float[] rot = new float[] { buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat()};
            buffer.Dispose();
            Room.SetTransform(ID, pos, rot);
        }

        private static void HandleSurrender(int ID, GameRoom Room, byte[] data)
        {
            Room.Surrended(ID);
        }

        private static void HandleRoomLeave(int ID, GameRoom Room, byte[] data)
        {
            ServerSendData.SendRoomLogOut(ID, Room.RoomIndex);
            Room.BackToNetPlayer(ID);
        }

    }
}
