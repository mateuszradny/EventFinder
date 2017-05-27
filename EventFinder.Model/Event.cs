using System;

namespace EventFinder.Model
{
    public class Event : EntityBase
    {
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}