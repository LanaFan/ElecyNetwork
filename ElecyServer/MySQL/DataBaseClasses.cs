using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElecyServer
{
    public class Map
    {
        public int Id { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        public float FirstSpawnPointX { get; set; }
        public float FirstSpawnPointZ { get; set; }

        public float SecondSpawnPointX { get; set; }
        public float SecondSpawnPointZ { get; set; }

        public float FirstSpawnRotationX { get; set; }
        public float FirstSpawnRotationY { get; set; }
        public float FirstSpawnRotationZ { get; set; }
        public float FirstSpawnRotationW { get; set; }

        public float SecondSpawnRotationX { get; set; }
        public float SecondSpawnRotationY { get; set; }
        public float SecondSpawnRotationZ { get; set; }
        public float SecondSpawnRotationW { get; set; }
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

        public ICollection<int> Levels { get; set; }

        public ICollection<int> Ranks { get; set; }

        public Account Account { get; set; }
    }

    public class AccountSkillBuilds
    {
        [Key]
        [ForeignKey("Account")]
        public int Id { get; set; }

        public ICollection<string> IgnisBuild { get; set; }

        public ICollection<string> TerraBuild { get; set; }

        public ICollection<string> AquaBuild { get; set; }

        public ICollection<string> CaeliBuild { get; set; }

        public ICollection<string> PrimusBuild { get; set; }

        public Account Account { get; set; }
    }
}
