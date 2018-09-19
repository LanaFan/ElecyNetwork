using System.Data.Entity;

namespace ElecyServer
{
    public class AccountsContext : DbContext
    {
        public AccountsContext() 
            : base ("DataConnection")
        { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountParameters> AccountsParameters { get; set; }

        public DbSet<AccountSkillBuilds> AccountSkillBuilds { get; set; }
    }

    public class MapsContext : DbContext
    {
        public MapsContext()
            : base ("MapsConnection")
        { }

        public DbSet<Map> Maps { get; set; }

        public DbSet<SpawnPoint> SpawnPoints { get; set; }
    }

    public class BlackListContext : DbContext
    {
        public BlackListContext() 
            : base ("BlackListConnection")
        { }
    }
}
