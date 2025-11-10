using InventoryAPI.Data;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories;

public class MovimientosStockRepository : IMovimientoStockRepository
{
    private readonly ApplicationDbContext _context;

    public MovimientosStockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public MovimientoStock Add(MovimientoStock movimientoStock)
    {
        _context.MovimientosStock.Add(movimientoStock);
        _context.SaveChanges();

        return movimientoStock;
    }

    public bool Delete(int id)
    {
        var movimiento = _context.MovimientosStock.FirstOrDefault(m => m.Id == id);

        if (movimiento == null) return false;

        _context.MovimientosStock.Remove(movimiento);
        _context.SaveChanges();

        return true;
    }

    public List<MovimientoStock> GetAll()
    {
        return _context.MovimientosStock.ToList();
    }

    public MovimientoStock? GetById(int id)
    {
        return _context.MovimientosStock.FirstOrDefault(m => m.Id == id);

    }

    public MovimientoStock? Update(MovimientoStock movimientoStock)
    {
        _context.MovimientosStock.Update(movimientoStock);
        _context.SaveChanges();

        return movimientoStock;
    }
}