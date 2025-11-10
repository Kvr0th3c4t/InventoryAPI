using InventoryAPI.Data;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    private readonly ApplicationDbContext _context;

    public ProveedorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Proveedor Add(Proveedor proveedor)
    {
        _context.Proveedores.Add(proveedor);
        _context.SaveChanges();
        return proveedor;
    }

    public bool Delete(int id)
    {
        var proveedor = _context.Proveedores.FirstOrDefault(p => p.Id == id);

        if (proveedor == null) return false;

        _context.Proveedores.Remove(proveedor);
        _context.SaveChanges();

        return true;

    }

    public List<Proveedor> GetAll()
    {
        return _context.Proveedores.ToList();
    }

    public Proveedor? GetById(int id)
    {
        return _context.Proveedores.FirstOrDefault(p => p.Id == id);
    }

    public Proveedor? Update(Proveedor proveedor)
    {
        _context.Proveedores.Update(proveedor);
        _context.SaveChanges();
        return proveedor;
    }
}