using EventFinder.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventFinder.Contracts.Services
{
    public interface IEventService
    {
        Task<Event> Add(string name, string city, DateTime date, string description);

        Task<Event> Get(int id);

        Task<IEnumerable<Event>> GetByQuery(EventSearchQuery query);

        Task<IEnumerable<Event>> GetIncoming();

        Task Remove(int id);

        Task<Event> Update(int id, string name, string city, DateTime date, string description);
    }
}