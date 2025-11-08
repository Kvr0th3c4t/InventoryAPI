using InventoryAPI.Data;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly ApplicationDbContext _context;

    public ProductoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Producto Add(Producto producto)
    {
        _context.Productos.Add(producto);
        _context.SaveChanges();

        return producto;
    }

    public bool Delete(int id)
    {
        var producto = _context.Productos.FirstOrDefault(p => p.Id == id);

        if (producto == null)
            return false;

        _context.Productos.Remove(producto);
        _context.SaveChanges();

        return true;
    }

    public List<Producto> GetAll()
    {
        return _context.Productos.ToList();
    }

    public Producto? GetById(int id)
    {
        return _context.Productos.FirstOrDefault(p => p.Id == id);
    }

    public Producto? Update(Producto producto)
    {
        _context.Update(producto);
        _context.SaveChanges();
        return producto;
    }
}