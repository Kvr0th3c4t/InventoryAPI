using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetAllProductos;

public record GetAllProductosQuery() : IRequest<List<ResponseProductoDto>>;