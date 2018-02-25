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
            Global.data.AddAccount("Vados", "Pizdos", "Zasos");
            Global.data.GetAccountNickname("Vados");
            Console.ReadLine();
        }
    }
}
