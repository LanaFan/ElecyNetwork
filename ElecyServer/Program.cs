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
    }
}
