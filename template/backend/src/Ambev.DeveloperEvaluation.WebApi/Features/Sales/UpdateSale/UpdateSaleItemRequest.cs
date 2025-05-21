namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleItemRequest
    {
        /// <summary>
        /// Unique identifier of the sale item
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Product name associated with this item
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
        /// Flag indicating whether this item should be removed from the sale
        /// </summary>
        public bool IsCancelled { get; set; }
    }

}
