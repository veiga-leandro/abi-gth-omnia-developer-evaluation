using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

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
    Task<(List<Sale> Sales, int TotalCount)> GetListAsync(
        int pageNumber,
        int pageSize,
        DateTime? startDate,
        DateTime? endDate,
        Guid? customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific sale by ID
    /// </summary>
    /// <param name="id">Sale ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Sale entity if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific sale by ID including its items
    /// </summary>
    /// <param name="id">Sale ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Sale entity with items if found, null otherwise</returns>
    Task<Sale?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale in the repository
    /// </summary>
    /// <param name="sale">Sale entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Sale?> GetLastSaleNumberByDatePrefixAsync(string datePrefix, CancellationToken cancellationToken = default);
}
