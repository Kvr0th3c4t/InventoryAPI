using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Services;
using MediatR;
using InventoryAPI.Features.Productos.Commands.CreateProducto;
using InventoryAPI.Features.Productos.Queries.GetProductoById;
using InventoryAPI.Features.Productos.Queries.GetAllProductos;
using InventoryAPI.Features.Productos.DeleteProducto;
using InventoryAPI.Features.Productos.Commands.UpdateProducto;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllProductosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var query = new GetProductoByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductoDto dto)
    {
        var command = new CreateProductoCommand(
            dto.Nombre,
            dto.Descripcion,
            dto.CategoriaId,
            dto.StockActual,
            dto.StockMinimo,
            dto.Precio
        );

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductoDto dto)
    {
        var command = new UpdateProductoCommand(
            id,
            dto.Nombre,
            dto.Descripcion,
            dto.StockActual,
            dto.StockMinimo,
            dto.CategoriaId,
            dto.Precio
        );

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        var query = new DeleteProductoCommand(id);
        var result = await _mediator.Send(query);

        return NoContent();
    }
}