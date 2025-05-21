using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleValidator
        /// </summary>
        public UpdateSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Sale ID is required");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Sale date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required");

            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("Branch Name is required");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required")
                .Must(items => items.Any(i => !i.IsCancelled)).WithMessage("Sale must have at least one non-canceled item");

            RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemValidator());
        }
    }
}
