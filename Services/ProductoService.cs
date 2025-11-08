using InventoryAPI.Data;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Models;

namespace InventoryAPI.Services;

public class ProductoService
{
    private readonly ApplicationDbContext _context;

    public ProductoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Producto> GetAll()
    {
        return _context.Productos.ToList();
    }

    public Producto? GetProductoById(int id)
    {
        return _context.Productos.FirstOrDefault(p => p.Id == id);
    }

    public Producto Create(CreateProductoDto dto)
    {
        if (dto.Precio < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        if (dto.StockActual < 0)
            throw new ArgumentException("El stock actual no puede ser negativo");

        if (dto.StockMinimo < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo");

        string sku = GenerarSKU();

        var producto = new Producto
        {
            Nombre = dto.Nombre,
            SKU = sku,
            Descripcion = dto.Descripcion,
            StockActual = dto.StockActual,
            StockMinimo = dto.StockMinimo,
            Precio = dto.Precio,
            FechaCreacion = DateTime.Now
        };

        _context.Productos.Add(producto);
        _context.SaveChanges();

        return producto;
    }

    public Producto? Update(int id, UpdateProductoDto dto)
    {
        var producto = _context.Productos.FirstOrDefault(p => p.Id == id);

        if (producto == null)
            return null;

        if (dto.Precio.HasValue && dto.Precio.Value < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        if (dto.StockActual.HasValue && dto.StockActual.Value < 0)
            throw new ArgumentException("El stock actual no puede ser negativo");

        if (dto.StockMinimo.HasValue && dto.StockMinimo.Value < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo");

        if (dto.Nombre != null) producto.Nombre = dto.Nombre;
        if (dto.Descripcion != null) producto.Descripcion = dto.Descripcion;
        if (dto.StockActual.HasValue) producto.StockActual = dto.StockActual.Value;
        if (dto.StockMinimo.HasValue) producto.StockMinimo = dto.StockMinimo.Value;
        if (dto.Precio.HasValue) producto.Precio = dto.Precio.Value;

        _context.SaveChanges();

        return producto;
    }

    public bool Delete(int id)
    {
        var producto = _context.Productos.FirstOrDefault(p => p.Id == id);

        if (producto == null)
            return false;

        _context.Productos.Remove(producto);
        _context.SaveChanges();

        return true;
    }

    private string GenerarSKU()
    {
        return "PROD-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
    }
}