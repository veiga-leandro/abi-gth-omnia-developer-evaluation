using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Common.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(ILogger<EventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T @event) where T : class
        {
            _logger.LogInformation($"Event published: {@event.GetType().Name} - {JsonSerializer.Serialize(@event)}");
            return Task.CompletedTask;
        }
    }
}
