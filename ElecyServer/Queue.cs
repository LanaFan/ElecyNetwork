using Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecyServer
{
    public static class Queue
    {
        public static bool StartSearch(int index, int matchType)
        {
            switch (matchType)
            {
                case 0:
                    for(int i = 0; i < Constants.ARENA_SIZE; i++)
                    {
                        if (Global.arena[i].GetStatus() == GameRoom.RoomStatus.Searching)
                        {
                            Global.arena[i].AddPlayer(Global.players[index]);
                            return true;
                        }
                    }
                    for(int i = 0; i < Constants.ARENA_SIZE; i++)
                    {
                        if(Global.arena[i].GetStatus() == GameRoom.RoomStatus.Empty)
                        {
                            Global.arena[i].AddPlayer(Global.players[index]);
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public static void StopSearch(int index)
        {
            for(int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if(Global.normalQueue[i] == index)
                {
                    Global.normalQueue[i] = 0;
                    break;
                }
            }
        }

        public static void StopSearch(int index1, int index2)
        {
            bool completed = false;
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (Global.normalQueue[i] == index1)
                {
                    Global.normalQueue[i] = 0;
                    if(completed)
                        break;
                    completed = true;
                }
                if (Global.normalQueue[i] == index2)
                {
                    Global.normalQueue[i] = 0;
                    if (completed)
                        break;
                    completed = true;
                }
            }
        }

        public static int SearchForEnemy(int index)
        {
            int index2 = -1;
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (Global.normalQueue[i] != 0 && Global.normalQueue[i] != index)
                {
                    if (Global.players[Global.normalQueue[i]].state != NetPlayer.playerState.Playing)
                    {
                        Global.players[Global.normalQueue[i]].state = NetPlayer.playerState.Playing;
                        Global.players[index].state = NetPlayer.playerState.Playing;
                        index2 = Global.normalQueue[i];
                        break;
                    }
                    break; // time
                }
            }
            return index2;
        }

        //public static int SearchForRoom(int index1, int index2)
        //{
        //    for (int j = 0; j < Constants.ARENA_SIZE; j++)
        //    {
        //        if (Global.arena[j] == null)
        //        {
        //            Global.players[index1].NetPlayerStop();
        //            Global.players[index2].NetPlayerStop();
        //            Global.arena[j] = new GameRoom(j, Global.players[index1], Global.players[index2]);
        //            StopSearch(index1, index2);
        //            return j;
        //        }
        //    }
        //    return -1;
        //}

    }
}
