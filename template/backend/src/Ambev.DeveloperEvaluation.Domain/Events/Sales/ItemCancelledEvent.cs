namespace Ambev.DeveloperEvaluation.Domain.Events.Sales
{
    public class ItemCancelledEvent
    {
        public Guid SaleId { get; set; }
        public Guid SaleItemId { get; set; }
        public DateTime CancellationDate { get; set; }
    }
}
