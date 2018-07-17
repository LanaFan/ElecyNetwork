﻿using System;
using ADODB;

namespace ElecyServer
{
    public class MySQL
    {

        public Recordset DB_RS { get; private set; }
        public Connection DB_CONN { get; private set; }

        public void MySQLInit()
        {
            try
            {
                DB_RS = new Recordset();
                DB_CONN = new Connection();

                DB_CONN.ConnectionString = "Driver={MySQL ODBC 3.51 Driver}; Server=localhost; Port=3306;Database=elecyproject;User=root;Password=;Option=3;";
                DB_CONN.CursorLocation = CursorLocationEnum.adUseServer;
                DB_CONN.Open();
                Global.serverForm.StatusIndicator(2);
            }
            catch (Exception ex)
            {
                Global.serverForm.StatusIndicator(2, ex);
            }
        }

        public void MySQLClose()
        {
            try
            {
                DB_CONN.Close();
                Global.serverForm.HidePtr(2);
            }
            catch(Exception ex)
            {
                Global.serverForm.StatusIndicator(2, ex);
            }
        }

    }
}
