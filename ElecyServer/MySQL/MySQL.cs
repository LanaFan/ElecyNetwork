using System.Data.Entity;

namespace ElecyServer
{
    public class AccountsContext : DbContext
    {
        public AccountsContext() 
            : base ("ServerConnection")
        { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountParameters> AccountsParameters { get; set; }

        public DbSet<AccountSkillBuilds> AccountSkillBuilds { get; set; }
    }

    public class MapsContext : DbContext
    {
        public MapsContext()
            : base ("ServerConnection")
        { }

        public DbSet<Map> Maps { get; set; }

        public DbSet<SpawnPoint> SpawnPoints { get; set; }
    }
}
