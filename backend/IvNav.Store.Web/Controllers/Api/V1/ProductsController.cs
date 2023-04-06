using AutoMapper;
using IvNav.Store.Core.Commands.Product;
using IvNav.Store.Core.Queries.Product;
using IvNav.Store.Setup.Controllers.Base;
using IvNav.Store.Web.Models.V1.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Web.Controllers.Api.V1;

/// <summary>
/// Products
/// </summary>
public class ProductsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <inheritdoc />
    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Create product.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequestDto requestDto, CancellationToken cancellationToken)
    {
        var request = new CreateProductRequest(requestDto.Name!);
        var response = await _mediator.Send(request, cancellationToken);

        var responseDto = _mapper.Map<ReadProductResponseDto?>(response.Product!);

        return CreatedAtRoute(nameof(ReadProduct), new { id = response.Product!.Id }, responseDto);
    }

    /// <summary>
    /// Read product.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}", Name = nameof(ReadProduct))]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReadProductResponseDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReadProduct(Guid id, CancellationToken cancellationToken)
    {
        var request = new ReadProductRequest(id);
        var response = await _mediator.Send(request, cancellationToken);

        var responseDto = _mapper.Map<ReadProductResponseDto?>(response.Item!);

        return responseDto != null ? Ok(responseDto) : NotFound();
    }

    /// <summary>
    /// Read products.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReadProductsResponseDto))]
    public async Task<IActionResult> ReadProducts([FromQuery] ReadProductsRequestDto requestDto, CancellationToken cancellationToken)
    {
        var request = new ReadProductsRequest(requestDto.Page, requestDto.PageSize);
        var response = await _mediator.Send(request, cancellationToken);

        var responseDto = new ReadProductsResponseDto
        {
            Items = _mapper.Map<IReadOnlyCollection<ReadProductResponseDto>>(response.Items)
        };

        AddXTotalCountHeaderToResponse(response.TotalCount);

        return Ok(responseDto);
    }
}
