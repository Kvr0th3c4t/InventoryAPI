using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Data;

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
    public ActionResult<Producto> Create([FromBody] Producto producto)
    {
        producto.FechaCreacion = DateTime.Now;

        _context.Productos.Add(producto);
        _context.SaveChanges();  // ← Guarda en la BD

        return CreatedAtAction(nameof(GetProductById), new { id = producto.Id }, producto);
    }
}