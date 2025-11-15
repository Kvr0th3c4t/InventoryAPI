using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetProductoById;

public record GetProductoByIdQuery(
    int Id
) : IRequest<ResponseProductoDto>;