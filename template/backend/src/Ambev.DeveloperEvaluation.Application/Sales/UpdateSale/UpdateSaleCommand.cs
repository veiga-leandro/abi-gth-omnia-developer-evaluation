using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        /// <summary>
        /// Unique identifier of the sale to update
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Date when the sale occurred
        /// </summary>
        public DateTime Date { get; init; }

        /// <summary>
        /// Customer ID associated with this sale
        /// </summary>
        public Guid CustomerId { get; init; }

        /// <summary>
        /// Branch name where the sale was processed
        /// </summary>
        public string BranchName { get; init; }

        /// <summary>
        /// Collection of items in this sale
        /// </summary>
        public List<UpdateSaleItemCommand> Items { get; } = new List<UpdateSaleItemCommand>();
    }
}
