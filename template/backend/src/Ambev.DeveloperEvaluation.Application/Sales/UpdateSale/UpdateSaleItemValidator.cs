using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleItemValidator
        /// </summary>
        public UpdateSaleItemValidator()
        {
            When(x => !x.IsCancelled, () =>
            {
                RuleFor(x => x.ProductName)
                    .NotEmpty().WithMessage("Product name is required");

                RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero");

                RuleFor(x => x.UnitPrice)
                    .GreaterThan(0).WithMessage("Unit price must be greater than zero");
            });
        }
    }
}
