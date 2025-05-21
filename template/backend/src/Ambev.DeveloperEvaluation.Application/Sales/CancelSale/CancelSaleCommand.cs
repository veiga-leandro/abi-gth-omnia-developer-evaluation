using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleCommand : IRequest<bool?>
    {
        /// <summary>
        /// Unique identifier of the sale to cancel
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Initializes a new instance of the CancelSaleCommand
        /// </summary>
        /// <param name="id">Sale ID to cancel</param>
        public CancelSaleCommand(Guid id)
        {
            Id = id;
        }
    }
}
