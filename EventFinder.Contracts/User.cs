using System.Collections.Generic;

namespace EventFinder.Contracts
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}