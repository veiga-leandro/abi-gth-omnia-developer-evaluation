using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    public class ListSalesHandler : IRequestHandler<ListSalesCommand, PaginatedResult<ListSalesResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the GetSalesHandler
        /// </summary>
        /// <param name="saleRepository">Repository for accessing sale data</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetSalesQuery by retrieving filtered sales from the repository
        /// </summary>
        /// <param name="command">Query containing filter parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response containing paginated sales data</returns>
        public async Task<PaginatedResult<ListSalesResult>> Handle(ListSalesCommand command, CancellationToken cancellationToken)
        {
            var validator = new ListSalesValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var (sales, totalCount) = await _saleRepository.GetListAsync(
                command.PageNumber,
                command.PageSize,
                command.StartDate,
                command.EndDate,
                command.CustomerId,
                cancellationToken
            );

            var totalPages = (int)Math.Ceiling(totalCount / (double)command.PageSize);

            var salesResult = sales.Select(_mapper.Map<ListSalesResult>).ToList();
            var response = new PaginatedResult<ListSalesResult>(salesResult, totalCount, command.PageNumber, command.PageSize);
            return response;
        }
    }
}