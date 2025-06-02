using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Events
{
    public class EventPublisherTests
    {
        private readonly EventPublisher _eventPublisher;
        private readonly ILogger<EventPublisher> _logger;
        public EventPublisherTests()
        {
            _logger = Substitute.For<ILogger<EventPublisher>>();
            _eventPublisher = new EventPublisher(_logger);
        }

        [Fact]
        public void PublishSaleCreatedEvent_ShouldLogEvent_WhenSaleIsCreated()
        {
            // Arrange
            var saleCreatedEvent = new SaleCreatedEvent
            {
                SaleId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                SaleNumber = "Sale-001",
                Items = new List<SaleItemEventData>
                {
                    new SaleItemEventData
                    {
                        ProductName = "Product A",
                        Quantity = 2,
                        UnitPrice = 10.0m,
                        Discount = 0.0m,
                        TotalAmount = 20.0m
                    }
                }
            };

            string expectedLogMessage = $"Event published: {saleCreatedEvent.GetType().Name} - {JsonSerializer.Serialize(saleCreatedEvent)}";

            // Act
            _eventPublisher.PublishAsync(saleCreatedEvent);

            // Assert
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(expectedLogMessage)),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void PublishSaleModifiedEvent_ShouldLogEvent_WhenSaleIsModified()
        {
            // Arrange
            var saleModifiedEvent = new SaleModifiedEvent
            {
                SaleId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                SaleNumber = "Sale-001",
                Items = new List<SaleItemEventData>
                {
                    new SaleItemEventData
                    {
                        ProductName = "Product A",
                        Quantity = 2,
                        UnitPrice = 10.0m,
                        Discount = 0.0m,
                        TotalAmount = 20.0m
                    }
                }
            };

            string expectedLogMessage = $"Event published: {saleModifiedEvent.GetType().Name} - {JsonSerializer.Serialize(saleModifiedEvent)}";

            // Act
            _eventPublisher.PublishAsync(saleModifiedEvent);

            // Assert
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(expectedLogMessage)),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void PublishSaleCancelledEvent_ShouldLogEvent_WhenSaleIsCancelled()
        {
            // Arrange
            var saleModifiedEvent = new SaleCancelledEvent
            {
                SaleId = Guid.NewGuid()                
            };

            string expectedLogMessage = $"Event published: {saleModifiedEvent.GetType().Name} - {JsonSerializer.Serialize(saleModifiedEvent)}";

            // Act
            _eventPublisher.PublishAsync(saleModifiedEvent);

            // Assert
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(expectedLogMessage)),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void PublishItemCancelledEvent_ShouldLogEvent_WhenItemIsCancelled()
        {
            // Arrange
            var saleModifiedEvent = new ItemCancelledEvent
            {
                SaleId = Guid.NewGuid(),
                SaleItemId = Guid.NewGuid(),
            };

            string expectedLogMessage = $"Event published: {saleModifiedEvent.GetType().Name} - {JsonSerializer.Serialize(saleModifiedEvent)}";

            // Act
            _eventPublisher.PublishAsync(saleModifiedEvent);

            // Assert
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(expectedLogMessage)),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }
    }
}
