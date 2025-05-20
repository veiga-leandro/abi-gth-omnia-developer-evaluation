using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new sale in the repository
        /// </summary>
        /// <param name="sale">The sale to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale</returns>
        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        /// <summary>
        /// Retrieves a paginated and filtered list of sales
        /// </summary>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="startDate">Optional start date filter</param>
        /// <param name="endDate">Optional end date filter</param>
        /// <param name="customerId">Optional customer ID filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple containing the list of sales and the total count</returns>
        public async Task<(List<Sale> Sales, int TotalCount)> GetListAsync(
            int pageNumber,
            int pageSize,
            DateTime? startDate,
            DateTime? endDate,
            Guid? customerId,
            CancellationToken cancellationToken = default)
        {
            // Start with base query
            IQueryable<Sale> query = _context.Sales;

            // Apply filters
            if (startDate.HasValue)
                query = query.Where(s => s.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(s => s.Date <= endDate.Value);

            if (customerId.HasValue)
                query = query.Where(s => s.CustomerId == customerId.Value);

            // Get total count for pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var sales = await query
                .OrderByDescending(s => s.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (sales, totalCount);
        }

        /// <summary>
        /// Retrieves a specific sale by ID
        /// </summary>
        /// <param name="id">Sale ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Sale entity if found, null otherwise</returns>
        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves a specific sale by ID including its items
        /// </summary>
        /// <param name="id">Sale ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Sale entity with items if found, null otherwise</returns>
        public async Task<Sale?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <summary>
        /// Updates an existing sale in the repository
        /// </summary>
        /// <param name="sale">Sale entity to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds a new sale to the repository
        /// </summary>
        /// <param name="sale">Sale entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale == null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Sale?> GetLastSaleNumberByDatePrefixAsync(string datePrefix, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Where(s => s.Number.Contains($"SALE-{datePrefix}"))
                .OrderByDescending(s => s.Number)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
