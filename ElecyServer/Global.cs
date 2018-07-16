using Bindings;
using System.Collections.Generic;
using System.Threading;

namespace ElecyServer
{
    class Global
    {
        public static MySQL mysql;
        public static Database data;
        public static GameRoom[] arena = new GameRoom[Constants.ARENA_SIZE];
        public static List<GamePlayerUDP> playersUDP;
        public static List<ClientTCP> clientList; 
        public static List<GameRoom> roomsList;

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
            mysql = new MySQL();
            data = new Database();
            clientList = new List<ClientTCP>();
            roomsList = new List<GameRoom>();
            playersUDP = new List<GamePlayerUDP>();
        }

        public static void FinalGlobals()
        {
            mysql = null;
            data = null;
            foreach(ClientTCP client in clientList.ToArray())
            {
                client.Close();
            }
            clientList = null;
            foreach (GameRoom room in roomsList.ToArray())
            {
                room.CloseRoom();
            }
            roomsList = null;
            foreach (GamePlayerUDP player in playersUDP.ToArray())
            {
                playersUDP.Remove(player);
            }
            playersUDP = null;

        }
    }
}
