using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public class ListSalesRequest : ListPaginatedRequest
    {
        /// <summary>
        /// Optional start date filter to include sales from this date
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Optional end date filter to include sales until this date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Optional customer ID filter to get sales for a specific customer
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Filter by branch name
        /// </summary>
        public string? BranchName { get; set; }

        /// <summary>
        /// Filter by sale number
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Filter by sale cancellation status
        /// </summary>
        public bool? IsCancelled { get; set; }

        /// <summary>
        /// Ordering of results (e.g., "Date desc, TotalAmount asc")
        /// </summary>
        public string OrderBy { get; set; } = "Date desc";
    }

}
