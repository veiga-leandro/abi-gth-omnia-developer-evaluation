using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.SaleItem;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _handler = new CreateSaleHandler(_saleRepository, _userRepository, _mapper, _eventPublisher);
        }

        [Fact]
        public async Task Handle_ShouldReturnCreatedSale_WhenDataIsValid()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            var sale = MapCreateSaleCommandToSale(command, "SALE-001");
            var result = MapSaleToCreateSaleResult(sale);

            _mapper.Map<CreateSaleResult>(sale).Returns(result);

            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(sale);
            _userRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(sale.Customer);

            // When
            var createSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Then
            createSaleResult.Should().NotBeNull();
            createSaleResult.Id.Should().Be(sale.Id);
            await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldCreateSaleWithCorrectDiscount_WhenQuantityIsGreaterThan4()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidWithDiscoutCommand();
            var sale = MapCreateSaleCommandToSale(command, "SALE-001");
            var result = MapSaleToCreateSaleResult(sale);

            _mapper.Map<CreateSaleResult>(sale).Returns(result);

            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                               .Returns(sale);
            _userRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(sale.Customer);

            // Act
            var createSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            createSaleResult.Should().NotBeNull();
            createSaleResult.Items.First().Discount.Should().Be(50m);
            createSaleResult.TotalAmount.Should().Be(450m); // 5 items * $90 each after 10% discount
        }


        [Fact]
        public async Task Handle_ShouldThrowException_WhenQuantityExceeds20()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidWithDiscoutCommand();
            var sale = MapCreateSaleCommandToSale(command, "SALE-001");
            var result = MapSaleToCreateSaleResult(sale);

            command.Items.First().Quantity = 21; // Set quantity to 21 to trigger the business rule exception

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task Handle_ShouldNotApplyDiscount_WhenQuantityIsLessThan4()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidWithDiscoutCommand();
            command.Items.First().Quantity = 3; // Set quantity to 3 to not apply the discount

            var sale = MapCreateSaleCommandToSale(command, "SALE-001");
            var result = MapSaleToCreateSaleResult(sale);

            _mapper.Map<CreateSaleResult>(sale).Returns(result);

            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(sale);
            _userRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(sale.Customer);

            // When
            var createSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            createSaleResult.Items.First().Discount.Should().Be(0);
            createSaleResult.TotalAmount.Should().Be(300m); // No discount applied
            await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        }


        private static Sale MapCreateSaleCommandToSale(CreateSaleCommand command, string saleNumber)
        {
            var sale = new Sale(saleNumber, command.Date, command.BranchName, new User { Id = command.CustomerId, Role = DeveloperEvaluation.Domain.Enums.UserRole.Customer });
            var saleItem = new SaleItem(sale.Id, command.Items.First().ProductName, command.Items.First().Quantity, command.Items.First().UnitPrice);
            saleItem.CalculateDiscount();
            sale.AddItem(saleItem);
            return sale;
        }

        private static CreateSaleResult MapSaleToCreateSaleResult(Sale sale)
        {
            return new CreateSaleResult
            {
                Id = sale.Id,
                Number = sale.Number,
                Date = sale.Date,
                CustomerId = sale.CustomerId,
                BranchName = sale.BranchName,
                TotalAmount = sale.TotalAmount,
                Items = new List<SaleItemResult>
                {
                    new SaleItemResult
                    {
                        ProductName = sale.Items.First().ProductName,
                        Quantity = sale.Items.First().Quantity,
                        UnitPrice = sale.Items.First().UnitPrice,
                        TotalPrice = sale.Items.First().TotalAmount,
                        Discount = sale.Items.First().Discount
                    }
                }
            };
        }
    }
}
