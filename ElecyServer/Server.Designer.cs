namespace ElecyServer
{
    partial class Server
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.listGameRooms = new System.Windows.Forms.ListBox();
            this.listClients = new System.Windows.Forms.ListBox();
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.txtBoxChat = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblID = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.listData = new System.Windows.Forms.ListBox();
            this.ptrFour = new System.Windows.Forms.PictureBox();
            this.ptrThree = new System.Windows.Forms.PictureBox();
            this.ptrTwo = new System.Windows.Forms.PictureBox();
            this.ptrOne = new System.Windows.Forms.PictureBox();
            this.ptrUnactiveFour = new System.Windows.Forms.PictureBox();
            this.ptrUnactiveThree = new System.Windows.Forms.PictureBox();
            this.ptrUnactiveTwo = new System.Windows.Forms.PictureBox();
            this.ptrUnactiveOne = new System.Windows.Forms.PictureBox();
            this.ptrErrorFour = new System.Windows.Forms.PictureBox();
            this.ptrErrorThree = new System.Windows.Forms.PictureBox();
            this.ptrErrorTwo = new System.Windows.Forms.PictureBox();
            this.ptrErrorOne = new System.Windows.Forms.PictureBox();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ptrFour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrThree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrTwo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrOne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrUnactiveFour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrUnactiveThree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrUnactiveTwo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrUnactiveOne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrErrorFour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrErrorThree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrErrorTwo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrErrorOne)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(167)))), ((int)(((byte)(32)))));
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnRefresh.Location = new System.Drawing.Point(280, 16);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(135, 50);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.TabStop = false;
            this.btnRefresh.Text = "Restart";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.MouseEnter += new System.EventHandler(this.btnRefresh_MouseEnter);
            this.btnRefresh.MouseLeave += new System.EventHandler(this.btnRefresh_MouseLeave);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(167)))), ((int)(((byte)(32)))));
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnStop.Location = new System.Drawing.Point(159, 16);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(115, 50);
            this.btnStop.TabIndex = 1;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            this.btnStop.MouseEnter += new System.EventHandler(this.btnStop_MouseEnter);
            this.btnStop.MouseLeave += new System.EventHandler(this.btnStop_MouseLeave);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(167)))), ((int)(((byte)(32)))));
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnStart.Location = new System.Drawing.Point(218, 16);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnStart.Size = new System.Drawing.Size(115, 50);
            this.btnStart.TabIndex = 0;
            this.btnStart.TabStop = false;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.btnStart.MouseEnter += new System.EventHandler(this.btnStart_MouseEnter);
            this.btnStart.MouseLeave += new System.EventHandler(this.btnStart_MouseLeave);
            // 
            // listGameRooms
            // 
            this.listGameRooms.BackColor = System.Drawing.Color.Black;
            this.listGameRooms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listGameRooms.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listGameRooms.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.listGameRooms.FormattingEnabled = true;
            this.listGameRooms.Location = new System.Drawing.Point(25, 245);
            this.listGameRooms.Name = "listGameRooms";
            this.listGameRooms.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listGameRooms.Size = new System.Drawing.Size(190, 65);
            this.listGameRooms.TabIndex = 1;
            this.listGameRooms.TabStop = false;
            this.listGameRooms.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listGameRooms_DrawItem);
            this.listGameRooms.SelectedIndexChanged += new System.EventHandler(this.listGameRooms_SelectedIndexChanged);
            // 
            // listClients
            // 
            this.listClients.BackColor = System.Drawing.Color.Black;
            this.listClients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listClients.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listClients.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.listClients.FormattingEnabled = true;
            this.listClients.Location = new System.Drawing.Point(25, 113);
            this.listClients.Name = "listClients";
            this.listClients.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listClients.Size = new System.Drawing.Size(190, 78);
            this.listClients.TabIndex = 1;
            this.listClients.TabStop = false;
            this.listClients.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listClients_DrawItem);
            this.listClients.SelectedIndexChanged += new System.EventHandler(this.listClients_SelectedIndexChanged);
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.BackColor = System.Drawing.Color.Black;
            this.textBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDebug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.textBoxDebug.HideSelection = false;
            this.textBoxDebug.Location = new System.Drawing.Point(248, 87);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.ReadOnly = true;
            this.textBoxDebug.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDebug.Size = new System.Drawing.Size(402, 186);
            this.textBoxDebug.TabIndex = 4;
            this.textBoxDebug.TabStop = false;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.Transparent;
            this.btnSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSend.BackgroundImage")));
            this.btnSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Location = new System.Drawing.Point(665, 420);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(20, 20);
            this.btnSend.TabIndex = 2;
            this.btnSend.TabStop = false;
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtChat
            // 
            this.txtChat.BackColor = System.Drawing.Color.Black;
            this.txtChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.txtChat.Location = new System.Drawing.Point(250, 424);
            this.txtChat.Name = "txtChat";
            this.txtChat.ShortcutsEnabled = false;
            this.txtChat.Size = new System.Drawing.Size(402, 13);
            this.txtChat.TabIndex = 1;
            this.txtChat.TabStop = false;
            this.txtChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtChat_KeyDown);
            this.txtChat.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtChat_KeyUp);
            // 
            // txtBoxChat
            // 
            this.txtBoxChat.AcceptsReturn = true;
            this.txtBoxChat.AcceptsTab = true;
            this.txtBoxChat.BackColor = System.Drawing.Color.Black;
            this.txtBoxChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBoxChat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.txtBoxChat.Location = new System.Drawing.Point(250, 302);
            this.txtBoxChat.Multiline = true;
            this.txtBoxChat.Name = "txtBoxChat";
            this.txtBoxChat.ReadOnly = true;
            this.txtBoxChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxChat.Size = new System.Drawing.Size(440, 110);
            this.txtBoxChat.TabIndex = 3;
            this.txtBoxChat.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(167)))), ((int)(((byte)(32)))));
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnExit.Location = new System.Drawing.Point(585, 16);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(115, 50);
            this.btnExit.TabIndex = 4;
            this.btnExit.TabStop = false;
            this.btnExit.Text = "Close";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.MouseEnter += new System.EventHandler(this.btnExit_MouseEnter);
            this.btnExit.MouseLeave += new System.EventHandler(this.btnExit_MouseLeave);
            // 
            // lblID
            // 
            this.lblID.Location = new System.Drawing.Point(0, 0);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(100, 23);
            this.lblID.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClear.BackgroundImage")));
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(662, 88);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(30, 30);
            this.btnClear.TabIndex = 6;
            this.btnClear.TabStop = false;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // listData
            // 
            this.listData.BackColor = System.Drawing.Color.Black;
            this.listData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.listData.FormattingEnabled = true;
            this.listData.Location = new System.Drawing.Point(25, 336);
            this.listData.Name = "listData";
            this.listData.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listData.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listData.Size = new System.Drawing.Size(190, 104);
            this.listData.TabIndex = 7;
            this.listData.TabStop = false;
            // 
            // ptrFour
            // 
            this.ptrFour.BackColor = System.Drawing.Color.Transparent;
            this.ptrFour.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrFour.BackgroundImage")));
            this.ptrFour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrFour.Location = new System.Drawing.Point(28, 18);
            this.ptrFour.Name = "ptrFour";
            this.ptrFour.Size = new System.Drawing.Size(50, 15);
            this.ptrFour.TabIndex = 1;
            this.ptrFour.TabStop = false;
            // 
            // ptrThree
            // 
            this.ptrThree.BackColor = System.Drawing.Color.Transparent;
            this.ptrThree.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrThree.BackgroundImage")));
            this.ptrThree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrThree.Location = new System.Drawing.Point(28, 28);
            this.ptrThree.Name = "ptrThree";
            this.ptrThree.Size = new System.Drawing.Size(50, 15);
            this.ptrThree.TabIndex = 8;
            this.ptrThree.TabStop = false;
            // 
            // ptrTwo
            // 
            this.ptrTwo.BackColor = System.Drawing.Color.Transparent;
            this.ptrTwo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrTwo.BackgroundImage")));
            this.ptrTwo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrTwo.Location = new System.Drawing.Point(28, 38);
            this.ptrTwo.Name = "ptrTwo";
            this.ptrTwo.Size = new System.Drawing.Size(50, 15);
            this.ptrTwo.TabIndex = 9;
            this.ptrTwo.TabStop = false;
            // 
            // ptrOne
            // 
            this.ptrOne.BackColor = System.Drawing.Color.Transparent;
            this.ptrOne.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrOne.BackgroundImage")));
            this.ptrOne.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrOne.Location = new System.Drawing.Point(28, 48);
            this.ptrOne.Name = "ptrOne";
            this.ptrOne.Size = new System.Drawing.Size(50, 15);
            this.ptrOne.TabIndex = 10;
            this.ptrOne.TabStop = false;
            // 
            // ptrUnactiveFour
            // 
            this.ptrUnactiveFour.BackColor = System.Drawing.Color.Transparent;
            this.ptrUnactiveFour.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrUnactiveFour.BackgroundImage")));
            this.ptrUnactiveFour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrUnactiveFour.Location = new System.Drawing.Point(28, 18);
            this.ptrUnactiveFour.Name = "ptrUnactiveFour";
            this.ptrUnactiveFour.Size = new System.Drawing.Size(50, 15);
            this.ptrUnactiveFour.TabIndex = 11;
            this.ptrUnactiveFour.TabStop = false;
            // 
            // ptrUnactiveThree
            // 
            this.ptrUnactiveThree.BackColor = System.Drawing.Color.Transparent;
            this.ptrUnactiveThree.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrUnactiveThree.BackgroundImage")));
            this.ptrUnactiveThree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrUnactiveThree.Location = new System.Drawing.Point(28, 28);
            this.ptrUnactiveThree.Name = "ptrUnactiveThree";
            this.ptrUnactiveThree.Size = new System.Drawing.Size(50, 15);
            this.ptrUnactiveThree.TabIndex = 12;
            this.ptrUnactiveThree.TabStop = false;
            // 
            // ptrUnactiveTwo
            // 
            this.ptrUnactiveTwo.BackColor = System.Drawing.Color.Transparent;
            this.ptrUnactiveTwo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrUnactiveTwo.BackgroundImage")));
            this.ptrUnactiveTwo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrUnactiveTwo.Location = new System.Drawing.Point(28, 38);
            this.ptrUnactiveTwo.Name = "ptrUnactiveTwo";
            this.ptrUnactiveTwo.Size = new System.Drawing.Size(50, 15);
            this.ptrUnactiveTwo.TabIndex = 13;
            this.ptrUnactiveTwo.TabStop = false;
            // 
            // ptrUnactiveOne
            // 
            this.ptrUnactiveOne.BackColor = System.Drawing.Color.Transparent;
            this.ptrUnactiveOne.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrUnactiveOne.BackgroundImage")));
            this.ptrUnactiveOne.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrUnactiveOne.Location = new System.Drawing.Point(28, 48);
            this.ptrUnactiveOne.Name = "ptrUnactiveOne";
            this.ptrUnactiveOne.Size = new System.Drawing.Size(50, 15);
            this.ptrUnactiveOne.TabIndex = 14;
            this.ptrUnactiveOne.TabStop = false;
            // 
            // ptrErrorFour
            // 
            this.ptrErrorFour.BackColor = System.Drawing.Color.Transparent;
            this.ptrErrorFour.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrErrorFour.BackgroundImage")));
            this.ptrErrorFour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrErrorFour.Location = new System.Drawing.Point(28, 18);
            this.ptrErrorFour.Name = "ptrErrorFour";
            this.ptrErrorFour.Size = new System.Drawing.Size(50, 15);
            this.ptrErrorFour.TabIndex = 15;
            this.ptrErrorFour.TabStop = false;
            this.ptrErrorFour.DoubleClick += new System.EventHandler(this.ptrErrorFour_DoubleClick);
            // 
            // ptrErrorThree
            // 
            this.ptrErrorThree.BackColor = System.Drawing.Color.Transparent;
            this.ptrErrorThree.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrErrorThree.BackgroundImage")));
            this.ptrErrorThree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrErrorThree.Location = new System.Drawing.Point(28, 28);
            this.ptrErrorThree.Name = "ptrErrorThree";
            this.ptrErrorThree.Size = new System.Drawing.Size(50, 15);
            this.ptrErrorThree.TabIndex = 16;
            this.ptrErrorThree.TabStop = false;
            this.ptrErrorThree.Click += new System.EventHandler(this.ptrErrorThree_Click);
            // 
            // ptrErrorTwo
            // 
            this.ptrErrorTwo.BackColor = System.Drawing.Color.Transparent;
            this.ptrErrorTwo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrErrorTwo.BackgroundImage")));
            this.ptrErrorTwo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrErrorTwo.Location = new System.Drawing.Point(28, 38);
            this.ptrErrorTwo.Name = "ptrErrorTwo";
            this.ptrErrorTwo.Size = new System.Drawing.Size(50, 15);
            this.ptrErrorTwo.TabIndex = 17;
            this.ptrErrorTwo.TabStop = false;
            this.ptrErrorTwo.Click += new System.EventHandler(this.ptrErrorTwo_Click);
            // 
            // ptrErrorOne
            // 
            this.ptrErrorOne.BackColor = System.Drawing.Color.Transparent;
            this.ptrErrorOne.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrErrorOne.BackgroundImage")));
            this.ptrErrorOne.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrErrorOne.Location = new System.Drawing.Point(28, 48);
            this.ptrErrorOne.Name = "ptrErrorOne";
            this.ptrErrorOne.Size = new System.Drawing.Size(50, 15);
            this.ptrErrorOne.TabIndex = 18;
            this.ptrErrorOne.TabStop = false;
            this.ptrErrorOne.DoubleClick += new System.EventHandler(this.ptrErrorOne_DoubleClick);
            // 
            // btnHide
            // 
            this.btnHide.BackColor = System.Drawing.Color.Transparent;
            this.btnHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHide.FlatAppearance.BorderSize = 0;
            this.btnHide.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnHide.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(167)))), ((int)(((byte)(32)))));
            this.btnHide.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnHide.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(144)))), ((int)(((byte)(11)))));
            this.btnHide.Location = new System.Drawing.Point(464, 16);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(115, 50);
            this.btnHide.TabIndex = 19;
            this.btnHide.TabStop = false;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = false;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            this.btnHide.MouseEnter += new System.EventHandler(this.btnHide_MouseEnter);
            this.btnHide.MouseLeave += new System.EventHandler(this.btnHide_MouseLeave);
            // 
            // btnCopy
            // 
            this.btnCopy.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCopy.BackgroundImage")));
            this.btnCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCopy.FlatAppearance.BorderSize = 0;
            this.btnCopy.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnCopy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCopy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Location = new System.Drawing.Point(662, 130);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(30, 30);
            this.btnCopy.TabIndex = 20;
            this.btnCopy.TabStop = false;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(720, 480);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.ptrErrorOne);
            this.Controls.Add(this.ptrErrorTwo);
            this.Controls.Add(this.ptrErrorThree);
            this.Controls.Add(this.ptrErrorFour);
            this.Controls.Add(this.ptrUnactiveOne);
            this.Controls.Add(this.ptrUnactiveTwo);
            this.Controls.Add(this.ptrUnactiveThree);
            this.Controls.Add(this.ptrUnactiveFour);
            this.Controls.Add(this.ptrOne);
            this.Controls.Add(this.ptrTwo);
            this.Controls.Add(this.ptrThree);
            this.Controls.Add(this.listData);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.listGameRooms);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.listClients);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.ptrFour);
            this.Controls.Add(this.txtBoxChat);
            this.Controls.Add(this.textBoxDebug);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Server";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Server_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Server_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.ptrFour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrThree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrTwo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrOne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrUnactiveFour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrUnactiveThree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrUnactiveTwo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrUnactiveOne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrErrorFour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrErrorThree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrErrorTwo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrErrorOne)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox listGameRooms;
        private System.Windows.Forms.ListBox listClients;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.TextBox txtBoxChat;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ListBox listData;
        private System.Windows.Forms.PictureBox ptrFour;
        private System.Windows.Forms.PictureBox ptrThree;
        private System.Windows.Forms.PictureBox ptrTwo;
        private System.Windows.Forms.PictureBox ptrOne;
        private System.Windows.Forms.PictureBox ptrUnactiveFour;
        private System.Windows.Forms.PictureBox ptrUnactiveThree;
        private System.Windows.Forms.PictureBox ptrUnactiveTwo;
        private System.Windows.Forms.PictureBox ptrUnactiveOne;
        private System.Windows.Forms.PictureBox ptrErrorFour;
        private System.Windows.Forms.PictureBox ptrErrorThree;
        private System.Windows.Forms.PictureBox ptrErrorTwo;
        private System.Windows.Forms.PictureBox ptrErrorOne;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnCopy;
    }
}