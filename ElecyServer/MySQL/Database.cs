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
                        Levels = new int[] { 0, 0, 0, 0, 0 },
                        Ranks = new int[] { 0, 0, 0, 0, 0 }
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
                            data[0] = a.AccountParameters.Levels.ToArray<int>();
                            data[1] = a.AccountParameters.Ranks.ToArray<int>();
                            break;
                        }
                    }
                }
                return data;
            }
        }

        #endregion

        #region Maps info

        public void AddMap(int MapIndex, int mh, int mw, float[] fsp, float[] ssp, float[] fsr, float[] ssr)
        {
            lock (_mapsExpectant)
            {
                using(MapsContext db = new MapsContext())
                {
                    Map _m = new Map()
                    {
                        Id = MapIndex,
                        MapHeight = mh,
                        MapWidth = mw,
                        SpawnPoints = new List<SpawnPoint>()
                    };
                    db.Maps.Add(_m);
                    db.SaveChanges();
                    SpawnPoint _newFirstSpawnPoint = new SpawnPoint()
                    {
                        PositionX = fsp[0],
                        PositionY = fsp[1],
                        RotationX = fsr[0],
                        RotationY = fsr[1],
                        RotationZ = fsr[2],
                        RotationW = fsr[3],
                        Map = _m
                    };
                    SpawnPoint _newSecondSpawnPoint = new SpawnPoint()
                    {
                        PositionX = ssp[0],
                        PositionY = ssp[1],
                        RotationX = ssr[0],
                        RotationY = ssr[1],
                        RotationZ = ssr[2],
                        RotationW = ssr[3],
                        Map = _m
                    };
                    _m.SpawnPoints = new List<SpawnPoint>() { _newFirstSpawnPoint, _newSecondSpawnPoint };
                    db.SpawnPoints.AddRange(new List<SpawnPoint>() { _newFirstSpawnPoint, _newSecondSpawnPoint});
                    db.SaveChanges();
                }
            }
        }

        public Map GetMap(int mapIndex)
        {
            lock(_mapsExpectant)
            {
                int[] scale = new int[2];
                using(MapsContext db = new MapsContext())
                {
                    foreach (Map m in db.Maps.Include(m => m.SpawnPoints))
                    {
                        if (m.Id.Equals(mapIndex))
                        {
                            return m;
                        }
                    }
                }
                return null;
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
                    try
                    {
                        skillsNumbers[i] = Convert.ToInt16(_spells[i].Substring(0, 4));
                    }
                    catch
                    {
                        skillsNumbers[i] = 0;
                    }
                }
                return skillsNumbers;
            }
        }

        #endregion

    }
}
