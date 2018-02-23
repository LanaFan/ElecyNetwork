using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;

namespace ElecyServer
{
    class Database
    {

        public bool AccountExist(int index, string username)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM accounts WHERE Username='" + username + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                if (DB_RS.EOF)
                {
                    DB_RS.Close();
                    return false;
                }
                else
                {
                    DB_RS.Close();
                    return true;
                }
                
            }
        }

        public bool PasswordIsOkay(int index, string username, string password)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT'" + username + "' FROM accounts WHERE Password='" + password + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                if (DB_RS.EOF)
                {
                    DB_RS.Close();
                    return false;
                }
                else
                {
                    DB_RS.Close();
                    return true;
                }

            }
        }

        public void AddAccount(string username, string password)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM accounts WHERE 0=1", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                DB_RS.AddNew();
                DB_RS.Fields["Username"].Value = username;
                DB_RS.Fields["Password"].Value = password;
                DB_RS.Update();
                DB_RS.Close();
            }
        }
    }
}
