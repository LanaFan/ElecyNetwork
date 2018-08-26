namespace Bindings
{

    #region ServerTCP

    public enum ServerPackets
    {
        SConnectionOK = 1,
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
        SInstantiate,
        SDestroy,
    }

    #endregion

    #region ClientTCP

    public enum ClientPackets
    {
        CConnectComplite = 1,
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
        PTestRoom,

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
        RInstantiate,
        RDestroy,
    }

    #endregion

    #region UDP

    public enum UDPRoomPackets
    {
        URConnectionComplite = 1,
        URTransformUpdate,
        URTransformStepback,
    }

    public enum UDPServerPackets
    {
        USConnectionOK = 1,
        USTransformUpdate,
    }

    #endregion

}
