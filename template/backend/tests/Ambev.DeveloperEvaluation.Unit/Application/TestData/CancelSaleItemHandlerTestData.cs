using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CancelSaleItemHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid sale item entities.
    /// The generated sale item will have valid:
    /// - Id (sale item id)
    /// - SaleId (sale id)
    /// </summary>
    private static readonly Faker<CancelSaleItemCommand> cancelSaleCommandFaker = new Faker<CancelSaleItemCommand>()
        .CustomInstantiator(f => new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid()));

    /// <summary>
    /// Generates a valid Sale Item entity with randomized data.
    /// The generated sale item will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale Item entity with randomly generated data.</returns>
    public static CancelSaleItemCommand GenerateValidCommand()
    {
        return cancelSaleCommandFaker.Generate();
    }
}
