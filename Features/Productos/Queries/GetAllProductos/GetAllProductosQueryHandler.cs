using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetAllProductos;

public class GetAllProductosQueryHandler : IRequestHandler<GetAllProductosQuery, List<ResponseProductoDto>>
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public GetAllProductosQueryHandler(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
        _productoRepository = productoRepository;
    }

    public Task<List<ResponseProductoDto>> Handle(GetAllProductosQuery request, CancellationToken cancellationToken)
    {
        var productos = _productoRepository.GetAll();

        var result = productos
            .Select(producto =>
            {
                var categoria = _categoriaRepository.GetById(producto.CategoriaId);

                if (categoria == null)
                    throw new InvalidOperationException(
                        $"Producto {producto.Id} tiene categoría inválida {producto.CategoriaId}");


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
            })
            .ToList();

        return Task.FromResult(result);
    }
}