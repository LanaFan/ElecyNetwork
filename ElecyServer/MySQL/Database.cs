using System;
using System.Collections.Generic;
using Bindings;
using System.Threading;
using System.Data.Entity;
using System.Linq;


namespace ElecyServer
{
    class Database
    {
        private object _accountsExpectant = new object();
        private object _mapsExpectant = new object();
        private object _skillBuildsExpectant = new object();

        #region Accounts

        public bool LoginExist(string username)
        {
            lock(_accountsExpectant)
            {
                using(AccountsContext db = new AccountsContext())
                {
                    var _accounts = db.Accounts;
                    foreach(Account a in _accounts)
                    {
                        if(a.Login.Equals(username))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool NicknameExist(string nickname)
        {
            lock (_accountsExpectant)
            {
                using (AccountsContext db = new AccountsContext())
                {
                    var _accounts = db.Accounts;
                    foreach (Account a in _accounts)
                    {
                        if (a.Nickname.Equals(nickname))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool PasswordIsOkay(string username, string password)
        {
            lock (_accountsExpectant)
            {
                using (AccountsContext db = new AccountsContext())
                {
                    var _accounts = db.Accounts;
                    foreach (Account a in _accounts)
                    {
                        if (a.Login.Equals(username))
                        {
                            if(a.Password.Equals(password))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return false;
            }
        }

        public string GetAccountNickname(string username)
        {
            lock (_accountsExpectant)
            {
                using (AccountsContext db = new AccountsContext())
                {
                    var _accounts = db.Accounts;
                    foreach (Account a in _accounts)
                    {
                        if (a.Login.Equals(username))
                        {
                            return a.Nickname;
                        }
                    }
                }
                return null;
            }
        }

        public void AddAccount(string username, string password, string nickname)
        {
            lock(_accountsExpectant)
            {
                using(AccountsContext db = new AccountsContext())
                {
                    Account _newAccount = new Account() { Login = username, Password = password, Nickname = nickname };
                    db.Accounts.Add(_newAccount);
                    db.SaveChanges();
                    AccountParameters _newAccountParameters = new AccountParameters()
                    {
                        Id = _newAccount.Id,
                        Levels = new int[5] { 0, 0, 0, 0, 0 },
                        Ranks = new int[5] { 0, 0, 0, 0, 0 },
                        Account = _newAccount
                    };
                    db.AccountsParameters.Add(_newAccountParameters);
                    db.SaveChanges();
                    AccountSkillBuilds _newAccountSkillBuilds = new AccountSkillBuilds()
                    {
                        Id = _newAccount.Id,
                        IgnisBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null },
                        TerraBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null },
                        AquaBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null },
                        CaeliBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null },
                        PrimusBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null }
                    };
                    _newAccount = db.Accounts.Find(_newAccount.Id);
                    _newAccount.AccountParameters = _newAccountParameters;
                    _newAccount.AccountSkillBuilds = _newAccountSkillBuilds;
                    db.Entry(_newAccount).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region Accounts Parameters

        public void SetAccountData(string nickname, int[] levels, int[] ranks)
        {
            lock(_accountsExpectant)
            {
                using(AccountsContext db = new AccountsContext())
                {
                    foreach (Account a in db.Accounts.Include(b => b.AccountParameters))
                    {
                        if(a.Nickname.Equals(nickname))
                        {
                            a.AccountParameters.Levels = levels;
                            a.AccountParameters.Ranks = ranks;
                            db.Entry(a).State = EntityState.Modified;
                            db.SaveChanges();
                            return;
                        }
                    }
                }
            }
        }

        public int[][] GetAccountData(string nickname)
        {
            lock (_accountsExpectant)
            {
                int[][] data = new int[2][];
                using (AccountsContext db = new AccountsContext())
                {
                    foreach (Account a in db.Accounts.Include(b=>b.AccountParameters))
                    {
                        if (a.Nickname.Equals(nickname))
                        {
                            data[0] = a.AccountParameters.Levels as int[];
                            data[1] = a.AccountParameters.Ranks as int[];
                            break;
                        }
                    }
                }
                return data;
            }
        }

        #endregion

        #region Maps info

        public int[] GetMapScale(int mapIndex)
        {
            lock(_mapsExpectant)
            {
                int[] scale = new int[2];
                using(MapsContext db = new MapsContext())
                {
                    Map m = db.Maps.Find(mapIndex);
                    if(m != null)
                    {
                        scale[0] = m.MapHeight;
                        scale[1] = m.MapWidth;
                    }
                    return scale;
                }
            }
        }

        public float[][] GetSpawnPos(int mapIndex)
        {
            lock(_mapsExpectant)
            {
                float[][] spawnPos = new float[2][];
                float[] firstSpawnPos = new float[2];
                float[] secondSpawnPos = new float[2];
                using (MapsContext db = new MapsContext())
                {
                    Map m = db.Maps.Find(mapIndex);
                    if (m != null)
                    {
                        firstSpawnPos[0] = m.FirstSpawnPointX;
                        firstSpawnPos[1] = m.FirstSpawnPointZ;
                        secondSpawnPos[0] = m.SecondSpawnPointX;
                        secondSpawnPos[1] = m.SecondSpawnPointZ;
                    }
                }
                spawnPos[0] = firstSpawnPos;
                spawnPos[1] = secondSpawnPos;
                return spawnPos;
            }
        }

        public float[][] GetSpawnRot(int mapIndex)
        {
            lock (_mapsExpectant)
            {
                float[][] spawnRot = new float[2][];
                float[] firstSpawnRot = new float[4];
                float[] secondSpawnRot = new float[4];
                using (MapsContext db = new MapsContext())
                {
                    Map m = db.Maps.Find(mapIndex);
                    if (m != null)
                    {
                        firstSpawnRot[0] = m.FirstSpawnRotationX;
                        firstSpawnRot[1] = m.FirstSpawnRotationY;
                        firstSpawnRot[2] = m.FirstSpawnRotationZ;
                        firstSpawnRot[3] = m.FirstSpawnRotationW;
                        secondSpawnRot[0] = m.SecondSpawnRotationX;
                        secondSpawnRot[1] = m.SecondSpawnRotationY;
                        secondSpawnRot[2] = m.SecondSpawnRotationZ;
                        secondSpawnRot[3] = m.SecondSpawnRotationW;
                    }
                }
                spawnRot[0] = firstSpawnRot;
                spawnRot[1] = secondSpawnRot;
                return spawnRot;
            }
        }

        #endregion

        #region SkillBuilds

        public void SetSkillBuildData(string nickname)
        {
            lock(_skillBuildsExpectant)
            {
                using (AccountsContext db = new AccountsContext())
                {
                    foreach(Account a in db.Accounts.Include(b=>b.AccountSkillBuilds))
                    {
                        if(a.Nickname.Equals(nickname))
                        {
                            a.AccountSkillBuilds.IgnisBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null };
                            a.AccountSkillBuilds.AquaBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null };
                            a.AccountSkillBuilds.TerraBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null };
                            a.AccountSkillBuilds.CaeliBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null };
                            a.AccountSkillBuilds.PrimusBuild = new string[9] { "0000000", null, null, null, null, null, null, null, null };
                            db.Entry(a).State = EntityState.Modified;
                            db.SaveChanges();
                            return;
                        }
                    }
                }
            }
        }

        public void SetSkillBuildData(string nickname, string raceName, string[] skillsIndexes)
        {
            lock(_skillBuildsExpectant)
            {
                using(AccountsContext db = new AccountsContext())
                {
                    switch (raceName)
                    {
                        case "Ignis":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    a.AccountSkillBuilds.IgnisBuild = skillsIndexes;
                                    db.Entry(a).State = EntityState.Modified;
                                    db.SaveChanges();
                                    break;
                                }
                            }
                                    break;

                        case "Terra":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    a.AccountSkillBuilds.TerraBuild = skillsIndexes;
                                    db.Entry(a).State = EntityState.Modified;
                                    db.SaveChanges();
                                    break;
                                }
                            }
                            break;

                        case "Caeli":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    a.AccountSkillBuilds.CaeliBuild = skillsIndexes;
                                    db.Entry(a).State = EntityState.Modified;
                                    db.SaveChanges();
                                    break;
                                }
                            }
                            break;

                        case "Aqua":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    a.AccountSkillBuilds.AquaBuild = skillsIndexes;
                                    db.Entry(a).State = EntityState.Modified;
                                    db.SaveChanges();
                                    break;
                                }
                            }
                            break;

                        case "Primus":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    a.AccountSkillBuilds.PrimusBuild = skillsIndexes;
                                    db.Entry(a).State = EntityState.Modified;
                                    db.SaveChanges();
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        public short[] GetSkillBuildData(string nickname, string raceName)
        {
            lock(_skillBuildsExpectant)
            {
                string[] _spells = new string[1];
                using(AccountsContext db = new AccountsContext())
                {
                    switch (raceName)
                    {
                        case "Ignis":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    _spells = a.AccountSkillBuilds.IgnisBuild as string[];
                                    break;
                                }
                            }
                            break;

                        case "Terra":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    _spells = a.AccountSkillBuilds.TerraBuild as string[];
                                    break;
                                }
                            }
                            break;

                        case "Caeli":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    _spells = a.AccountSkillBuilds.CaeliBuild as string[];
                                    break;
                                }
                            }
                            break;

                        case "Aqua":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    _spells = a.AccountSkillBuilds.AquaBuild as string[];
                                    break;
                                }
                            }
                            break;

                        case "Primus":
                            foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
                            {
                                if (a.Nickname.Equals(nickname))
                                {
                                    _spells = a.AccountSkillBuilds.PrimusBuild as string[];
                                    break;
                                }
                            }
                            break;

                        default:
                            return null;
                    }
                }
                short[] skillsNumbers = new short[_spells.Length];
                for (int i = 0; i < skillsNumbers.Length; i++)
                {
                        skillsNumbers[i] = Convert.ToInt16(_spells[i].Substring(0, 4));
                }
                return skillsNumbers;
            }
        }

        #endregion

    }
}
