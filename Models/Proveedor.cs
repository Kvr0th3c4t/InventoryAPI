namespace InventoryAPI.Models;
public class Proveedor
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }

    public ICollection<Producto>? Productos { get; set; }
    public ICollection<MovimientoStock>? MovimientosStock { get; set; }
}