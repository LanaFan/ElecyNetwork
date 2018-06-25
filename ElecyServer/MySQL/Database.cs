using System;
using System.Collections.Generic;
using Bindings;
using System.Threading;
using System.Threading.Tasks;

namespace ElecyServer
{
    class Database
    {

        private void CheckQueue(List<object> list)
        {
            if (list.Count != 0)
            {
                lock (list[0])
                {
                    Monitor.Pulse(list[0]);
                }
                list.RemoveAt(0);
            }

        }

        #region Accounts

        static List<object> accountsTable = new List<object>();

        public bool LoginExist(string username)
        {
            try
            {
                var DB_RS = Global.mysql.DB_RS;
                {
                    DB_RS.Open("SELECT * FROM accounts WHERE Username='" + username + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    if (DB_RS.EOF)
                    {
                        DB_RS.Close();
                        CheckQueue(accountsTable);
                        return false;
                    }
                    else
                    {
                        DB_RS.Close();
                        CheckQueue(accountsTable);
                        return true;
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                accountsTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                return LoginExist(username);
            }
        }

        public bool NicknameExist(string nickname)
        {
            try
            {
                var DB_RS = Global.mysql.DB_RS;
                {
                    DB_RS.Open("SELECT * FROM accounts WHERE Nickname='" + nickname + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    if (DB_RS.EOF)
                    {
                        DB_RS.Close();
                        CheckQueue(accountsTable);
                        return false;
                    }
                    else
                    {
                        DB_RS.Close();
                        CheckQueue(accountsTable);
                        return true;
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                accountsTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                return NicknameExist(nickname);
            }
        }

        public bool PasswordIsOkay(string username, string password)
        {
            try
            {
                var DB_RS = Global.mysql.DB_RS;
                {
                    DB_RS.Open("SELECT'" + username + "' FROM accounts WHERE Password='" + password + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    if (DB_RS.EOF)
                    {
                        DB_RS.Close();
                        CheckQueue(accountsTable);
                        return false;
                    }
                    else
                    {
                        DB_RS.Close();
                        CheckQueue(accountsTable);
                        return true;
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                accountsTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                return PasswordIsOkay(username, password);
            }
        }

        public string GetAccountNickname(string username)
        {
            try
            {
                var DB_RS = Global.mysql.DB_RS;
                {
                    DB_RS.Open("SELECT * FROM accounts WHERE Username='" + username + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    string nickname = DB_RS.Fields["Nickname"].Value.ToString();
                    DB_RS.Close();
                    CheckQueue(accountsTable);
                    return nickname;
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                accountsTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                return GetAccountNickname(username);
            }
        }

        public void AddAccount(string username, string password, string nickname)
        {
            try
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
                    CheckQueue(accountsTable);
                }
                SetAccountData(nickname);
                SetSkillBuildData(nickname);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                accountsTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                AddAccount(username, password, nickname);
            }
        }

        #endregion

        #region Accounts Parameters

        List<object> accountsParametersTable = new List<object>();

        private void SetAccountData(string nickname)
        {
            try
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
                    CheckQueue(accountsParametersTable);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                accountsParametersTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                SetAccountData(nickname);
            }
        }

        public void SetAccountData(string nickname, int[] levels, int[] ranks)
        {
            try
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
                    CheckQueue(accountsParametersTable);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                accountsParametersTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                SetAccountData(nickname, levels, ranks);
            }
        }

        public int[][] GetAccountData(string nickname)
        {
            try
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
                    CheckQueue(accountsParametersTable);
                }
                data[0] = levels;
                data[1] = ranks;

                return data;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                accountsParametersTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                return GetAccountData(nickname);
            }
        }

        #endregion

        #region Maps info

        List<object> mapsInfoTable = new List<object>();

        public int[] GetMapScale(int mapIndex)
        {
            try
            {
                int[] scale = new int[2];
                var DB_RS = Global.mysql.DB_RS;
                {
                    DB_RS.Open("SELECT * FROM MapsInfo WHERE MapNumber ='" + mapIndex + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    scale[0] = Convert.ToInt32(DB_RS.Fields["MapLenght"].Value);
                    scale[1] = Convert.ToInt32(DB_RS.Fields["MapWidth"].Value);
                    DB_RS.Close();
                    CheckQueue(mapsInfoTable);
                }
                return scale;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                mapsInfoTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                return GetMapScale(mapIndex);
            }
        }

        public float[][] GetSpawnPos(int mapIndex)
        {
            try
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
                    CheckQueue(mapsInfoTable);
                }
                spawnPos[0] = firstSpawnPos;
                spawnPos[1] = secondSpawnPos;
                return spawnPos;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                mapsInfoTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                return GetSpawnPos(mapIndex);
            }
        }

        public float[][] GetSpawnRot(int mapIndex)
        {
            try
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
                    CheckQueue(mapsInfoTable);
                }
                spawnRot[0] = firstSpawnRot;
                spawnRot[1] = secondSpawnRot;
                return spawnRot;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                mapsInfoTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                return GetSpawnRot(mapIndex);
            }
        }

        #endregion

        #region SkillBuilds

        List<object> skillBuildsTable = new List<object>();

        public void SetSkillBuildData(string nickname)
        {
            try
            {
                var DB_RS = Global.mysql.DB_RS;
                {
                    DB_RS.Open("Select * FROM SkillBuilds WHERE 0=1", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    DB_RS.AddNew();
                    DB_RS.Fields["Nickname"].Value = nickname;
                    for (int i = 0; i < (int)Constants.SPELLCOUNT.Ignis; i++)
                    {
                        DB_RS.Fields[Constants.FIRST_RACE_NAME+ " " + i.ToString() + " Spell"].Value = 0;
                    }
                    DB_RS.Update();
                    DB_RS.Close();
                    CheckQueue(skillBuildsTable);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                skillBuildsTable.Add(o);
                lock(o)
                {
                    Monitor.Wait(o);
                }
                SetSkillBuildData(nickname);
            }
        }

        public void SetSkillBuildData(string nickname, string raceName, int[] skillsIndexes)
        {
            try
            {
                var DB_RS = Global.mysql.DB_RS;
                {
                    DB_RS.Open("Select * FROM SkillBuilds WHERE Nickname='" + nickname + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    foreach (int i in skillsIndexes)
                    {
                        DB_RS.Fields[raceName + " " + Array.IndexOf(skillsIndexes, i).ToString() + " Spell"].Value = skillsIndexes[Array.IndexOf(skillsIndexes, i)];
                    }
                    DB_RS.Update();
                    DB_RS.Close();
                    CheckQueue(skillBuildsTable);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                skillBuildsTable.Add(o);
                lock (o)
                {
                    Monitor.Wait(o);
                }
                SetSkillBuildData(nickname, raceName, skillsIndexes);
            }
        }

        public int[] GetSkillBuildData(string nickname, string raceName)
        {
            try
            {
                int skillCount;
                switch(raceName)
                {
                    case "Ignis":
                        skillCount = (int)Constants.SPELLCOUNT.Ignis;
                        break;

                    case "Terra":
                        skillCount = (int)Constants.SPELLCOUNT.Terra;
                        break;

                    case "Caeli":
                        skillCount = (int)Constants.SPELLCOUNT.Caeli;
                        break;

                    case "Aqua":
                        skillCount = (int)Constants.SPELLCOUNT.Aqua;
                        break;

                    default:
                        skillCount = 0;
                        break;
                }
                int[] skillsNumbers = new int[skillCount];
                var DB_RS = Global.mysql.DB_RS;
                {
                    DB_RS.Open("SELECT * FROM SkillBuilds WHERE Nickname='" + nickname + "'", Global.mysql.DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    for(int i = 0; i < skillCount; i++)
                    {
                        skillsNumbers[i] = Convert.ToInt32(DB_RS.Fields[raceName + " " + i.ToString() + " Spell"].Value);
                    }
                    DB_RS.Close();
                    CheckQueue(skillBuildsTable);
                }
                return skillsNumbers;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                object o = new object();
                skillBuildsTable.Add(o);
                lock(o)
                {
                    Monitor.Wait(o);
                }
                return GetSkillBuildData(nickname, raceName);
            }
        }
        #endregion
    }
}
