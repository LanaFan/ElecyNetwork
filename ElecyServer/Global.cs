using Bindings;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ElecyServer
{
    class Global
    {
        public static Database data;
        public static GameRoom[] arena = new GameRoom[Constants.ARENA_SIZE];
        public static List<GamePlayerUDP> connectedPlayersUDP;
        public static List<GamePlayerUDP> unconectedPlayersUDP;
        public static List<ClientTCP> clientList; 
        public static List<BaseGameRoom> roomsList;
        public static List<GameRoom> roomsUDP;

        public static Server serverForm;

        #region Threads

        public static Thread dataTimerThread;

        #endregion

        public static void ThreadsStop()
        {
            dataTimerThread.Abort();
        }

        public static void InitGlobals()
        {
            data = new Database();
            clientList = new List<ClientTCP>();
            roomsList = new List<BaseGameRoom>();
            connectedPlayersUDP = new List<GamePlayerUDP>();
            unconectedPlayersUDP = new List<GamePlayerUDP>();
            roomsUDP = new List<GameRoom>();
        }

        public static void FinalGlobals()
        {
            data = null;
            foreach (ClientTCP client in clientList.ToArray())
            {
                try
                {
                    client.Close();
                }
                catch
                {

                }
            }
            clientList = null;
            foreach (GameRoom room in roomsList.ToArray())
            {
                room.CloseRoom();
            }
            roomsList = null;
            foreach (GamePlayerUDP player in connectedPlayersUDP.ToArray())
            {
                connectedPlayersUDP.Remove(player);
            }
            connectedPlayersUDP = null;
        }
    }
}
