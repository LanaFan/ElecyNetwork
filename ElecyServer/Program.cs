using System;
using System.Windows.Forms;
using System.Linq;

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
                Global.data.AddAccount("1", "1", "Tarkes");
                Global.data.AddAccount("2", "2", "Ludaris");
                Global.data.AddMap(1, 8, 8, new float[] { -34.5f, 0 }, new float[] { 34.5f, 0 }, new float[] { 0, 90f, 0, 1 }, new float[] { 0, -90f, 0, 1 });
                Global.serverForm.StatusIndicator(1);
            }
            catch (Exception ex)
            {
                Global.serverForm.StatusIndicator(1, ex);
            }
            ServerTCP.SetupServer();
        }
    }
}
