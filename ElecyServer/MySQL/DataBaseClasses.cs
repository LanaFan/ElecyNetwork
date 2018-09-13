﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ElecyServer
{
    public class Map
    {
        public int Id { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        public ICollection<SpawnPoint> SpawnPoints { get; set; }
    }

    public class SpawnPoint
    {
        public int Id { get; set; }

        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }

        public int? MapId { get; set; }

        public Map Map { get; set; }
    }

    public class Account
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }

        public AccountParameters AccountParameters { get; set; }

        public AccountSkillBuilds AccountSkillBuilds { get; set; }
    }

    public class AccountParameters
    {
        [Key]
        [ForeignKey("Account")]
        public int Id { get; set; }

        public IEnumerable<int> Levels
        {
            get
            {
                var tab = LevelsString.Split(',');
                return tab.Select(int.Parse).AsEnumerable();
            }
            set
            {
                LevelsString = string.Join(",", value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string LevelsString { get; set; }

        public IEnumerable<int> Ranks
        {
            get
            {
                var tab = RanksString.Split(',');
                return tab.Select(int.Parse).AsEnumerable();
            }
            set
            {
                RanksString = string.Join(",", value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string RanksString { get; set; }

        public Account Account { get; set; }
    }

    public class AccountSkillBuilds
    {
        [Key]
        [ForeignKey("Account")]
        public int Id { get; set; }

        public IEnumerable<string> IgnisBuild
        {
            get
            {
                var tab = IgnisString.Split(',');
                return tab.AsEnumerable();
            }
            set
            {
                IgnisString = string.Join(",", value);
            }
        }

        public IEnumerable<string> TerraBuild
        {
            get
            {
                var tab = TerraString.Split(',');
                return tab.AsEnumerable();
            }
            set
            {
                TerraString = string.Join(",", value);
            }
        }

        public IEnumerable<string> AquaBuild
        {
            get
            {
                var tab = AquaString.Split(',');
                return tab.AsEnumerable();
            }
            set
            {
                AquaString = string.Join(",", value);
            }
        }

        public IEnumerable<string> CaeliBuild
        {
            get
            {
                var tab = CaeliString.Split(',');
                return tab.AsEnumerable();
            }
            set
            {
                CaeliString = string.Join(",", value);
            }
        }

        public IEnumerable<string> PrimusBuild
        {
            get
            {
                var tab = PrimusString.Split(',');
                return tab.AsEnumerable();
            }
            set
            {
                PrimusString = string.Join(",", value);
            }
        }

        public Account Account { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string IgnisString { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TerraString { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string AquaString { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CaeliString { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string PrimusString { get; set; }
    }
}
