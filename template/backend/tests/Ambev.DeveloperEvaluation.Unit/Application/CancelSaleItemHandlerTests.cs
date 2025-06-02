using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CancelSaleItemHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly CancelSaleItemHandler _handler;

        public CancelSaleItemHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _handler = new CancelSaleItemHandler(_saleRepository, _eventPublisher);
        }

        [Fact]
        public async Task Handle_ShouldCancelItem_WhenItemExists()
        {
            // Arrange
            var command = CancelSaleItemHandlerTestData.GenerateValidCommand();
            var sale = MapCancelSaleItemCommandToSale(command, 2);

            _saleRepository.GetByIdWithItemsAsync(command.SaleId, Arg.Any<CancellationToken>())
                               .Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                               .Returns(Task.CompletedTask);

            // Act
            var cancelSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            cancelSaleResult.Should().NotBeNull();
            sale.Items.First(i => i.Id == command.ItemId).IsCancelled.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenItemItsOnlyOneInSale()
        {
            // Arrange
            var command = CancelSaleItemHandlerTestData.GenerateValidCommand();
            var sale = MapCancelSaleItemCommandToSale(command, 1);

            _saleRepository.GetByIdWithItemsAsync(command.SaleId, Arg.Any<CancellationToken>())
                               .Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                               .Returns(Task.CompletedTask);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<DomainException>().WithMessage("Cannot cancel the only active item in a sale. Consider cancelling the entire sale instead.");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenItemIsAlreadyCancelled()
        {
            // Arrange
            var command = CancelSaleItemHandlerTestData.GenerateValidCommand();
            var sale = MapCancelSaleItemCommandToSale(command, 2);
            sale.Items.First().IsCancelled = true; // Mark the item as cancelled

            _saleRepository.GetByIdWithItemsAsync(command.SaleId, Arg.Any<CancellationToken>())
                               .Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                               .Returns(Task.CompletedTask);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<DomainException>().WithMessage("This item is already cancelled");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenSaleIsAlreadyCancelled()
        {
            // Arrange
            var command = CancelSaleItemHandlerTestData.GenerateValidCommand();
            var sale = MapCancelSaleItemCommandToSale(command, 2);
            sale.IsCancelled = true; // Mark the item as cancelled

            _saleRepository.GetByIdWithItemsAsync(command.SaleId, Arg.Any<CancellationToken>())
                               .Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                               .Returns(Task.CompletedTask);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<DomainException>().WithMessage("Cannot modify items of a cancelled sale");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenItemNotFoundInSale()
        {
            // Arrange
            var command = new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid());
            var sale = MapCancelSaleItemCommandToSale(command, 1);
            sale.Items.First().Id = Guid.NewGuid();

            _saleRepository.GetByIdWithItemsAsync(command.SaleId, Arg.Any<CancellationToken>())
                               .Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                               .Returns(Task.CompletedTask);

            // Act
            var cancelSaleItemResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            cancelSaleItemResult.Should().BeNull();
        }


        private static Sale MapCancelSaleItemCommandToSale(CancelSaleItemCommand command, int items)
        {
            var sale = new Sale("Sale-01", DateTime.UtcNow, "Branch-01", new User { Id = Guid.NewGuid() })
            {
                Id = command.SaleId // Set the ID from the command
            };

            for (var i = 0; i < items; i++)
            {
                var saleItem = new SaleItem(sale.Id, $"Product-{i + 1:D2}", 1, 100m)
                {
                    Id = Guid.NewGuid() // Generate a new ID for each item
                };
                saleItem.CalculateDiscount(); // Assuming this method sets the discount if applicable
                sale.AddItem(saleItem);
            }
            sale.Items.First().Id = command.ItemId; // Set the ID of the item to be cancelled

            return sale;
        }
    }
}
