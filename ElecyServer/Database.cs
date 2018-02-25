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

        public bool NicknameExist(int index, string nickname)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM accounts WHERE Nickname='" + nickname + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
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

        public void AddAccount(string username, string password, string nickname)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM accounts WHERE 0=1", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                DB_RS.AddNew();
                DB_RS.Fields["Username"].Value = username;
                DB_RS.Fields["Password"].Value = password;
                DB_RS.Fields["Nickname"].Value = nickname;
                DB_RS.Fields["IgnisLevel"].Value = 1;
                DB_RS.Fields["TerraLevel"].Value = 1;
                DB_RS.Fields["CaeliLevel"].Value = 1;
                DB_RS.Fields["AquaLevel"].Value = 1;
                DB_RS.Fields["PrimusLevel"].Value = 1;
                DB_RS.Fields["IgnisRank"].Value = 0;
                DB_RS.Fields["TerraRank"].Value = 0;
                DB_RS.Fields["CaeliRank"].Value = 0;
                DB_RS.Fields["AquaRank"].Value = 0;
                DB_RS.Fields["PrimusRank"].Value = 0;
                DB_RS.Update();
                DB_RS.Close();
            }
        }

        public string GetAccountNickname(string username)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT'" + username + "'FROM accounts WHERE 0=1", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                string nickname = (string)DB_RS.Fields["Nickname"].Value;
                DB_RS.Close();
                return nickname;
            }
        }
    }
}
