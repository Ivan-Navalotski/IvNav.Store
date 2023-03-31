using IvNav.Store.Core.Commands.CreateProduct;
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

    /// <inheritdoc />
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create product
    /// </summary>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateProductRequestDto requestDto)
    {
        var response = await _mediator.Send(new CreateProductRequest(requestDto.Name!));

        return CreatedAtRoute(nameof(GetProduct), new { id = response.ProductId! }, new IdDto<Guid> { Id = response.ProductId!.Value });
    }

    /// <summary>
    /// Get product
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}", Name = nameof(GetProduct))]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(GetProductResponse))]
    public IActionResult GetProduct(Guid id)
    {
        return Ok(id);
    }
}
