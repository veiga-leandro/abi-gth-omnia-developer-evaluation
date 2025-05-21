using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public class ListSalesValidator : AbstractValidator<ListSalesRequest>
    {
        /// <summary>
        /// Initializes validation rules for the ListSalesRequest
        /// </summary>
        public ListSalesValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100");

            When(x => x.StartDate.HasValue && x.EndDate.HasValue, () =>
            {
                RuleFor(x => x.EndDate)
                    .GreaterThanOrEqualTo(x => x.StartDate.Value)
                    .WithMessage("End date must be greater than or equal to start date");
            });
        }
    }
}
