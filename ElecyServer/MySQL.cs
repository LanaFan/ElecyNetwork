using System;
using ADODB;

public class MySQL
{

    public Recordset DB_RS;
    public Connection DB_CONN;

    public void MySQLInit()
    {
        try
        {
            DB_RS = new Recordset();
            DB_CONN = new Connection();

            DB_CONN.ConnectionString = "Driver={MySQL ODBC 3.51 Driver}; Server=localhost; Port=24985;Database=elecyproject;User=root;Password=;Option=3;";
            DB_CONN.CursorLocation = CursorLocationEnum.adUseServer;
            DB_CONN.Open();
            Console.WriteLine("Connection to MySQL server completed!");

            var db = DB_RS;
            {

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

}
