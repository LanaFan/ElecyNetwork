using System;
using System.Windows.Forms;

namespace ElecyServer
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Global.serverForm = new Server();
            Application.Run(Global.serverForm);
        }

        public static void ServerStart()
        {
            try
            {
                HandleDataTCP.InitializeNetworkPackages();
                Global.InitGlobals();
                Global.serverForm.StatusIndicator(1);
            }
            catch (Exception ex)
            {
                Global.serverForm.StatusIndicator(1, ex);
            }
            Global.mysql.MySQLInit();
            Global.data.InitDatabase();
            ServerTCP.SetupServer();
        }
    }
}
