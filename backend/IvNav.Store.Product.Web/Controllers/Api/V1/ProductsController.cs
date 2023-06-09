using AutoMapper;
using IvNav.Store.Product.Core.Commands.Product;
using IvNav.Store.Product.Core.Queries.Product;
using IvNav.Store.Product.Web.Models.V1.Product;
using IvNav.Store.Setup.Attributes;
using IvNav.Store.Setup.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Product.Web.Controllers.Api.V1;

/// <summary>
/// Products
/// </summary>
public class ProductsController : ApiControllerVersionedBaseSecure
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
    [OnlyForUserAuthorize]
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
    [HttpGet("{id}", Name = nameof(ReadProduct))]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReadProductResponseDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReadProduct(Guid id, CancellationToken cancellationToken)
    {
        var request = new ReadProductRequest(id);
        var response = await _mediator.Send(request, cancellationToken);

        if (response == ReadProductResponse.NotExists)
        {
            return NotFound();
        }

        var responseDto = _mapper.Map<ReadProductResponseDto?>(response.Item!);
        return Ok(responseDto);
    }

    /// <summary>
    /// Read products.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReadProductsResponseDto))]
    public async Task<IActionResult> ReadProducts([FromQuery] ReadProductsRequestDto requestDto, CancellationToken cancellationToken)
    {
        var request = new ReadProductsRequest(requestDto.Offset, requestDto.Limit);
        var response = await _mediator.Send(request, cancellationToken);

        var responseDto = new ReadProductsResponseDto
        {
            Items = _mapper.Map<IReadOnlyCollection<ReadProductResponseDto>>(response.Items),
            TotalCount = response.TotalCount,
        };

        AddXTotalCountHeaderToResponse(response.TotalCount);

        return Ok(responseDto);
    }
}
