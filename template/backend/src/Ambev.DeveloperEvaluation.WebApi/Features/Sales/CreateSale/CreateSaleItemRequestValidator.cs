﻿using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public CreateSaleItemRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0");
        }
    }
}
