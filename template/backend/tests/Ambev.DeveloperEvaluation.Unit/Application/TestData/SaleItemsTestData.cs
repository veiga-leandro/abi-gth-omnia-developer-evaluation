using Ambev.DeveloperEvaluation.Application.Sales.SaleItem;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleItemsTestData
{
    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// The generated users will have valid:
    /// - Username (using internet usernames)
    /// - Password (meeting complexity requirements)
    /// - Email (valid format)
    /// - Phone (Brazilian format)
    /// - Status (Active or Suspended)
    /// - Role (Customer or Admin)
    /// </summary>
    private static readonly Faker<SaleItemDto> createSaleItemDtoFaker = new Faker<SaleItemDto>()
        .RuleFor(u => u.ProductName, f => $"Product-@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.Quantity, f => f.Random.Number(1, 20))
        .RuleFor(u => u.UnitPrice, f => f.Random.Decimal(0.1M, 9999.99M));

    private static readonly Faker<SaleItemDto> createSaleItemDtoWithDiscountFaker = new Faker<SaleItemDto>()
        .RuleFor(u => u.ProductName, f => $"Product-@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.Quantity, f => 5)
        .RuleFor(u => u.UnitPrice, f => 100m);

    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid User entity with randomly generated data.</returns>
    public static SaleItemDto GenerateValidItem()
    {
        return createSaleItemDtoFaker.Generate();
    }

    public static SaleItemDto GenerateValidWithDiscountItem()
    {
        return createSaleItemDtoWithDiscountFaker.Generate();
    }
}
