using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
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
    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(u => u.Date, f => DateTime.UtcNow)
        .RuleFor(u => u.BranchName, f => $"Branch-@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.CustomerId, f => Guid.NewGuid())
        .RuleFor(u => u.Items, [SaleItemsTestData.GenerateValidItem()]);

    private static readonly Faker<CreateSaleCommand> createSaleCommandWithDiscountFaker = new Faker<CreateSaleCommand>()
        .RuleFor(u => u.Date, f => DateTime.UtcNow)
        .RuleFor(u => u.BranchName, f => $"Branch-@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.CustomerId, f => Guid.NewGuid())
        .RuleFor(u => u.Items, [SaleItemsTestData.GenerateValidWithDiscountItem()]);

    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid User entity with randomly generated data.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid User entity with randomly generated data.</returns>
    public static CreateSaleCommand GenerateValidWithDiscoutCommand()
    {
        return createSaleCommandWithDiscountFaker.Generate();
    }
}
