namespace Ambev.DeveloperEvaluation.Domain.Events.Sales
{
    public class SaleCreatedEvent
    {
        public Guid SaleId { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public List<SaleItemEventData> Items { get; set; } = new List<SaleItemEventData>();
    }
}
