using Bindings;
using System.Collections.Generic;
using System.Threading;

namespace ElecyServer
{
    class Global
    {
        public static MySQL mysql = new MySQL();
        public static Database data = new Database();
        public static Client[] clients = new Client[Constants.MAX_PLAYERS];
        public static NetPlayer[] players = new NetPlayer[Constants.MAX_PLAYERS];
        public static GameRoom[] arena = new GameRoom[Constants.ARENA_SIZE];
        public static List<GamePlayerUDP> playersUDP = new List<GamePlayerUDP>();
        public static Server serverForm;

        #region Threads

        public static Thread dataTimerThread;

        #endregion

        public static void ThreadsStop()
        {
            dataTimerThread.Abort();
        }
    }
}
