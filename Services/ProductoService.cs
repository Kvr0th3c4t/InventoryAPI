using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Events;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.Services.Interfaces;

namespace InventoryAPI.Services;

public class ProductoService
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IEventPublisher _eventpublisher;

    public ProductoService(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository, IEventPublisher eventPublisher)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
        _eventpublisher = eventPublisher;
    }

    public List<ResponseProductoDto> GetAll()
    {
        var productos = _productoRepository.GetAll();

        return productos
            .Select(producto => MapToResponseDto(producto))
            .ToList();
    }

    public ResponseProductoDto? GetProductoById(int id)
    {
        var producto = _productoRepository.GetById(id);

        if (producto == null)
        {
            return null;
        }

        return MapToResponseDto(producto);
    }

    public ResponseProductoDto Create(CreateProductoDto dto)
    {
        if (dto.Precio < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        if (dto.StockActual < 0)
            throw new ArgumentException("El stock actual no puede ser negativo");

        if (dto.StockMinimo < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo");

        var categoriaBuscada = _categoriaRepository.GetById(dto.CategoriaId);

        if (categoriaBuscada == null)
        {
            throw new ArgumentException("La categoría asignada no existe");
        }

        string sku = GenerarSKU();

        var producto = new Producto
        {
            Nombre = dto.Nombre,
            SKU = sku,
            CategoriaId = dto.CategoriaId,
            Descripcion = dto.Descripcion,
            StockActual = dto.StockActual,
            StockMinimo = dto.StockMinimo,
            Precio = dto.Precio,
            FechaCreacion = DateTime.Now
        };

        var productoCreado = _productoRepository.Add(producto);

        var response = MapToResponseDto(productoCreado, categoriaBuscada);

        return response;
    }

    public ResponseProductoDto? Update(int id, UpdateProductoDto dto)
    {
        var producto = _productoRepository.GetById(id);

        if (producto == null)
            return null;

        if (dto.Precio.HasValue && dto.Precio.Value < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        if (dto.StockActual.HasValue && dto.StockActual.Value < 0)
            throw new ArgumentException("El stock actual no puede ser negativo");

        if (dto.StockMinimo.HasValue && dto.StockMinimo.Value < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo");



        if (dto.CategoriaId.HasValue)
        {
            var categoriaBuscada = _categoriaRepository.GetById(dto.CategoriaId.Value);

            if (categoriaBuscada == null)
            {
                throw new ArgumentException("La categoría asignada no existe");
            }
        }

        if (dto.Nombre != null) producto.Nombre = dto.Nombre;
        if (dto.Descripcion != null) producto.Descripcion = dto.Descripcion;
        if (dto.StockActual.HasValue) producto.StockActual = dto.StockActual.Value;
        if (dto.StockMinimo.HasValue) producto.StockMinimo = dto.StockMinimo.Value;
        if (dto.CategoriaId.HasValue) producto.CategoriaId = dto.CategoriaId.Value;
        if (dto.Precio.HasValue) producto.Precio = dto.Precio.Value;

        _productoRepository.Update(producto);

        if (producto.StockActual < producto.StockMinimo)
        {
            var evento = new StockBajoEvent
            {
                ProductoId = producto.Id,
                ProductoNombre = producto.Nombre,
                StockActual = producto.StockActual,
                StockMinimo = producto.StockMinimo,
                FechaEvento = DateTime.Now
            };

            _eventpublisher.Publish(evento);
        }

        return MapToResponseDto(producto);

    }

    public bool Delete(int id)
    {
        return _productoRepository.Delete(id);
    }

    private string GenerarSKU()
    {
        return "PROD-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
    }

    private ResponseProductoDto MapToResponseDto(Producto producto, Categoria categoria)
    {
        return new ResponseProductoDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            SKU = producto.SKU,
            CategoriaId = producto.CategoriaId,
            CategoriaNombre = categoria.Nombre,
            StockActual = producto.StockActual,
            Precio = producto.Precio
        };
    }
    private ResponseProductoDto MapToResponseDto(Producto producto)
    {
        var categoria = _categoriaRepository.GetById(producto.CategoriaId);
        if (categoria == null)
            throw new InvalidOperationException($"Producto {producto.Id} tiene categoría no válida");

        return MapToResponseDto(producto, categoria);
    }
}