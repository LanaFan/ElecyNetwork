namespace Bindings
{

    #region ServerTCP

    public enum ServerPackets
    {
        SConnectionOK,
        SRegisterOK,
        SLoginOK,
        SAlert,
        SGlChatMsg,
        SQueueStarted,
        SMatchFound,
        SMapLoad,
        SPlayerSpawned,
        SRockSpawned,
        STreeSpawned,
        SSpellLoaded,
        SRoomStart,
        SEnemyLoadProgress,
        SPlayerLogOut,
        SMatchResult,
        SBuildInfo,
        SBuildSaved,
    }

    #endregion

    #region ClientTCP

    public enum ClientPackets
    {
        CConnectComplite,
        CRegisterTry,
        CLoginTry,

        ClientPacketsNum // Number
    }

    public enum NetPlayerPackets
    {
        PConnectionComplite = ClientPackets.ClientPacketsNum,
        PGlChatMsg,
        PQueueStart,
        PQueueStop,
        PGetSkillsBuild,
        PSaveSkillsBuild,

        NetPlayerPacketsNum // Number
    }

    public enum RoomPackets
    {
        RConnectionComplite = NetPlayerPackets.NetPlayerPacketsNum,
        RGetPlayers,
        RGetRocks,
        RGetTrees,
        RGetSpells,
        RLoadComplite,
        RSurrender,
        RRoomLeave,
    }

    #endregion

    #region UDP

    public enum UDPRoomPackets
    {
        URConnectionComplite = 1,
    }

    public enum UDPServerPackets
    {
        USConnectionOK = 1,
    }

    #endregion

}
