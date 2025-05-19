using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required")
                .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters");

            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("Branch name is required")
                .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required");

            RuleForEach(x => x.Items).SetValidator(new CreateSaleItemRequestValidator());
        }
    }
}
