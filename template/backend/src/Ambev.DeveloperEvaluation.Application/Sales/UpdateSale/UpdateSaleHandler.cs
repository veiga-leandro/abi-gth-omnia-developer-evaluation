using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the UpdateSaleCommandHandler
        /// </summary>
        public UpdateSaleHandler(
            ISaleRepository saleRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the UpdateSaleCommand by updating the specified sale in the database
        /// </summary>
        /// <param name="command">Command containing the sale updates</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response containing the updated sale details, or null if not found</returns>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            // Get existing sale with its items
            var sale = await _saleRepository.GetByIdWithItemsAsync(command.Id, cancellationToken);

            if (sale is null)
                return null;

            // Check if sale is already cancelled
            if (sale.IsCancelled)
                throw new DomainException("Cannot update a cancelled sale");

            // 1. Get user by id
            var user = await _userRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (user is null)
                throw new DomainException("User not found");
            if (user.Role != Domain.Enums.UserRole.Customer)
                throw new DomainException("User is not customer");

            var validator = new UpdateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            // Update sale properties
            sale.Date = command.Date;
            sale.CustomerId = command.CustomerId;
            sale.BranchName = command.BranchName;

            // Handle item updates
            await UpdateSaleItems(sale, command.Items, cancellationToken);

            // Update in database
            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // 5. Publish event
            await _eventPublisher.PublishAsync(new SaleModifiedEvent
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

            // Create response
            sale.Items = sale.Items.Where(i => !i.IsCancelled).ToList();
            return _mapper.Map<UpdateSaleResult>(sale);
        }

        /// <summary>
        /// Updates the items in a sale based on the command
        /// </summary>
        private async Task UpdateSaleItems(
            Sale sale,
            List<UpdateSaleItemCommand> itemCommands,
            CancellationToken cancellationToken)
        {
            // Process each item in the command
            foreach (var itemCommand in itemCommands)
            {
                if (itemCommand.IsCancelled)
                {
                    // Remove existing items marked for deletion
                    if (itemCommand.Id.HasValue)
                    {
                        var itemToRemove = sale.Items.FirstOrDefault(i => i.Id == itemCommand.Id.Value);
                        if (itemToRemove != null)
                        {
                            itemToRemove.IsCancelled = true;
                            sale.RecalculateTotalAmount();
                        }
                    }
                }
                else if (itemCommand.Id.HasValue)
                {
                    // Update existing items
                    var existingItem = sale.Items.FirstOrDefault(i => i.Id == itemCommand.Id.Value);
                    if (existingItem != null)
                    {
                        // Update item properties
                        existingItem.ProductName = itemCommand.ProductName;
                        existingItem.Quantity = itemCommand.Quantity;
                        existingItem.UnitPrice = itemCommand.UnitPrice;
                        existingItem.CalculateDiscount();
                    }
                }
                else
                {
                    var newItem = new Domain.Entities.SaleItem
                    (
                        sale.Id,
                        itemCommand.ProductName,
                        itemCommand.Quantity,
                        itemCommand.UnitPrice
                    );

                    newItem.CalculateDiscount();
                    sale.AddItem(newItem);
                }
            }

            // Ensure at least one item remains
            if (!sale.Items.Any())
                throw new ValidationException("Sale must have at least one item");
        }
    }
}
