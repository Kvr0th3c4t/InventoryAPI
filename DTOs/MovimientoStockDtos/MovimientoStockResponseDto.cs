namespace InventoryAPI.Dtos.MovimientoStockDtos;

using InventoryAPI.Enums;

public class MovimientoStockResponseDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public int? ProveedorId { get; set; }
    public string? ProveedorNombre { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public int Cantidad { get; set; }
    public string? Razon { get; set; }
    public DateTime Fecha { get; set; }
}