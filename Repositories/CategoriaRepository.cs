using InventoryAPI.Data;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext _context;

    public CategoriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Categoria Add(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return categoria;
    }

    public bool Delete(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id);

        if (categoria == null)
            return false;

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return true;
    }

    public List<Categoria> GetAll()
    {
        return _context.Categorias.ToList();
    }

    public Categoria? GetById(int id)
    {
        return _context.Categorias.FirstOrDefault(c => c.Id == id);
    }

    public Categoria? Update(Categoria categoria)
    {
        _context.Update(categoria);
        _context.SaveChanges();
        return categoria;
    }
}