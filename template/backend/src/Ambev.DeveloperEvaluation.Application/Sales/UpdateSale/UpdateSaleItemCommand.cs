namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleItemCommand
    {
        /// <summary>
        /// Unique identifier of the sale item
        /// </summary>
        public Guid? Id { get; init; }

        /// <summary>
        /// Product name associated with this item
        /// </summary>
        public string ProductName { get; init; }

        /// <summary>
        /// Quantity of the product purchased
        /// </summary>
        public int Quantity { get; init; }

        /// <summary>
        /// Unit price of the product at the time of sale
        /// </summary>
        public decimal UnitPrice { get; init; }

        /// <summary>
        /// Flag indicating whether this item should be canceled
        /// </summary>
        public bool IsCancelled { get; init; }
    }
}
