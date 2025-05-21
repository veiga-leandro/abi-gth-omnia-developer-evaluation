using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleCommand : IRequest<GetSaleResult>
    {
        /// <summary>
        /// The unique identifier of the sale to retrieve
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Initializes a new instance of the GetSaleByIdQuery
        /// </summary>
        /// <param name="id">Sale ID to retrieve</param>
        public GetSaleCommand(Guid id)
        {
            Id = id;
        }
    }
}
