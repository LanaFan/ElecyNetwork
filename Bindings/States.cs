namespace Bindings
{

    public enum NetPlayerState
    {
        InMainLobby = 1,
        SearchingForMatch = 2,
        Playing = 3,
        EndPlaying = 4
    }

    public enum ClientTCPState
    {
        Sleep,
        Entrance,
        MainLobby,
        GameRoom,
    }

    public enum RoomState
    {
        Empty = 1,
        Searching = 2,
        MatchEnded = 3,
        Closed = 4
    }

}
