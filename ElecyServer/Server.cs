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
            AddNetPlayer(player.Nickname);
        }

        public void AddGameRoom(string roomIndex)
        {
            if (listClients.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(AddGameRoom);
                Invoke(d, new object[] { roomIndex });
            }
            else
            {
                listGameRooms.Items.Add(roomIndex);
            }
        }

        public void RemoveClient(string ip)
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

        public void RemoveNetPlayer(string nick)
        {
            if(listNetPlayers.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(RemoveNetPlayer);
                Invoke(d, new object[] { nick });
            }
            else
            {
                listClients.Items.Remove(nick);
            }
        }

        public void RemoveGameRoom(string roomIndex)
        {
            if (listClients.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(RemoveGameRoom);
                Invoke(d, new object[] { roomIndex });
            }
            else
            {
                listGameRooms.SetSelected(listGameRooms.Items.IndexOf(roomIndex), false);
                listGameRooms.Items.Remove(roomIndex);
            }
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

        private void StartServer()
        {
            textBoxDebug.Clear();
            ServerHandleClientData.InitializeNetworkPackages();
            ServerHandlePlayerData.InitializeNetworkPackages();
            ServerHandleRoomData.InitializeNetworkPackages();
            Global.mysql.MySQLInit();
            ServerTCP.SetupServer();
            lblID.Show();
            lblID.Text = "Server ip: " + ServerTCP.ServerIP;
            ptrRed.Hide();
            ptrGreen.Show();
        }

        private void StopServer()
        {
            ServerTCP.ServerClose();
            lblID.Hide();
            ptrGreen.Hide();
            ptrRed.Show();
            listClients.Items.Clear();
            listNetPlayers.Items.Clear();
            listGameRooms.Items.Clear();
            listData.Items.Clear();
            txtBoxChat.Clear();
        }

        #region Elements

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (ServerTCP.Closed)
            { 
                StartServer();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!ServerTCP.Closed)
            {
                StopServer();
                StartServer();
                Debug("Server has been refreshed");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            ServerTCP.ServerClose();
            Application.Exit();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!ServerTCP.Closed)
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
                if (!ServerTCP.Closed)
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
                try
                {
                    if (client.IP == listClients.SelectedItem.ToString())
                    {
                        listData.Items.Clear();
                        listData.Items.Add("index: " + client.Index);
                        listData.Items.Add("ip: " + client.IP);
                        break;
                    }
                }
                catch (NullReferenceException) { }
            }
        }

        private void listNetPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(NetPlayer player in Global.players)
            {
                try
                {
                    if (player.Nickname == listNetPlayers.SelectedItem.ToString())
                    {
                        listData.Items.Clear();
                        listData.Items.Add("index: " + player.Index);
                        listData.Items.Add("roomIndex: " + player.RoomIndex);
                        listData.Items.Add("ip: " + player.IP);
                        listData.Items.Add("nickname: " + player.Nickname);
                        listData.Items.Add("closing: " + player.Stopped);
                        listData.Items.Add("state: " + player.State);
                        listData.Items.Add("Ignis level: " + player.levels[0]);
                        listData.Items.Add("Terra level: " + player.levels[1]);
                        listData.Items.Add("Caeli level: " + player.levels[2]);
                        listData.Items.Add("Aqua level: " + player.levels[3]);
                        listData.Items.Add("Primus level: " + player.levels[4]);
                        listData.Items.Add("Ignis rank: " + player.ranks[0]);
                        listData.Items.Add("Terra rank: " + player.ranks[1]);
                        listData.Items.Add("Caeli rank: " + player.ranks[2]);
                        listData.Items.Add("Aqua rank: " + player.ranks[3]);
                        listData.Items.Add("Primus rank: " + player.ranks[4]);
                        break;
                    }
                }
                catch (NullReferenceException) { }
            }
        }

        private void listGameRooms_SelectedIndexChanged(object sender, EventArgs e) // Edit!!!
        {
            foreach(GameRoom room in Global.arena)
            {
                try
                {
                    if ((room.RoomIndex + "") == listGameRooms.SelectedItem.ToString())
                    {
                        listData.Items.Clear();
                        listData.Items.Add("roomIndex: " + room.RoomIndex);
                        if (room.GetPlayer(1) != null)
                            listData.Items.Add("player1: " + room.GetPlayer(1).Nickname);
                        if (room.GetPlayer(2) != null)
                            listData.Items.Add("player2: " + room.GetPlayer(2).Nickname);
                        listData.Items.Add("size: " + room.Size);
                        listData.Items.Add("status: " + room.Status);
                        break;
                    }
                }
                catch (NullReferenceException) { }
            }
        }

        #endregion
    }
        
}
