using System;
using System.Collections.Generic;
using System.Net;
using Bindings;

namespace ElecyServer
{
    class HandleDataUDP
    {
        private delegate void Packet_(GamePlayerUDP player, byte[] data);
        private static Dictionary<int, Packet_> Packets;
        public static void InitializeNetworkPackages()
        {
            Packets = new Dictionary<int, Packet_>
            {
                {(int)UDPRoomPackets.URConnectionComplite, HandleConnectionComplite },
                {(int)UDPRoomPackets.URPositionUpdate, HandlePositionUpdate },
                {(int)UDPRoomPackets.URPositionStepback, HandlePositionStepback },
                {(int)UDPRoomPackets.URRotationUpdate, HandleRotationUpdate },
                {(int)UDPRoomPackets.URRotationStepback, HandleRotationStepback },
                {(int)UDPRoomPackets.URHealthUpdate, HandleHealthUpdate},
                {(int)UDPRoomPackets.URHealthStepback, HandleHealthStepBack},
                {(int)UDPRoomPackets.URSynergyUpdate, HandleSynergyUpdate},
                {(int)UDPRoomPackets.URSynergyStepback, HandleSynergyStepBack}
            };
        }

        public static void HandleNetworkInformation(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                int packetnum = buffer.ReadInteger();
                if (Packets.TryGetValue(packetnum, out Packet_ Packet))
                {
                    Packet.Invoke(player, data);
                }
            }
        }

        /// <summary>
        /// Buffer: 
        ///         int PacketNum;
        ///         int player's ID (1 or 2);
        ///         int roomIndex;
        /// </summary>
        private static void HandleConnectionComplite(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                player.Connected();
            }
        }

        private static void HandlePositionUpdate(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int UpdateIndex = buffer.ReadInteger();
                float[] pos = new float[3];
                switch (type)
                {
                    case ObjectType.player:
                        pos[0] = buffer.ReadFloat();
                        pos[1] = buffer.ReadFloat();
                        pos[2] = buffer.ReadFloat();
                        player.room.roomPlayers[player.ID].position.SetUpdate(pos, UpdateIndex);
                        break;

                    case ObjectType.spell:
                        pos[0] = buffer.ReadFloat();
                        pos[1] = buffer.ReadFloat();
                        pos[2] = buffer.ReadFloat();
                        try
                        {
                            player.room.dynamicObjectsList.Get(index).position.SetUpdate(pos, UpdateIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;

                    case ObjectType.staticObjects:
                        pos[0] = buffer.ReadFloat();
                        pos[1] = buffer.ReadFloat();
                        pos[2] = buffer.ReadFloat();
                        try
                        {
                            player.room.staticObjectsList.Get(index).position.SetUpdate(pos, UpdateIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;
                }
            }
        }

        private static void HandlePositionStepback(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int StepBackIndex = buffer.ReadInteger();
                switch(type)
                {
                    case ObjectType.player:
                        player.room.roomPlayers[player.ID].position.UdpateStepBack(StepBackIndex);
                        break;

                    case ObjectType.spell:
                        try
                        {
                            player.room.dynamicObjectsList.Get(index).position.UdpateStepBack(StepBackIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;

                    case ObjectType.staticObjects:
                        try
                        {
                            player.room.staticObjectsList.Get(index).position.UdpateStepBack(StepBackIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;
                }
            }
        }

        private static void HandleRotationUpdate(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int UpdateIndex = buffer.ReadInteger();
                float[] rot = new float[4];
                switch (type)
                {
                    case ObjectType.player:
                        rot[0] = buffer.ReadFloat();
                        rot[1] = buffer.ReadFloat();
                        rot[2] = buffer.ReadFloat();
                        rot[3] = buffer.ReadFloat();
                        player.room.roomPlayers[player.ID].rotation.SetUpdate(rot, UpdateIndex);
                        break;

                    case ObjectType.spell:
                        rot[0] = buffer.ReadFloat();
                        rot[1] = buffer.ReadFloat();
                        rot[2] = buffer.ReadFloat();
                        rot[3] = buffer.ReadFloat();
                        try
                        {
                            player.room.dynamicObjectsList.Get(index).rotation.SetUpdate(rot, UpdateIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;

                    case ObjectType.staticObjects:
                        rot[0] = buffer.ReadFloat();
                        rot[1] = buffer.ReadFloat();
                        rot[2] = buffer.ReadFloat();
                        rot[3] = buffer.ReadFloat();
                        try
                        {
                            player.room.staticObjectsList.Get(index).rotation.SetUpdate(rot, UpdateIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;
                }
            }
        }

        private static void HandleRotationStepback(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int StepBackIndex = buffer.ReadInteger();
                switch (type)
                {
                    case ObjectType.spell:
                        try
                        {
                            player.room.dynamicObjectsList.Get(index).rotation.UdpateStepBack(StepBackIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;

                    case ObjectType.staticObjects:
                        try
                        {
                            player.room.staticObjectsList.Get(index).rotation.UdpateStepBack(StepBackIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;
                }
            }
        }

        private static void HandleHealthUpdate(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int UpdateIndex = buffer.ReadInteger();
                int _health = buffer.ReadInteger();
                switch (type)
                {
                    case ObjectType.player:
                        player.room.roomPlayers[player.ID].healthPoints.SetUpdate(_health, UpdateIndex);
                        break;

                    case ObjectType.spell:
                        try
                        {
                            player.room.dynamicObjectsList.Get(index).healthPoints.SetUpdate(_health, UpdateIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;

                    case ObjectType.staticObjects:
                        try
                        {
                            player.room.staticObjectsList.Get(index).healthPoints.SetUpdate(_health, UpdateIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;
                }
            }
        }

        private static void HandleHealthStepBack(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int StepBackIndex = buffer.ReadInteger();
                switch (type)
                {
                    case ObjectType.player:
                        player.room.roomPlayers[player.ID].healthPoints.UdpateStepBack(StepBackIndex);
                        break;

                    case ObjectType.spell:
                        try
                        {
                            player.room.dynamicObjectsList.Get(index).healthPoints.UdpateStepBack(StepBackIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;

                    case ObjectType.staticObjects:
                        try
                        {
                            player.room.staticObjectsList.Get(index).healthPoints.UdpateStepBack(StepBackIndex);
                        }
                        catch (Exception ex)
                        {
                            if (ex is NullReferenceException || ex is IndexOutOfRangeException)
                                return;
                            Global.serverForm.Debug(ex + "");
                        }
                        break;
                }
            }
        }

        private static void HandleSynergyUpdate(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int UpdateIndex = buffer.ReadInteger();
                int _synergy = buffer.ReadInteger();
                switch (type)
                {
                    case ObjectType.player:
                        player.room.roomPlayers[player.ID].synergyPoints.SetUpdate(_synergy, UpdateIndex);
                        break;
                }
            }
        }

        private static void HandleSynergyStepBack(GamePlayerUDP player, byte[] data)
        {
            using (PacketBuffer buffer = new PacketBuffer())
            {
                buffer.WriteBytes(data);
                buffer.ReadInteger();
                ObjectType type = (ObjectType)buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int StepBackIndex = buffer.ReadInteger();
                switch (type)
                {
                    case ObjectType.player:
                        player.room.roomPlayers[player.ID].synergyPoints.UdpateStepBack(StepBackIndex);
                        break;
                }
            }
        }
    }
}
