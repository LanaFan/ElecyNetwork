using System;
using System.Windows.Forms;

namespace ElecyServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Global.serverForm = new Server();
            Application.Run(Global.serverForm);
        }

        public static void ServerStart()
        {
            ServerHandleClientData.InitializeNetworkPackages();
            ServerHandlePlayerData.InitializeNetworkPackages();
            ServerHandleRoomData.InitializeNetworkPackages();
            Global.mysql.MySQLInit();
            Global.data.InitDatabase();
            ServerTCP.SetupServer();
        }
    }
}
