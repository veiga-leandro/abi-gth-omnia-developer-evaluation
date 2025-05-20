using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("Branch name is required")
                .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer id is required");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required");

            RuleForEach(x => x.Items).SetValidator(new SaleItemValidator());
        }
    }
}
