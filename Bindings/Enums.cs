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
        Empty,
        Searching,
        Loading,
        Playing,
        MatchEnded,
        Closed,
    }

    public enum ObjectType
    {
        unsigned = 0,
        tree = 1,
        rock = 2,
        spell = 3,
    }

    public enum RoomTypes
    {
        GameRoom = 2,
        TestRoom = 3,
    }
}
