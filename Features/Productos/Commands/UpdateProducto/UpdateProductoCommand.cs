using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Productos.Commands.UpdateProducto;

public record UpdateProductoCommand
(
    int Id,
    string? Nombre,
    string? Descripcion,
    int? StockActual,
    int? StockMinimo,
    int? CategoriaId,
    decimal? Precio
) : IRequest<ResponseProductoDto>;