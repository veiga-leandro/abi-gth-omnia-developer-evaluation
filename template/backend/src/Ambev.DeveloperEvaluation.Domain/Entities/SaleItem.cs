namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Private constructor for EF
        /// </summary>
        private SaleItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleItem"/> class with the specified sale and product details. Constructor for creating a sale item
        /// </summary>
        /// <param name="saleId">The unique identifier of the sale to which this item belongs.</param>
        /// <param name="productName">The name of the product being sold.</param>
        /// <param name="quantity">The quantity of the product being sold. Must be greater than zero and no more than 20.</param>
        /// <param name="unitPrice">The price per unit of the product. Must be greater than zero.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="quantity"/> is less than or equal to zero, greater than 20, or if <paramref
        /// name="unitPrice"/> is less than or equal to zero.</exception>
        public SaleItem(
            Guid saleId,
            string productName,
            int quantity,
            decimal unitPrice)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name is required", nameof(productName));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            if (quantity > 20)
                throw new ArgumentException("Cannot sell more than 20 identical items", nameof(quantity));

            if (unitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than zero", nameof(unitPrice));

            Id = Guid.NewGuid();
            SaleId = saleId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = 0;
            CalculateTotalAmount();
            IsCancelled = false;
        }

        public void CalculateDiscount()
        {
            // Apply business rules for discount:
            // 1. Purchases above 4 identical items have a 10% discount
            // 2. Purchases between 10 and 20 identical items have a 20% discount
            // 3. Purchases below 4 items cannot have a discount

            if (Quantity >= 10 && Quantity <= 20) // 20% discount
                Discount = Math.Round(UnitPrice * Quantity * 0.2m, 2);
            else if (Quantity >= 4) // 10% discount
                Discount = Math.Round(UnitPrice * Quantity * 0.1m, 2);
            else // No discount
                Discount = 0;

            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = Math.Round((UnitPrice * Quantity) - Discount, 2);
        }

        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
