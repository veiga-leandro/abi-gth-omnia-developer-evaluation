using Ambev.DeveloperEvaluation.Application.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    public class ListSalesCommand : IRequest<PaginatedResult<ListSalesResult>>
    {
        /// <summary>
        /// Page number for pagination (starts at 1)
        /// </summary>
        public int PageNumber { get; init; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; init; }

        /// <summary>
        /// Optional start date filter to include sales from this date
        /// </summary>
        public DateTime? StartDate { get; init; }

        /// <summary>
        /// Optional end date filter to include sales until this date
        /// </summary>
        public DateTime? EndDate { get; init; }

        /// <summary>
        /// Optional customer ID filter to get sales for a specific customer
        /// </summary>
        public Guid? CustomerId { get; init; }

        /// <summary>
        /// Initializes a new instance of the GetSalesQuery
        /// </summary>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="startDate">Optional start date filter</param>
        /// <param name="endDate">Optional end date filter</param>
        /// <param name="customerId">Optional customer ID filter</param>
        public ListSalesCommand(int pageNumber = 1, int pageSize = 10, DateTime? startDate = null,
                            DateTime? endDate = null, Guid? customerId = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            StartDate = startDate;
            EndDate = endDate;
            CustomerId = customerId;
        }
    }
}
