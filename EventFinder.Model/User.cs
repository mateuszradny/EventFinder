using System.Collections.Generic;

namespace EventFinder.Model
{
    public class User : EntityBase
    {
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}