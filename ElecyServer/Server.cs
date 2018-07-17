using System;
using System.Drawing;
using System.Windows.Forms;
using Bindings;

namespace ElecyServer
{
    public partial class Server : Form
    {
        delegate void StringArgReturningVoidDelegate<T>(T o);
        Point lastPoint;

        #region Exceptions

        Exception ex1;
        Exception ex2;
        Exception ex3;
        Exception ex4;

        string lastException;

        #endregion

        public Server()
        {
            InitializeComponent();
            HidePtr();
            btnStop.Hide();
            btnRefresh.Hide();
        }   

        public void AddClient(ClientTCP client)
        { 
            if (listClients.InvokeRequired)
            {
                StringArgReturningVoidDelegate<ClientTCP> d = new StringArgReturningVoidDelegate<ClientTCP>(AddClient);
                Invoke(d, new object[] { client });
            }
            else
            {
                listClients.Items.Add(client);
            }
        }

        public void AddGameRoom(GameRoom room)
        {
            if (listClients.InvokeRequired)
            {
                StringArgReturningVoidDelegate<GameRoom> d = new StringArgReturningVoidDelegate<GameRoom>(AddGameRoom);
                Invoke(d, new object[] { room });
            }
            else
            {
                listGameRooms.Items.Add(room);
            }
        }

        public void RemoveClient(ClientTCP client)
        {
            if (listClients.InvokeRequired)
            {
                StringArgReturningVoidDelegate<ClientTCP> d = new StringArgReturningVoidDelegate<ClientTCP>(RemoveClient);
                Invoke(d, new object[] { client });
            }
            else
            {
                try
                {
                    if (listClients.SelectedItem.Equals(client))
                    {
                        listClients.ClearSelected();
                        listData.Items.Clear();
                    }
                } catch { }
                listClients.Items.Remove(client);
            }
        }

        public void RemoveGameRoom(GameRoom room)
        {
            if (listClients.InvokeRequired)
            {
                StringArgReturningVoidDelegate<GameRoom> d = new StringArgReturningVoidDelegate<GameRoom>(RemoveGameRoom);
                Invoke(d, new object[] { room });
            }
            else
            {
                try
                {
                    if (listGameRooms.SelectedItem.Equals(room))
                    {
                        listGameRooms.ClearSelected();
                        listData.Items.Clear();
                    }
                }
                catch { }
                listGameRooms.Items.Remove(room);
            }
        }

        public void Debug(string msg)
        {
            if (textBoxDebug.InvokeRequired)
            {
                StringArgReturningVoidDelegate<string> d = new StringArgReturningVoidDelegate<string>(Debug);
                Invoke(d, new object[] { msg });
            }
            else
            {
                lastException = msg;
                textBoxDebug.AppendText(msg + "\n");
            }

        }

        public void ShowChatMsg(string nickname, string msg)
        {
            ShowChatMsg(nickname + ": " + msg);
        }

        public void StatusIndicator(int index, Exception ex = null)
        {
            switch(index)
            {
                case 1:
                    ptrUnactiveOne.Hide();
                    if (ex == null)
                        ptrOne.Show();
                    else
                    {
                        ptrErrorOne.Show();
                        ex1 = ex;
                    }
                    break;
                case 2:
                    ptrUnactiveTwo.Hide();
                    if (ex == null)
                        ptrTwo.Show();
                    else
                    {
                        ptrErrorTwo.Show();
                        ex2 = ex;
                    }
                    break;
                case 3:
                    ptrUnactiveThree.Hide();
                    if (ex == null)
                        ptrThree.Show();
                    else
                    {
                        ptrErrorThree.Show();
                        ex3 = ex;
                    }
                    break;
                case 4:
                    ptrUnactiveFour.Hide();
                    if (ex == null)
                        ptrFour.Show();
                    else
                    {
                        ptrErrorFour.Show();
                        ex4 = ex;
                    }
                    break;
            }
        }

        public void HidePtr(int index = 0)
        {
            switch(index)
            {
                case 0:
                    ptrFour.Hide();
                    ptrThree.Hide();
                    ptrTwo.Hide();
                    ptrOne.Hide();
                    ptrErrorOne.Hide();
                    ptrErrorTwo.Hide();
                    ptrErrorThree.Hide();
                    ptrErrorFour.Hide();
                    ptrUnactiveOne.Show();
                    ptrUnactiveTwo.Show();
                    ptrUnactiveThree.Show();
                    ptrUnactiveFour.Show();
                    break;
                case 1:
                    ptrOne.Hide();
                    ptrErrorOne.Hide();
                    ptrUnactiveOne.Show();
                    break;
                case 2:
                    ptrTwo.Hide();
                    ptrErrorTwo.Hide();
                    ptrUnactiveTwo.Show();
                    break;
                case 3:
                    ptrThree.Hide();
                    ptrErrorThree.Hide();
                    ptrUnactiveThree.Show();
                    break;
                case 4:
                    ptrFour.Hide();
                    ptrErrorFour.Hide();
                    ptrUnactiveFour.Show();
                    break;
            }
        }

        private void ShowChatMsg(string text)
        {
            if (txtBoxChat.InvokeRequired)
            {
                StringArgReturningVoidDelegate<string> d = new StringArgReturningVoidDelegate<string>(ShowChatMsg);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtBoxChat.AppendText(text + "\n");
            }
        }

        private void StartServer()
        {
            textBoxDebug.Clear();
            Program.ServerStart();
        }

        private void StopServer()
        {
            ServerTCP.ServerClose();
            listClients.Items.Clear();
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
                btnStart.Hide();
                btnStop.Show();
                btnRefresh.Show();
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!ServerTCP.Closed)
            {
                StopServer();
                btnStop.Hide();
                btnRefresh.Hide();
                btnStart.Show();
            }

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
            if(!ServerTCP.Closed)
                ServerTCP.ServerClose();
            Application.Exit();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!ServerTCP.Closed)
            {
                if (!String.IsNullOrEmpty(txtChat.Text))
                {
                    ServerSendData.SendGlChatMsg("Server", txtChat.Text);
                    txtChat.Text = "";
                }
            }
        }

        private void txtChat_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue.Equals(13))
            {
                if (!ServerTCP.Closed)
                {
                    if (!String.IsNullOrEmpty(txtChat.Text))
                    {
                        ServerSendData.SendGlChatMsg("Server", txtChat.Text);
                        txtChat.Text = "";
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
            try
            {
                if(listClients.SelectedItem != null)
                {
                    listData.Items.Clear();
                    listGameRooms.ClearSelected();
                }
                else
                {
                    return;
                }
            }
            catch (NullReferenceException)
            {
                return;
            }

            foreach (ClientTCP client in Global.clientList)
            {
                try
                {
                    if (client.Equals(listClients.SelectedItem))
                    {
                        listData.Items.Add("ip: " + client.ip);
                        listData.Items.Add("client state: " + client.clientState);
                        if(client.clientState != ClientTCPState.Entrance)
                        {
                            listData.Items.Add("player state: " + client.playerState);
                            listData.Items.Add("nickname: " + client.nickname);
                            listData.Items.Add("Ignis level: " + client.levels[0]);
                            listData.Items.Add("Terra level: " + client.levels[1]);
                            listData.Items.Add("Caeli level: " + client.levels[2]);
                            listData.Items.Add("Aqua level: " + client.levels[3]);
                            listData.Items.Add("Primus level: " + client.levels[4]);
                            listData.Items.Add("Ignis rank: " + client.ranks[0]);
                            listData.Items.Add("Terra rank: " + client.ranks[1]);
                            listData.Items.Add("Caeli rank: " + client.ranks[2]);
                            listData.Items.Add("Aqua rank: " + client.ranks[3]);
                            listData.Items.Add("Primus rank: " + client.ranks[4]);
                            if (client.playerState != NetPlayerState.InMainLobby)
                                listData.Items.Add("race: " + client.race);
                        }
                        break;
                    }
                }
                catch (NullReferenceException) { }
            }
        }

        private void listGameRooms_SelectedIndexChanged(object sender, EventArgs e) 
        {
            try
            {
                if(listGameRooms.SelectedItem != null)
                {
                    listData.Items.Clear();
                    listClients.ClearSelected();
                }
                else
                {
                    return;
                }
            }
            catch (NullReferenceException)
            {
                return;
            }

            foreach (GameRoom room in Global.roomsList)
            {
                try
                {
                    if (room.Equals(listGameRooms.SelectedItem))
                    {
                        listData.Items.Add("State: " + room.Status);
                        if (room.player1 != null)
                            listData.Items.Add("player1: " + room.player1.nickname);
                        if (room.player2 != null)
                            listData.Items.Add("player2: " + room.player2.nickname);
                        break;
                    }
                }
                catch (NullReferenceException) { }
            }
        }

        private void ptrErrorOne_DoubleClick(object sender, EventArgs e)
        {
            Debug(ex1 + "");
        }

        private void ptrErrorTwo_Click(object sender, EventArgs e)
        {
            Debug(ex2 + "");
        }

        private void ptrErrorThree_Click(object sender, EventArgs e)
        {
            Debug(ex3 + "");
        }

        private void ptrErrorFour_DoubleClick(object sender, EventArgs e)
        {
            Debug(ex4 + "");
        }

        private void btnStart_MouseEnter(object sender, EventArgs e)
        {
            btnStart.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            btnStart.ForeColor = Color.FromArgb(38, 144, 11);
        }

        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            btnExit.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.ForeColor = Color.FromArgb(38, 144, 11);
        }

        private void btnRefresh_MouseEnter(object sender, EventArgs e)
        {
            btnRefresh.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void btnRefresh_MouseLeave(object sender, EventArgs e)
        {
            btnRefresh.ForeColor = Color.FromArgb(38, 144, 11);
        }

        private void btnStop_MouseEnter(object sender, EventArgs e)
        {
            btnStop.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void btnStop_MouseLeave(object sender, EventArgs e)
        {
            btnStop.ForeColor = Color.FromArgb(38, 144, 11);
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnHide_MouseEnter(object sender, EventArgs e)
        {
            btnHide.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void btnHide_MouseLeave(object sender, EventArgs e)
        {
            btnHide.ForeColor = Color.FromArgb(38, 144, 11);
        }

        private void listClients_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < listClients.Items.Count)
            {
                Graphics g = e.Graphics;
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.FromArgb(38,144,11) : Color.Transparent);
                g.FillRectangle(backgroundColorBrush, e.Bounds);
                string itemText = listClients.Items[itemIndex].ToString();
                SolidBrush itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.Black) : new SolidBrush(Color.FromArgb(38, 144, 11));
                g.DrawString(itemText, e.Font, itemTextColorBrush, listClients.GetItemRectangle(itemIndex).Location);
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void listGameRooms_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < listClients.Items.Count)
            {
                Graphics g = e.Graphics;
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.FromArgb(38, 144, 11) : Color.Transparent);
                g.FillRectangle(backgroundColorBrush, e.Bounds);
                string itemText = listClients.Items[itemIndex].ToString();
                SolidBrush itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.Black) : new SolidBrush(Color.FromArgb(38, 144, 11));
                g.DrawString(itemText, e.Font, itemTextColorBrush, listClients.GetItemRectangle(itemIndex).Location);
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if(lastException != null)
                Clipboard.SetText(lastException);
        }


        #endregion

        private void txtChat_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.V))
                (sender as TextBox).Paste();
        }
    }
        
}
