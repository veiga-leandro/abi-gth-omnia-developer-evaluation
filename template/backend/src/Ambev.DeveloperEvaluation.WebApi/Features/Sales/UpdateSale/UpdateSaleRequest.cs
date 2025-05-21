namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequest
    {
        /// <summary>
        /// Unique identifier of the sale to update
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date when the sale occurred
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Customer ID associated with this sale
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Branch name where the sale was processed
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Collection of items in this sale
        /// </summary>
        public List<UpdateSaleItemRequest> Items { get; set; } = new List<UpdateSaleItemRequest>();
    }
}
