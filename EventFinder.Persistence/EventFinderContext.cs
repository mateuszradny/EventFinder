using EventFinder.Model;
using System.Data.Entity;

namespace EventFinder.Persistence
{
    public class EventFinderContext : DbContext
    {
        public EventFinderContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Event> Events { get; set; }
    }
}