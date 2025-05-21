using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemCommand : IRequest<bool?>
    {
        public CancelSaleItemCommand(Guid saleId, Guid itemId)
        {
            SaleId = saleId;
            ItemId = itemId;
        }

        public Guid SaleId { get; set; }
        public Guid ItemId { get; set; }
    }
}
