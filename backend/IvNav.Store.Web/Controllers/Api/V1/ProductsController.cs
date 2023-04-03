using AutoMapper;
using IvNav.Store.Core.Commands.Product;
using IvNav.Store.Core.Queries.Product;
using IvNav.Store.Setup.Controllers.Base;
using IvNav.Store.Setup.Models;
using IvNav.Store.Web.Models.V1.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Web.Controllers.Api.V1;

/// <summary>
/// Products
/// </summary>
public class ProductsController : ApiControllerBaseSecure
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
    /// Create product
    /// </summary>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateProductRequestDto requestDto, CancellationToken cancellationToken)
    {
        var request = new CreateProductRequest(requestDto.Name!);
        var response = await _mediator.Send(request, cancellationToken);

        return CreatedAtRoute(nameof(GetProduct), new { id = response.ProductId! }, new IdDto<Guid> { Id = response.ProductId!.Value });
    }

    /// <summary>
    /// Get product
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}", Name = nameof(GetProduct))]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReadProductResponseDto))]
    public IActionResult GetProduct(Guid id, CancellationToken cancellationToken)
    {
        return Ok(id);
    }

    /// <summary>
    /// Get product
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReadProductsResponseDto))]
    public async Task<IActionResult> GetProducts(ReadProductsRequestDto requestDto, CancellationToken cancellationToken)
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
