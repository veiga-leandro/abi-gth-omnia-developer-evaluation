using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ListPaginatedRequest
{
    /// <summary>
    /// Page number for pagination (starts at 1)
    /// </summary>
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page
    /// </summary>
    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}