using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Productos.DeleteProducto;

public class DeleteProductoCommandHandler : IRequestHandler<DeleteProductoCommand, bool>
{
    private readonly IProductoRepository _productoRepository;

    public DeleteProductoCommandHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }
    public Task<bool> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
    {
        var eliminado = _productoRepository.Delete(request.Id);

        if (!eliminado)
            throw new KeyNotFoundException($"No se puede eliminar. El producto con Id {request.Id} no existe");

        return Task.FromResult(true);
    }
}