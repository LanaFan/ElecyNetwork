namespace Bindings
{
    //get send from server to client
    public enum ServerPackets
    {
        SConnectionOK = 1,
        SRegisterOK = 2,
        SLoginOK = 3,
        SAlert = 4,
        SGlChatMsg = 5,
        SQueueStarted = 6,
        SQueueContinue = 7,
        SMatchFound = 8,
        SLoadStarted = 9,
        SRockSpawn = 10,
        SRoomStart = 11,
        STransform = 12,
        STreeSpawn = 13,
        SEnemyLoadProgress = 14,
        SInstantiate = 15,
        //SClientExit = 16,
        //SNetPlayerExit = 17,
        //SPlayerExit = 18,
        SNetPlayerLogOut = 19,
        SPlayerLogOut = 20,
        SMatchResult = 21,
        SSpellLoad = 22,
        SBuildInfo = 23,
        SBuildSaved = 24,
    }


    //get send from client to server
    public enum ClientPackets
    {
        CConnectComplite = 1,
        CRegisterTry = 2,
        CLoginTry = 3,
        CAlert = 4,
        CClose = 5,
        CReconnectComplite = 6,
    }

    //get send from player to server
    public enum NetPlayerPackets
    {
        PConnectionComplite = 7,
        PGlChatMsg = 8,
        PQueueStart = 9,
        PSearch = 10,
        PQueueStop = 11,
        PAlert = 12,
        PStopPlayer = 17,
        //PLogOut = 23,
        PGetSkillsBuild = 26,
        PSaveSkillsBuild = 27
    }

    public enum RoomPackets
    {
        RConnectionComplite = 13,
        RPlayerSpawned = 14,
        RLoadComplite = 15,
        //RTransform = 16,
        RRockSpawned = 18,
        RTreesSpawned = 19,
        RInstantiate = 21,
        RSurrender = 24,
        RRoomLeave = 25,
    }
}
