using System;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    public static class ServerHandleRoomData
    {
        private delegate void Packet_(int ID, byte[] data);
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
                {(int)RoomPackets.RLoadProgress, HandleLoadProgress },
                {(int)RoomPackets.RInstantiate, HandleInstantiate},
                {(int)RoomPackets.RSurrender, HandleSurrender },
                {(int)RoomPackets.RRoomLeave, HandleRoomLeave },
                {(int)SystemPackets.SysExit, HandleRoomExit }
            };
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
                Packet.Invoke(index, data);
            }
        }

        private static void HandleRoomConnect(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            buffer.Dispose();
            Global.arena[roomIndex].SetGameLoadData(ID);
        }

        private static void HandlePlayerSpawn(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            buffer.Dispose();
            Global.arena[roomIndex].SpawnRock(ID);
        }

        //Gets roomIndex
        private static void HandleRockSpawned(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            Global.arena[roomIndex].SpawnTree(ID);
        }

        private static void HandleComplete(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            buffer.Dispose();
            Global.arena[roomIndex].LoadComplete(ID);
        }

        private static void HandleInstantiate(int ID, byte[] data)
        {
            float[] pos = new float[3];
            float[] rot = new float[4];
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
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

        private static void HandleTransform(int ID, byte[] data)
        {
            float[] pos = new float[3];
            float[] rot = new float[4];
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            pos[0] = buffer.ReadFloat();
            pos[1] = buffer.ReadFloat();
            pos[2] = buffer.ReadFloat();
            rot[0] = buffer.ReadFloat();
            rot[1] = buffer.ReadFloat();
            rot[2] = buffer.ReadFloat();
            rot[3] = buffer.ReadFloat();
            buffer.Dispose();
            Global.arena[roomIndex].SetTransform(ID, pos, rot);
        }

        private static void HandleLoadProgress(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            float loadProgress = buffer.ReadFloat();
            Global.arena[roomIndex].SetLoadProgress(ID, loadProgress);
        }

        private static void HandleRoomExit(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            ServerSendData.SendRoomExit(ID, roomIndex);
            Global.arena[roomIndex].AbortGameSession(ID);
        }

        private static void HandleSurrender(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            buffer.Dispose();
            Global.arena[roomIndex].Surrended(ID);
        }

        private static void HandleRoomLeave(int ID, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            int roomIndex = buffer.ReadInteger();
            buffer.Dispose();
            ServerSendData.SendRoomLogOut(ID, roomIndex);
            Global.arena[roomIndex].BackToNetPlayer(ID);
        }
    }
}
