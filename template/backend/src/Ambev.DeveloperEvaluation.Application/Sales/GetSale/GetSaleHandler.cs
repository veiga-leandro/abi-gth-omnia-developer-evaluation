using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the GetSaleByIdHandler
        /// </summary>
        /// <param name="saleRepository">Repository for accessing sale data</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetSaleByIdQuery by retrieving the specified sale from the repository
        /// </summary>
        /// <param name="command">Query containing the sale ID to retrieve</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response containing the sale details, or null if not found</returns>
        public async Task<GetSaleResult?> Handle(GetSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new GetSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdWithItemsAsync(command.Id, cancellationToken);

            if (sale is null)
                return null;

            var response = _mapper.Map<GetSaleResult>(sale);
            return response;
        }
    }
}