using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Services;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _productoService;

    public ProductosController(ProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public ActionResult<List<ResponseProductoDto>> GetAll()
    {
        var productos = _productoService.GetAll();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public ActionResult<ResponseProductoDto> GetProductById(int id)
    {
        var producto = _productoService.GetProductoById(id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost]
    public ActionResult<ResponseProductoDto> Create([FromBody] CreateProductoDto dto)
    {
        try
        {
            var producto = _productoService.Create(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = producto.Id }, producto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public ActionResult<ResponseProductoDto> UpdateProduct(int id, [FromBody] UpdateProductoDto dto)
    {
        try
        {
            var producto = _productoService.Update(id, dto);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteProducto(int id)
    {
        bool eliminado = _productoService.Delete(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}