using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.EventFeed
{
    [Route("/events")]
    public class EventFeedController
    {
        private readonly IEventStore eventStore;

        public EventFeedController(IEventStore eventStore) => this.eventStore = eventStore;

        [HttpGet]
        public Event[] Get([FromQuery] long start, [FromQuery] long end = long.MaxValue) =>
          eventStore.GetEvents(start, end).ToArray();
    }
}