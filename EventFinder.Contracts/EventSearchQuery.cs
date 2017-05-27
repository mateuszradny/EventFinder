using System;

namespace EventFinder.Contracts
{
    public class EventSearchQuery
    {
        public string City { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Tag { get; set; }
    }
}