using InventoryAPI.Models;

namespace InventoryAPI.Repositories;
public interface IProductoRepository
{
    List<Producto> GetAll();
    Producto? GetById(int id);
    Producto Add(Producto producto);
    Producto? Update(Producto producto);
    bool Delete(int id);
}
