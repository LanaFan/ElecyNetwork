namespace Bindings
{
    public class Constants
    {
        public const int MAX_PLAYERS = 100; //Maximum number of players on the server

        public const int MAX_CLIENTS = 100; //Maximum number of clients on the server

        public const int RACES_NUMBER = 5; //Amount of races(levels, ranks)

        public const int PORT = 24985; //Port that server and clients uses

        public const int SERVER_LISTEN = 10; //Number of clients that server can listen

        public const int BUFFER_SIZE = 1024; //Size of buffers (player, client, server)

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
    }
}
