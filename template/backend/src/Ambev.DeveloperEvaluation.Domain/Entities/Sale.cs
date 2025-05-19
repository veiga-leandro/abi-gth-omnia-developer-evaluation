namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }

        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();

        /// <summary>
        /// Private constructor for EF
        /// </summary>
        private Sale() { }

        /// <summary>
        /// Constructor for creating a sale
        /// </summary>
        /// <param name="number"></param>
        /// <param name="date"></param>
        /// <param name="customerName"></param>
        /// <param name="branchName"></param>
        /// <param name="user"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Sale(
            string number,
            DateTime date,
            string customerName,
            string branchName,
            User user)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name is required", nameof(customerName));

            if (string.IsNullOrWhiteSpace(branchName))
                throw new ArgumentException("Branch name is required", nameof(branchName));

            if (user == null)
                throw new ArgumentNullException(nameof(user), "User is required");

            Id = Guid.NewGuid();
            Number = number;
            Date = date;
            CustomerName = customerName;
            BranchName = branchName;
            UserId = user.Id;
            User = user;
            TotalAmount = 0;
            IsCancelled = false;
            Items = new List<SaleItem>();
        }

        public void AddItem(SaleItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Sale item cannot be null");

            Items.Add(item);
            RecalculateTotalAmount();
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = Items.Sum(i => i.TotalAmount);
        }

        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
