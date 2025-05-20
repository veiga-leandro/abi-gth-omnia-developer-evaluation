using Ambev.DeveloperEvaluation.Application.Sales.SaleItem;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public DateTime Date { get; set; }
        public string BranchName { get; set; }
        public Guid CustomerId { get; set; }
        public List<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
    }
}
