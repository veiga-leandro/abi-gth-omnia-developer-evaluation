namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public string BranchName { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }

        public List<CreateSaleItemResponse> Items { get; set; } = new List<CreateSaleItemResponse>();
    }
}
