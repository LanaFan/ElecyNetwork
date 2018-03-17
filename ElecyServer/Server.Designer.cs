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
            this.ptrRed = new System.Windows.Forms.PictureBox();
            this.ptrYellow = new System.Windows.Forms.PictureBox();
            this.ptrGreen = new System.Windows.Forms.PictureBox();
            this.listGameRooms = new System.Windows.Forms.ListBox();
            this.lblGameRooms = new System.Windows.Forms.Label();
            this.listNetPlayers = new System.Windows.Forms.ListBox();
            this.lblNetPlayers = new System.Windows.Forms.Label();
            this.listClients = new System.Windows.Forms.ListBox();
            this.lblClients = new System.Windows.Forms.Label();
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.txtBoxChat = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblID = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.listData = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.ptrRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrYellow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrGreen)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefresh.BackgroundImage")));
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(280, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(30, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.TabStop = false;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStop.BackgroundImage")));
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(240, 5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(30, 30);
            this.btnStop.TabIndex = 1;
            this.btnStop.TabStop = false;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStart.BackgroundImage")));
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(200, 5);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnStart.Size = new System.Drawing.Size(30, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.TabStop = false;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // ptrRed
            // 
            this.ptrRed.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrRed.BackgroundImage")));
            this.ptrRed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrRed.Location = new System.Drawing.Point(10, 5);
            this.ptrRed.Name = "ptrRed";
            this.ptrRed.Size = new System.Drawing.Size(26, 26);
            this.ptrRed.TabIndex = 2;
            this.ptrRed.TabStop = false;
            // 
            // ptrYellow
            // 
            this.ptrYellow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrYellow.BackgroundImage")));
            this.ptrYellow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrYellow.Location = new System.Drawing.Point(10, 5);
            this.ptrYellow.Name = "ptrYellow";
            this.ptrYellow.Size = new System.Drawing.Size(26, 26);
            this.ptrYellow.TabIndex = 1;
            this.ptrYellow.TabStop = false;
            // 
            // ptrGreen
            // 
            this.ptrGreen.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptrGreen.BackgroundImage")));
            this.ptrGreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptrGreen.Location = new System.Drawing.Point(10, 5);
            this.ptrGreen.Name = "ptrGreen";
            this.ptrGreen.Size = new System.Drawing.Size(26, 26);
            this.ptrGreen.TabIndex = 0;
            this.ptrGreen.TabStop = false;
            // 
            // listGameRooms
            // 
            this.listGameRooms.FormattingEnabled = true;
            this.listGameRooms.Location = new System.Drawing.Point(290, 75);
            this.listGameRooms.Name = "listGameRooms";
            this.listGameRooms.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listGameRooms.Size = new System.Drawing.Size(120, 121);
            this.listGameRooms.TabIndex = 1;
            this.listGameRooms.TabStop = false;
            // 
            // lblGameRooms
            // 
            this.lblGameRooms.AutoSize = true;
            this.lblGameRooms.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGameRooms.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblGameRooms.Location = new System.Drawing.Point(290, 50);
            this.lblGameRooms.Name = "lblGameRooms";
            this.lblGameRooms.Size = new System.Drawing.Size(108, 20);
            this.lblGameRooms.TabIndex = 0;
            this.lblGameRooms.Text = "GameRooms:";
            this.lblGameRooms.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Server_MouseDown);
            this.lblGameRooms.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Server_MouseMove);
            // 
            // listNetPlayers
            // 
            this.listNetPlayers.FormattingEnabled = true;
            this.listNetPlayers.Location = new System.Drawing.Point(150, 75);
            this.listNetPlayers.Name = "listNetPlayers";
            this.listNetPlayers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listNetPlayers.Size = new System.Drawing.Size(120, 121);
            this.listNetPlayers.TabIndex = 1;
            this.listNetPlayers.TabStop = false;
            this.listNetPlayers.SelectedIndexChanged += new System.EventHandler(this.listNetPlayers_SelectedIndexChanged);
            // 
            // lblNetPlayers
            // 
            this.lblNetPlayers.AutoSize = true;
            this.lblNetPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNetPlayers.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblNetPlayers.Location = new System.Drawing.Point(150, 50);
            this.lblNetPlayers.Name = "lblNetPlayers";
            this.lblNetPlayers.Size = new System.Drawing.Size(89, 20);
            this.lblNetPlayers.TabIndex = 0;
            this.lblNetPlayers.Text = "NetPlayers:";
            this.lblNetPlayers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Server_MouseDown);
            this.lblNetPlayers.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Server_MouseMove);
            // 
            // listClients
            // 
            this.listClients.FormattingEnabled = true;
            this.listClients.Location = new System.Drawing.Point(10, 75);
            this.listClients.Name = "listClients";
            this.listClients.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listClients.Size = new System.Drawing.Size(120, 121);
            this.listClients.TabIndex = 1;
            this.listClients.TabStop = false;
            this.listClients.SelectedIndexChanged += new System.EventHandler(this.listClients_SelectedIndexChanged);
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblClients.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblClients.Location = new System.Drawing.Point(10, 50);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(61, 20);
            this.lblClients.TabIndex = 0;
            this.lblClients.Text = "Clients:";
            this.lblClients.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Server_MouseDown);
            this.lblClients.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Server_MouseMove);
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDebug.Location = new System.Drawing.Point(310, 220);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.ReadOnly = true;
            this.textBoxDebug.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDebug.Size = new System.Drawing.Size(280, 180);
            this.textBoxDebug.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSend.BackgroundImage")));
            this.btnSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Location = new System.Drawing.Point(265, 380);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(20, 20);
            this.btnSend.TabIndex = 2;
            this.btnSend.TabStop = false;
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(10, 380);
            this.txtChat.Name = "txtChat";
            this.txtChat.ShortcutsEnabled = false;
            this.txtChat.Size = new System.Drawing.Size(250, 20);
            this.txtChat.TabIndex = 1;
            this.txtChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtChat_KeyDown);
            // 
            // txtBoxChat
            // 
            this.txtBoxChat.AcceptsReturn = true;
            this.txtBoxChat.AcceptsTab = true;
            this.txtBoxChat.BackColor = System.Drawing.Color.White;
            this.txtBoxChat.Location = new System.Drawing.Point(10, 220);
            this.txtBoxChat.Multiline = true;
            this.txtBoxChat.Name = "txtBoxChat";
            this.txtBoxChat.ReadOnly = true;
            this.txtBoxChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxChat.Size = new System.Drawing.Size(280, 150);
            this.txtBoxChat.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(570, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(26, 26);
            this.btnExit.TabIndex = 4;
            this.btnExit.TabStop = false;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblID.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblID.Location = new System.Drawing.Point(330, 8);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(103, 24);
            this.lblID.TabIndex = 5;
            this.lblID.Text = "IP address:";
            this.lblID.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Server_MouseDown);
            this.lblID.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Server_MouseMove);
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClear.BackgroundImage")));
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(550, 178);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(40, 40);
            this.btnClear.TabIndex = 6;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // listData
            // 
            this.listData.FormattingEnabled = true;
            this.listData.Location = new System.Drawing.Point(430, 75);
            this.listData.Name = "listData";
            this.listData.Size = new System.Drawing.Size(160, 95);
            this.listData.TabIndex = 7;
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(600, 410);
            this.Controls.Add(this.listData);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.lblClients);
            this.Controls.Add(this.lblNetPlayers);
            this.Controls.Add(this.lblGameRooms);
            this.Controls.Add(this.listGameRooms);
            this.Controls.Add(this.listNetPlayers);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.listClients);
            this.Controls.Add(this.ptrRed);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.ptrGreen);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.ptrYellow);
            this.Controls.Add(this.txtBoxChat);
            this.Controls.Add(this.textBoxDebug);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Server";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Server_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Server_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.ptrRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrYellow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptrGreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox listGameRooms;
        private System.Windows.Forms.Label lblGameRooms;
        private System.Windows.Forms.ListBox listNetPlayers;
        private System.Windows.Forms.Label lblNetPlayers;
        private System.Windows.Forms.ListBox listClients;
        private System.Windows.Forms.Label lblClients;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.TextBox txtBoxChat;
        private System.Windows.Forms.PictureBox ptrRed;
        private System.Windows.Forms.PictureBox ptrYellow;
        private System.Windows.Forms.PictureBox ptrGreen;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ListBox listData;
    }
}