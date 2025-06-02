using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CancelSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// The generated users will have valid:
    /// - Id (sale id)
    /// </summary>
    private static readonly Faker<CancelSaleCommand> cancelSaleCommandFaker = new Faker<CancelSaleCommand>()
        .CustomInstantiator(f => new CancelSaleCommand(Guid.NewGuid()));

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated sale will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static CancelSaleCommand GenerateValidCommand()
    {
        return cancelSaleCommandFaker.Generate();
    }
}
