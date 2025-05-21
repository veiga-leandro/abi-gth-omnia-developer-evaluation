namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public DateTime Date { get; set; }
        public string BranchName { get; set; }
        public List<CreateSaleItemRequest> Items { get; set; } = new List<CreateSaleItemRequest>();
    }
}
