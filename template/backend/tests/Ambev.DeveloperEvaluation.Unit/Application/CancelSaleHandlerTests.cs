using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CancelSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly CancelSaleHandler _handler;

        public CancelSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _handler = new CancelSaleHandler(_saleRepository, _eventPublisher);
        }

        [Fact]
        public async Task Handle_ShouldCancelSale()
        {
            // Arrange
            var command = CancelSaleHandlerTestData.GenerateValidCommand();
            var sale = MapCancelSaleCommandToSale(command);

            _saleRepository.GetByIdWithItemsAsync(command.Id, Arg.Any<CancellationToken>())
                               .Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                               .Returns(Task.CompletedTask);

            // Act
            var cancelSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            cancelSaleResult.Should().NotBeNull();
            sale.IsCancelled.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenSaleNotFound()
        {
            // Arrange
            var command = CancelSaleHandlerTestData.GenerateValidCommand();

            _saleRepository.GetByIdWithItemsAsync(command.Id, Arg.Any<CancellationToken>())
                               .Returns((Sale)null);

            // Act
            var cancelSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            cancelSaleResult.Should().BeNull();
        }


        private static Sale MapCancelSaleCommandToSale(CancelSaleCommand command)
        {
            var sale = new Sale("Sale-01", DateTime.UtcNow, "Branch-01", new User { Id = Guid.NewGuid() });
            sale.Id = command.Id; // Set the ID from the command
            return sale;
        }
    }
}
