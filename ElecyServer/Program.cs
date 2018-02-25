using System;

namespace ElecyServer
{
    class Program
    {

        public static void Main(string[] args)
        {
            ServerHandleNetworkData.InitializeNetworkPackages();
            Global.mysql.MySQLInit();
            ServerTCP.SetupServer();
            Console.ReadLine();
        }
    }
}
