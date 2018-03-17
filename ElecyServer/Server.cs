using System;
using System.Drawing;
using System.Windows.Forms;

namespace ElecyServer
{
    public partial class Server : Form
    {
        delegate void StringArgReturningVoidDelegate(string text);
        Point lastPoint;

        public Server()
        {
            InitializeComponent();
            ptrGreen.Hide();
            ptrYellow.Hide();
            lblID.Hide();
        }   

        public void AddClient(string ip)
        { 
            if (listClients.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(AddClient);
                Invoke(d, new object[] { ip });
            }
            else
            {
                listClients.Items.Add(ip);
            }
        }

        public void AddNetPlayer(NetPlayer player)
        {
            RemoveClient(player.ip);
            AddNetPlayer(player.nickname);
        }

        public void Debug(string msg)
        {
            if (textBoxDebug.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(Debug);
                Invoke(d, new object[] { msg });
            }
            else
            {
                textBoxDebug.AppendText(msg + "\n");
            }

        }

        public void ShowChatMsg(string nickname, string msg)
        {
            ShowChatMsg(nickname + ": " + msg);
        }

        private void ShowChatMsg(string text)
        {
            if (txtBoxChat.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(ShowChatMsg);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtBoxChat.AppendText(text + "\n");
            }
        }

        private void RemoveClient(string ip)
        {
            if (listClients.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(RemoveClient);
                Invoke(d, new object[] { ip });
            }
            else
            {
                listClients.Items.Remove(ip);
            }
        }

        private void AddNetPlayer(string nick)
        {
            if (listNetPlayers.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(AddNetPlayer);
                Invoke(d, new object[] { nick });
            }
            else
            {
                listNetPlayers.Items.Add(nick);
            }
        }

        #region Elements

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (ServerTCP.isClosed())
            { 
                textBoxDebug.Clear();
                ServerHandleNetworkData.InitializeNetworkPackages();
                ServerHandleRoomData.InitializeNetworkPackages();
                Global.mysql.MySQLInit();
                ServerTCP.SetupServer();
                lblID.Show();
                lblID.Text = "Server ip: " + ServerTCP.GetLocalIPAddress();
                ptrRed.Hide();
                ptrGreen.Show();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        { 
            ServerTCP.ServerClose();
            lblID.Hide();
            ptrGreen.Hide();
            ptrRed.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!ServerTCP.isClosed())
            {
                ServerTCP.ServerClose();
                ptrGreen.Hide();
                ptrRed.Show();
                lblID.Hide();
                textBoxDebug.Clear();
                ServerHandleNetworkData.InitializeNetworkPackages();
                ServerHandleRoomData.InitializeNetworkPackages();
                Global.mysql.MySQLInit();
                ServerTCP.SetupServer();
                lblID.Show();
                lblID.Text = "Server ip: " + ServerTCP.GetLocalIPAddress();
                ptrRed.Hide();
                ptrGreen.Show();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            ServerTCP.ServerClose();
            Application.Exit();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!ServerTCP.isClosed())
            {
                string msg = txtChat.Text;
                txtChat.Text = "";
                if (msg != "")
                {
                    ServerSendData.SendGlChatMsg("Server", msg);
                    ShowChatMsg("Server: " + msg);
                }
            }
        }

        private void txtChat_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue.Equals(13))
            {
                if (!ServerTCP.isClosed())
                {
                    string msg = txtChat.Text;
                    txtChat.Text = "";
                    if (msg != "")
                    {
                        ServerSendData.SendGlChatMsg("Server", msg);
                        ShowChatMsg("Server: " + msg);
                    }
                }
            }
        }

        private void Server_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void Server_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxDebug.Clear();
        }

        private void listClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Client client in Global.clients)
            {
                if (client.ip == listClients.SelectedItem.ToString())
                {
                    listData.Items.Clear();
                    listData.Items.Add("index: " + client.index);
                    listData.Items.Add("ip: " + client.ip);
                    break;
                }
            }
        }

        private void listNetPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(NetPlayer player in Global.players)
            {
                if(player.nickname == listNetPlayers.SelectedItem.ToString())
                {
                    listData.Items.Clear();
                    listData.Items.Add("index: " + player.index);
                    listData.Items.Add("roomIndex: " + player.roomIndex);
                    listData.Items.Add("ip: " + player.ip);
                    listData.Items.Add("nickname: " + player.nickname);
                    listData.Items.Add("closing: " + player.playerClosing);
                    listData.Items.Add("state: " + player.state);
                    listData.Items.Add("Ignis level: " + player.level[0]);
                    listData.Items.Add("Terra level: " + player.level[1]);
                    listData.Items.Add("Caeli level: " + player.level[2]);
                    listData.Items.Add("Aqua level: " + player.level[3]);
                    listData.Items.Add("Primus level: " + player.level[4]);
                    listData.Items.Add("Ignis rank: " + player.rank[0]);
                    listData.Items.Add("Terra rank: " + player.rank[1]);
                    listData.Items.Add("Caeli rank: " + player.rank[2]);
                    listData.Items.Add("Aqua rank: " + player.rank[3]);
                    listData.Items.Add("Primus rank: " + player.rank[4]);
                    break;
                }
            }
        }

        #endregion




    }
        
}
