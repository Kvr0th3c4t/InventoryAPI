using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetProductoById;

public class GetProductoByIdQueryHandler : IRequestHandler<GetProductoByIdQuery, ResponseProductoDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public GetProductoByIdQueryHandler(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public Task<ResponseProductoDto> Handle(GetProductoByIdQuery request, CancellationToken cancellationToken)
    {
        var producto = _productoRepository.GetById(request.Id);

        if (producto == null)
            throw new KeyNotFoundException($"Producto con ID {request.Id} no encontrado");

        var categoria = _categoriaRepository.GetById(producto.CategoriaId);

        if (categoria == null)
            throw new InvalidOperationException($"Producto {producto.Id} tiene categoría no válida");

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