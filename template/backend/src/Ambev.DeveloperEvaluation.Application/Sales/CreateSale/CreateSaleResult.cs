namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleResult
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleItemResult> Items { get; set; } = new List<SaleItemResult>();
    }
}
