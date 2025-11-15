using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Events;
using InventoryAPI.Repositories;
using InventoryAPI.Services.Interfaces;
using MediatR;

namespace InventoryAPI.Features.Productos.Commands.UpdateProducto;

public class UpdateProductoCommandHandler : IRequestHandler<UpdateProductoCommand, ResponseProductoDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IEventPublisher _publisher;

    public UpdateProductoCommandHandler(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository, IEventPublisher publisher)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
        _publisher = publisher;
    }

    public Task<ResponseProductoDto> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = _productoRepository.GetById(request.Id);

        if (producto == null)
            throw new ArgumentException("El producto no existe");

        if (request.Precio.HasValue && request.Precio.Value < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        if (request.StockActual.HasValue && request.StockActual.Value < 0)
            throw new ArgumentException("El stock actual no puede ser negativo");

        if (request.StockMinimo.HasValue && request.StockMinimo.Value < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo");

        if (request.CategoriaId.HasValue)
        {
            var categoriaBuscada = _categoriaRepository.GetById(request.CategoriaId.Value);

            if (categoriaBuscada == null)
            {
                throw new ArgumentException("La categoría asignada no existe");
            }
        }

        if (request.Nombre != null) producto.Nombre = request.Nombre;
        if (request.Descripcion != null) producto.Descripcion = request.Descripcion;
        if (request.StockActual.HasValue) producto.StockActual = request.StockActual.Value;
        if (request.StockMinimo.HasValue) producto.StockMinimo = request.StockMinimo.Value;
        if (request.CategoriaId.HasValue) producto.CategoriaId = request.CategoriaId.Value;
        if (request.Precio.HasValue) producto.Precio = request.Precio.Value;

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

            _publisher.Publish(evento);
        }
        var categoria = _categoriaRepository.GetById(producto.CategoriaId);

        if (categoria == null)
        {
            throw new ArgumentException("La categoría asignada no existe");
        }

        var response = new ResponseProductoDto
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

        return Task.FromResult(response);
    }
}