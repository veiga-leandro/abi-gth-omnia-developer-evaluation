namespace Ambev.DeveloperEvaluation.Domain.Events.Sales
{
    public class SaleCancelledEvent
    {
        public Guid SaleId { get; set; }
        public DateTime CancellationDate { get; set; }
    }
}
