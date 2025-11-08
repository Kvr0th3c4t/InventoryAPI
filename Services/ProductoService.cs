using InventoryAPI.Data;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;

namespace InventoryAPI.Services;

public class ProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public List<Producto> GetAll()
    {
        return _repository.GetAll();
    }

    public Producto? GetProductoById(int id)
    {
        return _repository.GetById(id);
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

        _repository.Add(producto);

        return producto;
    }

    public Producto? Update(int id, UpdateProductoDto dto)
    {
        var producto = _repository.GetById(id);

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

        _repository.Update(producto);

        return producto;
    }

    public bool Delete(int id)
    {
        return _repository.Delete(id);
    }

    private string GenerarSKU()
    {
        return "PROD-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
    }
}