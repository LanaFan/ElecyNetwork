namespace Bindings
{
    public class Constants
    {
        public const int MAX_PLAYERS = 100; //Maximum number of players on the server

        public const int MAX_CLIENTS = 100; //Maximum number of clients on the server

        public const int RACES_NUMBER = 5; //Amount of races(levels, ranks)

        public const int PORT = 24985; //Port that server and clients uses

        public const int UDP_PORT = 24986;

        public const int SERVER_LISTEN = 10; //Number of clients that server can listen

        public const int TCP_BUFFER_SIZE = 1024; //Size of buffers (player, client, server)

        public const int UDP_BUFFER_SIZE = 1024;

        public const int ARENA_SIZE = 60;

        public const int UPDATE_RATE = 40;

        public const int MAPS_COUNT = 1;

        #region DataBase

        public const string FIRST_RACE_NAME = "Ignis";

        public const string SECOND_RACE_NAME = "Terra";

        public const string THIRD_RACE_NAME = "Caeli";

        public const string FOURTH_RACE_NAME = "Aqua";

        public enum SPELLCOUNT
        {
            Ignis = 9,
            Terra = 0,
            Caeli = 0,
            Aqua = 0
        }

        #endregion


        #region GameObjects

        public const int bRockHP = 40;
        public const int mRockHP = 30;
        public const int sRockHP = 20;
        public const int rockDiff = 10;

        public const int bTreeHP = 40;
        public const int mTreeHP = 30;
        public const int sTreeHP = 20;
        public const int treeDiff = 10;

        #endregion

    }
}
