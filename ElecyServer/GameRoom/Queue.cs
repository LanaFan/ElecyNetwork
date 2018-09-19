using Bindings;

namespace ElecyServer
{
    public static class Queue
    {

        public static void StartSearch(ClientTCP client, int matchType, string race)
        {
            switch (matchType)
            {
                case 0:
                    client.playerState = NetPlayerState.SearchingForMatch;
                    client.race = race;
                    foreach(GameRoom room in Global.roomsList)
                    {
                        if (room.Status == RoomState.Searching)
                        {
                            room.AddPlayer(client);
                            return;
                        }
                    }
                    Global.roomsList.Add(new GameRoom(client));
                    break;
            }
        }

        public static void StopSearch(ClientTCP client)
        {
            if(client.room != null)
                client.room.DeletePlayer(client);
            client.playerState = NetPlayerState.InMainLobby;
        }

    }
}
