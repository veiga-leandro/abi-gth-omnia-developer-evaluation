using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleValidator : AbstractValidator<GetSaleCommand>
    {
        /// <summary>
        /// Initializes validation rules for the GetSaleByIdQuery
        /// </summary>
        public GetSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}
