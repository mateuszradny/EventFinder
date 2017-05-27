using EventFinder.Contracts;
using EventFinder.Contracts.Services;
using EventFinder.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventFinder.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]Event request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Event @event = await _eventService.Add(request.Name, request.City, request.Date, request.Description);
                @event.User = null;
                return Json(@event);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Json(await _eventService.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("query")]
        public async Task<IActionResult> GetByQuery([FromBody]EventSearchQuery query)
        {
            try
            {
                return Json(await _eventService.GetByQuery(query));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("incoming")]
        public async Task<IActionResult> GetIncoming()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Json(await _eventService.GetIncoming());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _eventService.Remove(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]Event request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (id != request.Id)
                    throw new InvalidOperationException("Invalid event data.");

                await _eventService.Update(id, request.Name, request.City, request.Date, request.Description);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}