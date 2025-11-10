namespace InventoryAPI.Models;

using InventoryAPI.Enums;

public class MovimientoStock
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int? ProveedorId { get; set; }  
    public Proveedor? Proveedor { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public int Cantidad { get; set; }  
    public DateTime FechaMovimiento { get; set; }
    public string? Razon { get; set; }
}