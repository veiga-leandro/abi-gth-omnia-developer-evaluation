namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleItemResult
    {
        /// <summary>
        /// Unique identifier of the sale item
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Product name for display purposes
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Quantity of the product purchased
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price of the product at the time of sale
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Total price for this item (Quantity × UnitPrice)
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
