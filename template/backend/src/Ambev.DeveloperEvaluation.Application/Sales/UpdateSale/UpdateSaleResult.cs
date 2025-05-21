namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleResult
    {
        /// <summary>
        /// Unique identifier of the updated sale
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Sale number displayed to users
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Date when the sale was created/updated
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Customer ID who made the purchase
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Branch name for display purposes
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Total amount of the sale
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Collection of items in this sale
        /// </summary>
        public List<UpdateSaleItemResult> Items { get; set; } = new List<UpdateSaleItemResult>();
    }
}
