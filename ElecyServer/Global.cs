using Bindings;

namespace ElecyServer
{
    class Global
    {
        public static MySQL mysql = new MySQL();
        public static Database data = new Database();
        public static Client[] _clients = new Client[Constants.MAX_PLAYERS];
        public static NetPlayer[] _players = new NetPlayer[Constants.MAX_PLAYERS];
    }
}
