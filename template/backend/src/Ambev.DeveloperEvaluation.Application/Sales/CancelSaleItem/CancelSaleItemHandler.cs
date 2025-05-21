using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, bool?>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the CancelSaleCommandHandler
        /// </summary>
        public CancelSaleItemHandler(
            ISaleRepository saleRepository,
            IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the CancelSaleItemCommand by canceling the specified item from a sale
        /// </summary>
        /// <param name="request">Command containing the sale ID and item ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result indicating success or failure</returns>
        public async Task<bool?> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
        {
            // Get the sale with its items
            var sale = await _saleRepository.GetByIdWithItemsAsync(request.SaleId, cancellationToken);

            if (sale is null)
                return null;

            // Check if sale is already cancelled
            if (sale.IsCancelled)
                throw new DomainException("Cannot modify items of a cancelled sale");

            // Find the item in the sale
            var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId);

            if (item is null)
                return null;

            // Check if item is already cancelled
            if (item.IsCancelled)
                throw new DomainException("This item is already cancelled");

            // Check if there would be at least one active item left
            var activeItemsCount = sale.Items.Count(i => !i.IsCancelled);
            if (activeItemsCount <= 1)
                throw new DomainException("Cannot cancel the only active item in a sale. Consider cancelling the entire sale instead.");

            // Mark the item as cancelled instead of removing it
            item.IsCancelled = true;
            item.CancellationDate = DateTime.UtcNow;

            // Recalculate the total amount (excluding cancelled items)
            sale.RecalculateTotalAmount();

            // Update the sale in the database
            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Publish event
            await _eventPublisher.PublishAsync(new ItemCancelledEvent
            {
                SaleId = sale.Id,
                SaleItemId = item.Id,
                CancellationDate = item.CancellationDate.Value,
            });

            return true;
        }
    }
}
