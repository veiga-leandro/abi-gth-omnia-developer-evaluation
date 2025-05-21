using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ListSalesValidator = Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales.ListSalesValidator;
using UpdateSaleValidator = Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale.UpdateSaleValidator;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Authorize(Roles = "Admin,Manager,Customer")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of AuthController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new sale
        /// </summary>
        /// <param name="sale">Sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateSaleCommand>(request);
            command.CustomerId = GetCurrentUserId();

            var result = await _mediator.Send(command);
            return Ok(_mapper.Map<CreateSaleResponse>(result));
        }

        /// <summary>
        /// Retrieves a Sale by ID
        /// </summary>
        /// <param name="id">The unique identifier of the Sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Sale details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSaleCommand(id), cancellationToken);

            if (result == null)
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Sale not found"
                });

            return Ok(_mapper.Map<GetSaleResponse>(result));
        }

        /// <summary>
        /// Retrieves a list of sales with optional filtering parameters
        /// </summary>
        /// <param name="request">Query parameters for filtering sales</param>
        /// <returns>A list of sales matching the criteria</returns>
        /// <response code="200">Returns the list of sales</response>
        /// <response code="401">Unauthorized. Please login to access this resource</response>
        /// <response code="403">Forbidden. You don't have permission to access this resource</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<PaginatedList<ListSalesResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> List([FromQuery] ListSalesRequest request, CancellationToken cancellationToken)
        {
            var validator = new ListSalesValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<ListSalesCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);
            return OkPaginated(_mapper.Map<PaginatedList<ListSalesResponse>>(result));
        }

        /// <summary>
        /// Updates an existing sale by ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale to update</param>
        /// <param name="request">Request containing the updated sale information</param>
        /// <returns>The updated sale details</returns>
        /// <response code="200">Returns the updated sale</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If the sale is not found</response>
        /// <response code="401">Unauthorized. Please login to access this resource</response>
        /// <response code="403">Forbidden. You don't have permission to access this resource</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            request.Id = id;
            request.CustomerId = GetCurrentUserId();

            var validator = new UpdateSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<UpdateSaleCommand>(request);

            var result = await _mediator.Send(command, cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<UpdateSaleResponse>(result));
        }
    }
}
