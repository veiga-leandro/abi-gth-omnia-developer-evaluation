namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string BranchName { get; set; }

        public Guid CustomerId { get; set; }
        public User Customer { get; set; }

        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancellationDate { get; set; }

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
        /// <param name="branchName"></param>
        /// <param name="user"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Sale(
            string number,
            DateTime date,
            string branchName,
            User user)
        {
            if (string.IsNullOrWhiteSpace(branchName))
                throw new ArgumentException("Branch name is required", nameof(branchName));

            if (user == null)
                throw new ArgumentNullException(nameof(user), "User is required");

            Id = Guid.NewGuid();
            Number = number;
            Date = date;
            BranchName = branchName;
            CustomerId = user.Id;
            Customer = user;
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

        public void RecalculateTotalAmount()
        {
            TotalAmount = Items.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount);
        }

        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
