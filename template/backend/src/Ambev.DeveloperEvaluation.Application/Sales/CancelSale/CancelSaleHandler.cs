using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool?>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the CancelSaleCommandHandler
        /// </summary>
        public CancelSaleHandler(
            ISaleRepository saleRepository,
            IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the CancelSaleCommand by canceling the specified sale
        /// </summary>
        /// <param name="request">Command containing the sale ID to cancel</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response containing the canceled sale details, or null if not found</returns>
        public async Task<bool?> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            // Get the sale with its items
            var sale = await _saleRepository.GetByIdWithItemsAsync(request.Id, cancellationToken);

            if (sale is null)
                return null;

            // Check if sale is already cancelled
            if (sale.IsCancelled)
                throw new DomainException("Sale is already cancelled");

            // Check if sale can be cancelled (implement your business rules here)
            // For example, you might not allow cancellation after X days
            if ((DateTime.UtcNow - sale.Date).TotalDays > 30)
                throw new DomainException("Sales older than 30 days cannot be cancelled");

            // Cancel the sale
            sale.IsCancelled = true;
            sale.CancellationDate = DateTime.UtcNow;

            // Update in database
            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Publish event
            await _eventPublisher.PublishAsync(new SaleCancelledEvent
            {
                SaleId = sale.Id,
                CancellationDate = sale.CancellationDate.Value,
            });

            return true;
        }
    }
}
