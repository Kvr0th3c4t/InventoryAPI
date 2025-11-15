using MediatR;

namespace InventoryAPI.Features.Productos.DeleteProducto;

public record DeleteProductoCommand(int Id) : IRequest<bool>;