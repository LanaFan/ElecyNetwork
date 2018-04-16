using System;
using Bindings;

namespace ElecyServer
{
    class Database
    {
        #region Accounts

        public bool LoginExist(string username)
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

        public bool NicknameExist(string nickname)
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

        public bool PasswordIsOkay(string username, string password)
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

        public string GetAccountNickname(string username)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM accounts WHERE Username='" + username+ "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                string nickname = DB_RS.Fields["Nickname"].Value.ToString();
                DB_RS.Close();
                return nickname;
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
                DB_RS.Update();
                DB_RS.Close();
            }
            SetAccountData(nickname);
        }

        #endregion

        #region Accounts Parameters

        private void SetAccountData(string nickname)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("Select * FROM AccountsParameters WHERE 0=1", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                DB_RS.AddNew();
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

        public void SetAccountData(string nickname, int[] levels, int[] ranks)
        {
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("Select * FROM AccountsParameters WHERE Nickname='" + nickname + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                DB_RS.AddNew();
                DB_RS.Fields["IgnisLevel"].Value = levels[1];
                DB_RS.Fields["TerraLevel"].Value = levels[2];
                DB_RS.Fields["CaeliLevel"].Value = levels[3];
                DB_RS.Fields["AquaLevel"].Value = levels[4];
                DB_RS.Fields["PrimusLevel"].Value = levels[5];
                DB_RS.Fields["IgnisRank"].Value = ranks[1];
                DB_RS.Fields["TerraRank"].Value = ranks[2];
                DB_RS.Fields["CaeliRank"].Value = ranks[3];
                DB_RS.Fields["AquaRank"].Value = ranks[4];
                DB_RS.Fields["PrimusRank"].Value = ranks[5];
                DB_RS.Update();
                DB_RS.Close();
            }
        }

        public int[][] GetAccountData(string nickname)
        {
            int[][] data = new int[2][];
            int[] levels = new int[Constants.RACES_NUMBER];
            int[] ranks = new int[Constants.RACES_NUMBER];
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM AccountsParameters WHERE Nickname='" + nickname + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                levels[0] = Convert.ToInt32(DB_RS.Fields["IgnisLevel"].Value);
                levels[1] = Convert.ToInt32(DB_RS.Fields["TerraLevel"].Value);
                levels[2] = Convert.ToInt32(DB_RS.Fields["CaeliLevel"].Value);
                levels[3] = Convert.ToInt32(DB_RS.Fields["AquaLevel"].Value);
                levels[4] = Convert.ToInt32(DB_RS.Fields["PrimusLevel"].Value);
                ranks[0] = Convert.ToInt32(DB_RS.Fields["IgnisRank"].Value);
                ranks[1] = Convert.ToInt32(DB_RS.Fields["TerraRank"].Value);
                ranks[2] = Convert.ToInt32(DB_RS.Fields["CaeliRank"].Value);
                ranks[3] = Convert.ToInt32(DB_RS.Fields["AquaRank"].Value);
                ranks[4] = Convert.ToInt32(DB_RS.Fields["PrimusRank"].Value);
                DB_RS.Close();
            }
            data[0] = levels;
            data[1] = ranks;

            return data;
        }

        #endregion

        #region Maps info

        public int[] GetMapScale(int mapIndex)
        {
            int[] scale = new int[2];
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM MapsInfo WHERE MapNumber ='" + mapIndex + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                scale[0] = Convert.ToInt32(DB_RS.Fields["MapLenght"].Value);
                scale[1] = Convert.ToInt32(DB_RS.Fields["MapWidth"].Value);
                DB_RS.Close();
            }
            return scale;
        }

        public float[][] GetSpawnPos(int mapIndex)
        {
            float[][] spawnPos = new float[2][];
            float[] firstSpawnPos = new float[3];
            float[] secondSpawnPos = new float[3];
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM MapsInfo WHERE MapNumber ='" + mapIndex + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                firstSpawnPos[0] = Convert.ToSingle(DB_RS.Fields["FirstSpawnPointX"].Value);
                firstSpawnPos[1] = Convert.ToSingle(DB_RS.Fields["FirstSpawnPointY"].Value);
                firstSpawnPos[2] = Convert.ToSingle(DB_RS.Fields["FirstSpawnPointZ"].Value);
                secondSpawnPos[0] = Convert.ToSingle(DB_RS.Fields["SecondSpawnPointX"].Value);
                secondSpawnPos[1] = Convert.ToSingle(DB_RS.Fields["SecondSpawnPointY"].Value);
                secondSpawnPos[2] = Convert.ToSingle(DB_RS.Fields["SecondSpawnPointZ"].Value);
                DB_RS.Close();
            }
            spawnPos[0] = firstSpawnPos;
            spawnPos[1] = secondSpawnPos;
            return spawnPos;
        }

        public float[][] GetSpawnRot(int mapIndex)
        {
            float[][] spawnRot = new float[2][];
            float[] firstSpawnRot = new float[4];
            float[] secondSpawnRot = new float[4];
            var DB_RS = Global.mysql.DB_RS;
            {
                DB_RS.Open("SELECT * FROM MapsInfo WHERE MapNumber ='" + mapIndex + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                firstSpawnRot[0] = Convert.ToSingle(DB_RS.Fields["FirstSpawnPointRotX"].Value);
                firstSpawnRot[1] = Convert.ToSingle(DB_RS.Fields["FirstSpawnPointRotY"].Value);
                firstSpawnRot[2] = Convert.ToSingle(DB_RS.Fields["FirstSpawnPointRotZ"].Value);
                firstSpawnRot[3] = Convert.ToSingle(DB_RS.Fields["FirstSpawnPointRotW"].Value);
                secondSpawnRot[0] = Convert.ToSingle(DB_RS.Fields["SecondSpawnPointRotX"].Value);
                secondSpawnRot[1] = Convert.ToSingle(DB_RS.Fields["SecondSpawnPointRotY"].Value);
                secondSpawnRot[2] = Convert.ToSingle(DB_RS.Fields["SecondSpawnPointRotZ"].Value);
                secondSpawnRot[3] = Convert.ToSingle(DB_RS.Fields["SecondSpawnPointRotW"].Value);
                DB_RS.Close();
            }
            spawnRot[0] = firstSpawnRot;
            spawnRot[1] = secondSpawnRot;
            return spawnRot;
        }

        #endregion
    }
}
