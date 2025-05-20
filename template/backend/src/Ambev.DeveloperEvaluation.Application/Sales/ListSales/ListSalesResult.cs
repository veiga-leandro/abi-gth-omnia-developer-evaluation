namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    public class ListSalesResult
    {
        /// <summary>
        /// Unique identifier of the sale
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Sale number displayed to users
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Date when the sale was created
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Customer ID who made the purchase
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Branch name where the sale was made
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Total amount of the sale
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Flag indicating whether the sale has been cancelled
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
