using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Data;
using InventoryAPI.Dtos.ProductoDtos;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<List<Producto>> GetAll()
    {
        var productos = _context.Productos.ToList();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public ActionResult<Producto> GetProductById(int id)
    {
        var producto = _context.Productos.FirstOrDefault(p => p.Id == id);

        if (producto == null)
        {
            return NotFound();
        }

        return Ok(producto);
    }

    [HttpPost]
    public ActionResult<Producto> Create([FromBody] CreateProductoDto dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            SKU = dto.SKU,
            Descripcion = dto.Descripcion,
            StockActual = dto.StockActual,
            StockMinimo = dto.StockMinimo,
            Precio = dto.Precio,
            FechaCreacion = DateTime.Now
        };

        _context.Productos.Add(producto);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetProductById), new { id = producto.Id }, producto);
    }

    [HttpPut("{id}")]
    public ActionResult<Producto> UpdateProduct(int id, [FromBody] UpdateProductoDto dto)
    {
        var productoEncontrado = _context.Productos.FirstOrDefault(p => p.Id == id);

        if (productoEncontrado == null)
        {
            return NotFound();
        }

        if (dto.Nombre != null) productoEncontrado.Nombre = dto.Nombre;
        if (dto.SKU != null) productoEncontrado.SKU = dto.SKU;
        if (dto.Descripcion != null) productoEncontrado.Descripcion = dto.Descripcion;
        if (dto.StockActual.HasValue) productoEncontrado.StockActual = dto.StockActual.Value;
        if (dto.StockMinimo.HasValue) productoEncontrado.StockMinimo = dto.StockMinimo.Value;
        if (dto.Precio.HasValue) productoEncontrado.Precio = dto.Precio.Value;

        _context.SaveChanges();

        return Ok(productoEncontrado);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteProducto(int id)
    {
        var productoExistente = _context.Productos.FirstOrDefault(p => p.Id == id);

        if (productoExistente == null)
        {
            return NotFound();
        }

        _context.Productos.Remove(productoExistente);
        _context.SaveChanges();

        return NoContent();
    }
}