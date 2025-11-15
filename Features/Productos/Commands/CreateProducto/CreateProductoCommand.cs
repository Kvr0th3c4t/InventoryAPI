using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Productos.Commands.CreateProducto;

public record CreateProductoCommand
(
    string Nombre,
    string? Descripcion,
    int CategoriaId,
    int StockActual,
    int StockMinimo,
    decimal Precio
) : IRequest<ResponseProductoDto>;