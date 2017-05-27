using EventFinder.Contracts;
using EventFinder.Contracts.Services;
using EventFinder.Model;
using EventFinder.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinder.Application.Services
{
    public class EventService : IEventService
    {
        private readonly EventFinderContext _context;
        private readonly IUserService _userService;

        public EventService(EventFinderContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Event> Add(string name, string city, DateTime date, string description)
        {
            if (date < DateTime.Now)
                throw new InvalidOperationException("You can not add events that have ended.");

            var user = await _userService.CurrentUser;
            var @event = new Event
            {
                Name = name,
                City = city,
                Date = date,
                Description = description,
                UserId = user.Id
            };

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return @event;
        }

        public async Task<Event> Get(int id)
        {
            var @event = await _context.Events.SingleOrDefaultAsync(x => x.Id == id);
            if (@event == null)
                throw new InvalidOperationException($"Event with id {id} does not exists.");

            return @event;
        }

        public async Task<IEnumerable<Event>> GetByQuery(EventSearchQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (query.From > query.To)
                return await Task.FromResult(Enumerable.Empty<Event>());

            IQueryable<Event> queryable = _context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.City))
                queryable = queryable.Where(x => x.City.Trim() == query.City.Trim());

            if (query.From.HasValue)
                queryable = queryable.Where(x => x.Date >= query.From.Value);

            if (query.To.HasValue)
                queryable = queryable.Where(x => x.Date <= query.To.Value);

            if (!string.IsNullOrWhiteSpace(query.Tag))
                queryable = queryable.Where(x => x.Description.Contains("#" + query.Tag));

            return await queryable.ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetIncoming()
        {
            DateTime from = DateTime.Now.AddHours(-2);
            return await _context.Events.Where(x => x.Date >= from).OrderBy(x => x.Date).ToListAsync();
        }

        public async Task Remove(int id)
        {
            var @event = await GetEventIfCurrentUserIsOwner(id);

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
        }

        public async Task<Event> Update(int id, string name, string city, DateTime date, string description)
        {
            var @event = await GetEventIfCurrentUserIsOwner(id);

            @event.Name = name;
            @event.City = city;
            @event.Date = date;
            @event.Description = description;

            _context.Entry(@event).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return @event;
        }

        private async Task<Event> GetEventIfCurrentUserIsOwner(int id)
        {
            var @event = await _context.Events.SingleOrDefaultAsync(x => x.Id == id);
            if (@event == null)
                throw new InvalidOperationException($"Event with id {id} does not exists.");

            var user = await _userService.CurrentUser;
            if (@event.UserId != user.Id)
                throw new InvalidOperationException("You do not have permissions to this event.");

            return @event;
        }
    }
}