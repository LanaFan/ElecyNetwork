﻿namespace Bindings
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
        SDamage,
        SFriendsInfo,
        SFriendLeave,
        SFriendInfo,
        SFriendChange
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
        PAddFriend,
        PGlChatMsg,
        PPrivateMsg,
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
        RDamage,
    }

    #endregion

    #region UDP

    public enum UDPRoomPackets
    {
        URConnectionComplite = 1,
        URPositionUpdate,
        URPositionStepback,
        URRotationUpdate,
        URRotationStepback,
        URHealthUpdate,
        URHealthStepback,
        URSynergyUpdate,
        URSynergyStepback,
    }

    public enum UDPServerPackets
    {
        USConnectionOK = 1,
        USPositionUpdate,
        USRotationUpdate,
        USHealthUpdate,
        USSynergyUpdate
    }

    #endregion
}
