using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of CreateSaleHandler
        /// </summary>
        /// <param name="saleRepository">The sale repository</param>
        /// <param name="userRepository">The user repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="eventPublisher">The event publisher instance</param>
        public CreateSaleHandler(ISaleRepository saleRepository,
                                 IUserRepository userRepository,
                                 IMapper mapper,
                                 IEventPublisher eventPublisher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the CreateSaleCommand request
        /// </summary>
        /// <param name="command">The CreateSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 1. Get user by id
            var user = await _userRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (user is null)
                throw new DomainException("User not found");
            if (user.Role != Domain.Enums.UserRole.Customer)
                throw new DomainException("User is not customer");

            // 2. Create the sale
            var saleNumber = await GenerateSaleNumberAsync();
            var sale = new Sale(
                saleNumber,
                request.Date,
                request.BranchName,
                user
            );

            // 3. Add items to the sale
            foreach (var itemDto in request.Items)
            {
                var saleItem = new SaleItem(
                    sale.Id,
                    itemDto.ProductName,
                    itemDto.Quantity,
                    itemDto.UnitPrice
                );

                // Apply discount rules
                saleItem.CalculateDiscount();

                sale.AddItem(saleItem);
            }

            // 4. Save to database
            await _saleRepository.CreateAsync(sale, cancellationToken);

            // 5. Publish event
            await _eventPublisher.PublishAsync(new SaleCreatedEvent
            {
                SaleId = sale.Id,
                SaleNumber = sale.Number,
                Date = sale.Date,
                CustomerId = user.Id,
                Items = sale.Items.Select(i => new SaleItemEventData
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    TotalAmount = i.TotalAmount
                }).ToList()
            });

            // 6. Return result
            return _mapper.Map<CreateSaleResult>(sale);
        }

        private async Task<string> GenerateSaleNumberAsync()
        {
            // Logic to generate unique sale number
            // Ex: SALE-20250519-0001
            var date = DateTime.Now.ToString("yyyyMMdd");
            var lastSale = await _saleRepository.GetLastSaleNumberByDatePrefixAsync(date);

            int sequence = 1;
            if (lastSale is not null)
            {
                var lastSequence = int.Parse(lastSale.Number.Split('-').Last());
                sequence = lastSequence + 1;
            }

            return $"SALE-{date}-{sequence:D4}";
        }
    }
}
