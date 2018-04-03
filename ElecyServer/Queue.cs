using Bindings;

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
                        if (Global.arena[i].Status == GameRoom.RoomStatus.Searching)
                        {
                            Global.arena[i].AddPlayer(Global.players[index]);
                            Global.players[index].state = NetPlayer.playerState.SearchingForMatch;
                            Global.players[index].roomIndex = i;
                            return true;
                        }
                    }
                    for(int i = 0; i < Constants.ARENA_SIZE; i++)
                    {
                        if(Global.arena[i].Status == GameRoom.RoomStatus.Empty)
                        {
                            Global.arena[i].AddPlayer(Global.players[index]);
                            Global.players[index].state = NetPlayer.playerState.SearchingForMatch;
                            Global.players[index].roomIndex = i;
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public static void StopSearch(int index, int roomIndex)
        {
            Global.arena[roomIndex].DeletePlayer(index);
        }

    }
}
