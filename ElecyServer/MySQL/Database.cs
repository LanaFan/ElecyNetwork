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

        private string GetGuideKey(int lenght)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345789";
            return new string(Enumerable.Repeat(chars, lenght).Select(s=>s[random.Next(s.Length)]).ToArray());
        }

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

        public Account GetAccount(string username)
        {
            lock (_accountsExpectant)
            {
                using (AccountsContext db = new AccountsContext())
                {
                    var _accounts = db.Accounts;
                    foreach (Account a in _accounts.Include(a=>a.AccountParameters).Include(b=>b.AccountSkillBuilds))
                    {
                        if (a.Login.Equals(username))
                        {
                            return a;
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
                    Account _newAccount = new Account() { Login = username, Password = password, Nickname = nickname, GuideKey = GetGuideKey(5) };
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
                        IgnisBuild = new short[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        TerraBuild = new short[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        AquaBuild = new short[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        CaeliBuild = new short[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        PrimusBuild = new short[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
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

        public void SaveAccount(Account account)
        {
            lock(_accountsExpectant)
            {
                using(AccountsContext db = new AccountsContext())
                {
                    db.Entry(account).State = EntityState.Modified;

                    db.Entry(account.AccountParameters).State = EntityState.Modified;

                    db.Entry(account.AccountSkillBuilds).State = EntityState.Modified;

                    db.SaveChanges();
                }
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

        //public void SetSkillBuildData(string nickname, string raceName, string[] skillsIndexes)
        //{
        //    lock(_skillBuildsExpectant)
        //    {
        //        using(AccountsContext db = new AccountsContext())
        //        {
        //            switch (raceName)
        //            {
        //                case "Ignis":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            a.AccountSkillBuilds.IgnisBuild = skillsIndexes;
        //                            db.Entry(a).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                case "Terra":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            a.AccountSkillBuilds.TerraBuild = skillsIndexes;
        //                            db.Entry(a).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                case "Caeli":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            a.AccountSkillBuilds.CaeliBuild = skillsIndexes;
        //                            db.Entry(a).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                case "Aqua":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            a.AccountSkillBuilds.AquaBuild = skillsIndexes;
        //                            db.Entry(a).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                case "Primus":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            a.AccountSkillBuilds.PrimusBuild = skillsIndexes;
        //                            db.Entry(a).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        //}

        //public short[] GetSkillBuildData(string nickname, string raceName)
        //{
        //    lock(_skillBuildsExpectant)
        //    {
        //        string[] _spells = new string[1];
        //        using(AccountsContext db = new AccountsContext())
        //        {
        //            switch (raceName)
        //            {
        //                case "Ignis":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            _spells = a.AccountSkillBuilds.IgnisBuild as string[];
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                case "Terra":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            _spells = a.AccountSkillBuilds.TerraBuild as string[];
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                case "Caeli":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            _spells = a.AccountSkillBuilds.CaeliBuild as string[];
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                case "Aqua":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            _spells = a.AccountSkillBuilds.AquaBuild as string[];
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                case "Primus":
        //                    foreach (Account a in db.Accounts.Include(a => a.AccountSkillBuilds))
        //                    {
        //                        if (a.Nickname.Equals(nickname))
        //                        {
        //                            _spells = a.AccountSkillBuilds.PrimusBuild as string[];
        //                            break;
        //                        }
        //                    }
        //                    break;

        //                default:
        //                    return null;
        //            }
        //        }
        //        short[] skillsNumbers = new short[_spells.Length];
        //        for (int i = 0; i < skillsNumbers.Length; i++)
        //        {
        //            try
        //            {
        //                skillsNumbers[i] = Convert.ToInt16(_spells[i].Substring(0, 4));
        //            }
        //            catch
        //            {
        //                skillsNumbers[i] = 0;
        //            }
        //        }
        //        return skillsNumbers;
        //    }
        //}

        #endregion

    }
}
